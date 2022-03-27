namespace Quotes.Features;

using MediatR;

public class Update
{
    public class Command : IRequest<int>
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Contents { get; set; }
        
        public class CommandHandler : IRequestHandler<Command, int>
        {
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                await 
            }
        }
    }
}