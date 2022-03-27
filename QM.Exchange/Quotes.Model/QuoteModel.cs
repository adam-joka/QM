namespace QuotesModel;

public class QuoteModel
{
    public int? Id { get; set; }
    public PersonModel? Author { get; set; }
    public string? Contents { get; set; }
}