namespace Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}