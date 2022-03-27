namespace Quotes.Features;

using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuotesModel;

public class List
{
    public class Query : IRequest<List<QuoteModel>>
    {
        public class QueryHandler : IRequestHandler<Query, List<QuoteModel>>
        {
            private readonly IQuotesDbContext _dbContext;

            public QueryHandler(IQuotesDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<List<QuoteModel>> Handle(Query request, CancellationToken cancellationToken)
                => await _dbContext.Quotes.Select(q => new QuoteModel
                {
                    Id = q.Id,
                    Author = new PersonModel
                    {
                        Id = q.AuthorId,
                        FirstName = q.Author.FirstName,
                        LastName = q.Author.LastName
                    }
                }).ToListAsync(cancellationToken);
        }
    }
}