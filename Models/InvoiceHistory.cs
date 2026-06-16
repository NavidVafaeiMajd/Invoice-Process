public class InvoiceHistory
{
    public int Id { get; set; }

    public int InvoiceId { get; set; }

    public InvoiceModel Invoice { get; set; }

    public InvoiceStatus FromStatus { get; set; }

    public InvoiceStatus ToStatus { get; set; }

    public InvoiceAction Action { get; set; }

    public DateTime CreatedAt { get; set; }
}
