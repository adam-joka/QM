namespace People.Features;

using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Model;

public class List 
{
    public class Query : IRequest<List<PersonModel>>
    {
        public class QueryHandler : IRequestHandler<Query, List<PersonModel>>
        {
            private readonly IQuotesDbContext _dbContext;

            public QueryHandler(IQuotesDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            
            public async Task<List<PersonModel>> Handle(Query request, CancellationToken cancellationToken) =>
                await _dbContext.Persons.Select(p => new PersonModel
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Quotes = p.Quotes.Select(q => new QuoteModel
                    {
                        Id = q.Id,
                        Contents = q.Contents
                    }).ToList()
                }).ToListAsync(cancellationToken);
        }
    }
}