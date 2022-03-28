namespace People.Features;

using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

public class Add 
{
    public class Command : IRequest<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        
        public class CommandHandler : IRequestHandler<Command, int>
        {
            private readonly IQuotesDbContext _dbContext;

            public CommandHandler(IQuotesDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var personToAdd = new Person
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName
                };

                _dbContext.Persons.Add(personToAdd);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return personToAdd.Id;
            }
        }
    }
}