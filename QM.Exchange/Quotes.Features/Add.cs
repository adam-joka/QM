namespace Quotes.Features;

using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class Add
{
    public class Command : IRequest<int>
    {
        public int AuthorId { get; set; }
        public string Contents { get; set; } = string.Empty;
        
        public class CommandHandler : IRequestHandler<Command, int>
        {
            private readonly IQuotesDbContext _dbContext;
            private readonly IMediator _mediator;

            public CommandHandler(IQuotesDbContext dbContext, IMediator mediator)
            {
                _dbContext = dbContext;
                _mediator = mediator;
            }
            
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var newQuote = new Quote
                {
                    AuthorId = request.AuthorId,
                    Contents = request.Contents
                };

                _dbContext.Quotes.Add(newQuote);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await _mediator.Publish(new Notification {Id = newQuote.Id}, cancellationToken);

                return newQuote.Id;
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
        }
    }
}