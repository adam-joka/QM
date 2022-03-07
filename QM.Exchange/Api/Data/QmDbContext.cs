namespace Api.Data;

using Microsoft.EntityFrameworkCore;
using Models.Entities;

public class QmDbContext : DbContext
{
    private static readonly Guid TestUserId = Guid.Parse("776566c9-b005-466e-8c7a-282ea376a49a");
    private static readonly Guid TestAccountId = Guid.Parse("b59a54e0-9b61-4698-a5a4-a80259474933");
    private static readonly string TestAccountNumber = "1234567890";
    private static readonly decimal TestAccountBalance = 500;

    private static readonly Account TestAccount = new()
    {
        AccountId = TestAccountId,
        UserId = TestUserId,
        Balance = TestAccountBalance,
        AccountNumber = TestAccountNumber
    };
    
    public QmDbContext(DbContextOptions options) : base(options)  
    {  
        LoadAccounts();  
    }  
  
    private void LoadAccounts()
    {
        if (!Accounts.Any(a => a.AccountId == TestAccountId))
        {
            Accounts.Add(TestAccount);
            SaveChanges();
        }
    }  
    
    public DbSet<Account> Accounts { get; set; }
}