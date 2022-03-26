using Api.Features;
using Api.Infrastructure.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PeopleEndpoint;
using Persistence;
using QuotesEndpoint;
using Tools;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointDefinitions
(
    typeof(QuotesEndpointDefinition), 
    typeof(PeopleEndpointDefinition));

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

builder.Services.AddDbContext<QuotesDbContext>();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<QuotesDbContext>();
    context.Database.Migrate();
}

app.Run();