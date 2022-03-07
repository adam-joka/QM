using Api.Data;
using Api.Features;
using Api.Infrastructure.Behaviours;
using Api.Models.Configs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var theAssembly = typeof(ZlotysToEuros).Assembly;

#region MediatR
// need to register mediatr from each project
builder.Services.AddMediatR(theAssembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));


#endregion

#region Fluent Validation
builder.Services.AddValidatorsFromAssemblies(new[] { theAssembly });
#endregion



builder.Services.AddDbContext<QmDbContext>(options=>options.UseInMemoryDatabase("QmDbContext"));
builder.Services.AddScoped<QmDbContext>();

var nbpConfigSection = builder.Configuration.GetSection("NbpConfig");
builder.Services.Configure<NbpConfig>(nbpConfigSection);
var npbConfig = nbpConfigSection.Get<NbpConfig>();

builder.Services.AddHttpClient(npbConfig.Name, client =>
{
    client.BaseAddress = new Uri(npbConfig.BaseApiUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/account/{userId}/{accountNumber}/euros",
    async (string accountNumber, Guid userId, IMediator mediator) =>
    {
        var euros = await mediator.Send(
                new ZlotysToEuros.Query {AccountNumber = accountNumber, UserId = userId})
            .ConfigureAwait(false);

        return euros == null ? Results.NotFound() : Results.Ok(euros);
    });

app.Run();