namespace Tools;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

public interface IEndpointDefinition
{
    void DefineServices(IServiceCollection services);
    void DefineEndpoints(WebApplication app);
}