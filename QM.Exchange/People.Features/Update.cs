namespace People.Features;

using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class Update 
{
    public class Command : IRequest<int?>
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        
        public class CommandHandler : IRequestHandler<Command, int?>
        {
            private readonly IQuotesDbContext _dbContext;

            public CommandHandler(IQuotesDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            
            public async Task<int?> Handle(Command request, CancellationToken cancellationToken)
            {
                Person? personToUpdate = await _dbContext.Persons
                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

                if (personToUpdate == null) return null;

                personToUpdate.FirstName = request.FirstName;
                personToUpdate.LastName = request.LastName;

                await _dbContext.SaveChangesAsync(cancellationToken);

                return personToUpdate.Id;
            }
        }
    }
}