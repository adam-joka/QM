namespace Domain.Entities;

public class Quote : BaseEntity
{
    public int AuthorId { get; set; }
    public Person Author { get; set; } = null!;
    public string Contents { get; set; } = null!;
}