namespace Persistence;

using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class QuotesDbContext : DbContext, IQuotesDbContext
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly QuotesDbContextConfiguration _configuration;

    public QuotesDbContext(ILoggerFactory loggerFactory, IOptions<QuotesDbContextConfiguration> configuration)
    {
        _loggerFactory = loggerFactory;
        _configuration = configuration.Value;
    }
    
    public DbSet<Quote> Quotes { get; set; }
    public DbSet<Person> Persons { get; set; }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        AddAuditInfo();
        return base.SaveChangesAsync(cancellationToken);
    }

    public Task<int> SaveChangesAsync()
    {
        AddAuditInfo();
        return base.SaveChangesAsync();
    }

    private void AddAuditInfo()
    {
        DateTime now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            entry.Entity.UpdatedOn = now;
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_configuration.DbPath}");
        
#if DEBUG
        optionsBuilder.UseLoggerFactory(_loggerFactory);  
#endif
    }
}