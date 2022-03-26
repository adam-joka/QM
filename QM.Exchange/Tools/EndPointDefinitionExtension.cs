namespace Tools;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class EndPointDefinitionExtension
{
    public static void AddEndpointDefinitions(
        this IServiceCollection services, params Type[] scanMarkers)
    {
        var endpoints = new List<IEndpointDefinition>();

        foreach (var scanMarker in scanMarkers)
        {
            endpoints.AddRange(
                scanMarker.Assembly.ExportedTypes
                    .Where(x => typeof(IEndpointDefinition)
                        .IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(Activator.CreateInstance)
                    .Cast<IEndpointDefinition>()
            );
        }

        foreach (var endpoint in endpoints)
        {
            endpoint.DefineServices(services);
        }

        services.AddSingleton
            (endpoints as IReadOnlyCollection<IEndpointDefinition>);
    }
        
    public static void UseEndpointDefinitions(this WebApplication app)
    {
        foreach (var def in app.Services.
                     GetRequiredService<IReadOnlyCollection<IEndpointDefinition>>())
        {
            def.DefineEndpoints(app);
        }
    }
}