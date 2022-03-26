namespace Domain.Entities;

public class Quote : BaseEntity
{
    public int PersonId { get; set; }
    public string Contents { get; set; }
}