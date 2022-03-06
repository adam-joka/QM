namespace Api.Features;

using MediatR;
using Models;

public class ZlotysToEuros
{
    public class Query : IRequest<ZlotysToEurosResult>
    {
        public Guid UserId { get; set; }
        public string AccountNumber { get; set; }
        
        public class QueryHandler : IRequestHandler<Query, ZlotysToEurosResult>
        {
            public Task<ZlotysToEurosResult> Handle(Query request, CancellationToken cancellationToken)
            {
                // /exchangerates/rates/a/chf/
                throw new NotImplementedException();
            }
        }
    }
}