namespace Persistence;

using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class QuotesDbContext : DbContext, IQuotesDbContext
{
    private readonly ILoggerFactory _loggerFactory = null!;
    private readonly QuotesDbContextConfiguration _configuration = null!;

    
    public QuotesDbContext(ILoggerFactory loggerFactory, IOptions<QuotesDbContextConfiguration> configuration)
    {
        _loggerFactory = loggerFactory;
        _configuration = configuration.Value;
    }

    public QuotesDbContext(DbContextOptions<QuotesDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Quote> Quotes { get; set; } = null!;
    public DbSet<Person> Persons { get; set; } = null!;

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
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedOn = now;
            }
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