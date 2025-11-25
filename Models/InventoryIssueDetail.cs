using System;
using System.Collections.Generic;

namespace Inventory_manager.Models;

public partial class InventoryIssueDetail
{
    public int DetailId { get; set; }

    public int? IssueId { get; set; }

    public int? MaterialId { get; set; }

    public decimal? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual InventoryIssue? Issue { get; set; }

    public virtual Material? Material { get; set; }
}
