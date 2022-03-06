using Api.Data;
using Api.Features;
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

#endregion

#region Fluent Validation
builder.Services.AddValidatorsFromAssemblies(new[] { theAssembly });
#endregion

builder.Services.Configure<NbpConfig>(builder.Configuration.GetSection("NbpConfig"));

builder.Services.AddDbContext<QmDbContext>(options=>options.UseInMemoryDatabase());

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

app.Run();