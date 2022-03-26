namespace Domain.Entities;

public class BaseEntity
{
    public int Id { get; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is BaseEntity entity)
        {
            return entity.Id == Id;
        }

        return false;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }

    public bool IsTransient()
    {
        return Id == default;
    }
}