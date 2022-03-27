namespace QuotesEndpoint;

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Tools;

public class QuotesEndpointDefinition : IEndpointDefinition
{
    public void DefineServices(IServiceCollection services)
    {
        var theAssembly =  typeof(Quotes.Features.List).Assembly;

        services.AddMediatR(theAssembly);
        services.AddValidatorsFromAssemblies(new[] { theAssembly });
    }

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/quotes",
            async (IMediator mediator) =>
                Results.Ok(await mediator.Send(new Quotes.Features.List.Query())
                    .ConfigureAwait(false)));
        
        app.MapPost("/quotes",
            async ([FromBody] Quotes.Features.Add.Command request, IMediator mediator) =>
                Results.Ok(await mediator.Send(request)
                    .ConfigureAwait(false)));
        
        app.MapPut("/quotes",
            async ([FromBody] Quotes.Features.Update.Command request, IMediator mediator) =>
                Results.Ok(await mediator.Send(request)
                    .ConfigureAwait(false)));
    }
}