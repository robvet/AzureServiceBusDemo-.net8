using System;
using System.Collections.Generic;

namespace ServiceBusSqlAdapter.Models;

public partial class Billing
{
    public int Id { get; set; }

    public string BillingNumber { get; set; } = null!;

    public string Insured { get; set; } = null!;

    public DateTime CreateDate { get; set; }
}
