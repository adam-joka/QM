namespace Application.Common.Interfaces;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public interface IQuotesDbContext
{
    public DbSet<Quote> Quotes { get; set; }
    public DbSet<Person> Persons { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    Task<int> SaveChangesAsync();
}