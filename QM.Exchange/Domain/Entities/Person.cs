namespace Domain.Entities;

public class Person : BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<Quote> Quotes { get; set; } = new();
}