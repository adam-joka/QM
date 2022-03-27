using Api.Infrastructure.Behaviours;
using Application.Common.Interfaces;
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

builder.Services.ConfigureSwaggerGen(options =>
{
    options.CustomSchemaIds(x => x.FullName);
});

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

builder.Services.Configure<QuotesDbContextConfiguration>(builder.Configuration.GetSection("QuotesDbContextConfiguration"));
builder.Services.AddDbContext<QuotesDbContext>();

builder.Services.AddScoped<IQuotesDbContext>(provider => provider.GetService<QuotesDbContext>());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseEndpointDefinitions();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<QuotesDbContext>()?.Database.Migrate();
}

app.Run();