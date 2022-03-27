namespace Quotes.Features;

using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class Update
{
    public class Command : IRequest<int?>
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Contents { get; set; } = string.Empty;
        
        public class CommandHandler : IRequestHandler<Command, int?>
        {
            private readonly IQuotesDbContext _dbContext;
            private readonly IMediator _mediator;

            public CommandHandler(IQuotesDbContext dbContext, IMediator mediator)
            {
                _dbContext = dbContext;
                _mediator = mediator;
            }
            
            public async Task<int?> Handle(Command request, CancellationToken cancellationToken)
            {
                Quote? quoteToUpdate = await _dbContext.Quotes.FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);

                if (quoteToUpdate == null) return null;
                
                // TODO: maybe add automapper?
                quoteToUpdate.Contents = request.Contents;
                quoteToUpdate.AuthorId = request.AuthorId;

                await _dbContext.SaveChangesAsync(cancellationToken);
                await _mediator.Publish(new Notification {Id = request.Id}, cancellationToken);

                return quoteToUpdate.Id;

            }
        }
    }

    public class Notification : INotification
    {
        public int Id { get; set; }
    }
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator(IQuotesDbContext dbContext)
        {
            RuleFor(c => c.AuthorId)
                .MustAsync(async (id, cancellationToken) => await dbContext.Persons.AnyAsync(p => p.Id == id, cancellationToken))
                .WithMessage("Quote author not found.");

            RuleFor(c => c.Contents).NotEmpty();
        }
    }
    
}