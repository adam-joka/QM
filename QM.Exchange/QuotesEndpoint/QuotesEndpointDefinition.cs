namespace QuotesEndpoint;

using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
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
        throw new NotImplementedException();
    }
}