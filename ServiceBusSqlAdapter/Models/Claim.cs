using System;
using System.Collections.Generic;

namespace ServiceBusSqlAdapter.Models;

public partial class Claim
{
    public int Id { get; set; }

    public string ClaimNumber { get; set; } = null!;

    public string Insured { get; set; } = null!;

    public DateTime CreateDate { get; set; }
}
