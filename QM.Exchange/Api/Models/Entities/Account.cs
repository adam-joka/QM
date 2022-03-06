namespace Api.Models.Entities;

using System.ComponentModel.DataAnnotations;

public class Account
{
    [Key]
    public Guid AccountId { get; set; }
    
    public string AccountNumber { get; set; }
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
}