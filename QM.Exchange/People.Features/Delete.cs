namespace People.Features;

using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class Delete
{
    public class Command : IRequest
    {
        public int Id { get; set; }
        
        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly IQuotesDbContext _dbContext;

            public CommandHandler(IQuotesDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                Person? personToDelete = await _dbContext.Persons
                    .Include(p => p.Quotes)
                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

                if (personToDelete == null) return Unit.Value;
                
                personToDelete.Quotes.ForEach(q => _dbContext.Quotes.Remove(q));
                _dbContext.Persons.Remove(personToDelete);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}