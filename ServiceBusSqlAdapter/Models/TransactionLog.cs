namespace ServiceBusSqlAdapter.Models;

public partial class TransactionLog
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public string Insured { get; set; } = null!;

    public DateTime CreateDate { get; set; }
}
