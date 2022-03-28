namespace PeopleEndpoint;

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Tools;

public class PeopleEndpointDefinition : IEndpointDefinition
{
    public void DefineServices(IServiceCollection services)
    {
        var theAssembly = typeof(People.Features.List).Assembly;

        services.AddMediatR(theAssembly);
        services.AddValidatorsFromAssemblies(new[] {theAssembly});
    }

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/people",
            async (IMediator mediator) =>
                Results.Ok(await mediator.Send(new People.Features.List.Query())
                    .ConfigureAwait(false)));

        app.MapPost("/people",
            async ([FromBody] People.Features.Add.Command request, IMediator mediator) =>
                Results.Ok(await mediator.Send(request)
                    .ConfigureAwait(false)));

        app.MapPut("/people",
            async ([FromBody] People.Features.Update.Command request, IMediator mediator) =>
                Results.Ok(await mediator.Send(request)
                    .ConfigureAwait(false)));

        app.MapDelete("/people/{id}",
            async (int id, IMediator mediator) =>
            {
                await mediator.Send(new People.Features.Delete.Command {Id = id})
                    .ConfigureAwait(false);
                return Results.Ok();
            });
    }
}