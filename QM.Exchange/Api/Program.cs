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

builder.Services.AddEndpointDefinitions(
    typeof(QuotesEndpointDefinition), 
    typeof(PeopleEndpointDefinition));

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

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