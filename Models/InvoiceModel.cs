public class InvoiceModel
{
    public int Id { get; set; }

    public DateTime InvoiceDate { get; set; }

    public string InvoiceBuyer { get; set; }

    public decimal TotalAmount { get; set; }

    public InvoiceStatus Status { get; set; }

    public ICollection<InvoiceHistory> Histories { get; set; }
        = new List<InvoiceHistory>();
}