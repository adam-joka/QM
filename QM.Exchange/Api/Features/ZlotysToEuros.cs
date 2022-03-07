using Api.Data;
using Api.Models.Configs;
using Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Api.Features;

using FluentValidation;
using MediatR;
using Models;

public class ZlotysToEuros
{
    public class Query : IRequest<ZlotysToEurosResult>
    {
        public Guid UserId { get; set; }
        public string AccountNumber { get; set; } = null!;

        public class QueryHandler : IRequestHandler<Query, ZlotysToEurosResult>
        {
            private readonly IHttpClientFactory _httpClientFactory;
            private readonly QmDbContext _dbContext;
            private readonly NbpConfig _nbpConfig;

            public QueryHandler(IHttpClientFactory httpClientFactory, QmDbContext dbContext,
                IOptions<NbpConfig> nbpConfig)
            {
                _httpClientFactory = httpClientFactory;
                _dbContext = dbContext;
                _nbpConfig = nbpConfig.Value;
            }


            public async Task<ZlotysToEurosResult> Handle(Query request, CancellationToken cancellationToken)
            {
                // /exchangerates/rates/a/chf/
                // TODO: add cachinig to minimaze calls

                Account account = await _dbContext.Accounts.FirstOrDefaultAsync(a =>
                    a.UserId == request.UserId && a.AccountNumber == request.AccountNumber, cancellationToken);

                if (account == null)
                {
                    return null;
                }

                NpbResponse nbpRate = await GetExchangeRateFromNbpApi(cancellationToken);

                if (nbpRate != null && nbpRate.Rates != null && nbpRate.Rates.Any())
                {
                    decimal currentRate = nbpRate.Rates.First().Mid;

                    return new ZlotysToEurosResult
                    {
                        Zlotys = account.Balance,
                        ExchangeRate = currentRate,
                        Euros = CalculateEuros(account.Balance, currentRate)
                    };
                }

                return null;
            }

            private decimal CalculateEuros(decimal accountBalance, decimal currentRate) => 
                Math.Round(accountBalance / currentRate, 2);

            private async Task<NpbResponse> GetExchangeRateFromNbpApi(CancellationToken cancellationToken)
            {
                var httpClient = _httpClientFactory.CreateClient(_nbpConfig.Name);

                HttpResponseMessage response =
                    await httpClient.GetAsync("exchangerates/rates/a/eur/?format=json", cancellationToken);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

                return JsonConvert.DeserializeObject<NpbResponse>(responseBody);
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.AccountNumber).NotEmpty();
                RuleFor(q => q.UserId).NotEmpty();
            }
        }
    }
}