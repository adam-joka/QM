namespace Quotes.Features;

using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuotesModel;

public class Random
{
    public class Query : IRequest<QuoteModel>
    {
        public class QueryHandler : IRequestHandler<Query, QuoteModel>
        {
            private readonly IQuotesDbContext _dbContext;

            public QueryHandler(IQuotesDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<QuoteModel> Handle(Query request, CancellationToken cancellationToken)
            {
                // TODO: this can be cached and updated via MediatR notifications
                int quotesCount = await _dbContext.Quotes.CountAsync(cancellationToken);

                var skip = (int) ((new System.Random()).NextDouble() * quotesCount);
                return await _dbContext.Quotes
                    .OrderBy(q => q.Id)
                    .Skip(skip)
                    .Take(1)
                    .Select(q => new QuoteModel
                    {
                        Id = q.Id,
                        Contents = q.Contents,
                        Author = new PersonModel
                        {
                            Id = q.AuthorId,
                            FirstName = q.Author.FirstName,
                            LastName = q.Author.LastName
                        }
                    }).FirstAsync(cancellationToken);
            }
        }
    }
}