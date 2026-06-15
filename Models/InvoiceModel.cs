public class InvoiceModel
{
    public int Id { get; set; }
    public DateTime InvoiceDate { get; set; }

    public string InvoiceBuyer { get; set; }

    public decimal TotalAmount { get; set; }
}