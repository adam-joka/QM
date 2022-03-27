namespace QuotesModel;

public class QuoteModel
{
    public int? Id { get; set; }
    public int AuthorId { get; set; }
    public string Contents { get; set; }
}