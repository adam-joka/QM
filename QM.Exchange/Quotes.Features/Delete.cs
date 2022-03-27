namespace Quotes.Features;

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
            private readonly IMediator _mediator;

            public CommandHandler(IQuotesDbContext dbContext, IMediator mediator)
            {
                _dbContext = dbContext;
                _mediator = mediator;
            }
            
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                Quote? quoteToDelete = await _dbContext.Quotes.FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);
                if (quoteToDelete == null) return Unit.Value;
                
                // TODO: soft delete?
                _dbContext.Quotes.Remove(quoteToDelete);
                await _dbContext.SaveChangesAsync(cancellationToken);
                
                // TODO: need to pass better info than just Id as entity with this id is already deleted
                await _mediator.Publish(new Notification {Id = request.Id}, cancellationToken);

                return Unit.Value;
            }
        }
    }

    public class Notification : INotification
    {
        public int Id { get; set; }
        
        public class NotificationHandler : INotificationHandler<Notification>
        {
            private readonly IQuotesDbContext _dbContext;

            public NotificationHandler(IQuotesDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            
            public Task Handle(Notification notification, CancellationToken cancellationToken)
            {
                // TODO: add audit log
                return Task.CompletedTask;
            }
        }
    }
}