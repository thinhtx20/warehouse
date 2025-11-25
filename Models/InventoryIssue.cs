using System;
using System.Collections.Generic;

namespace Inventory_manager.Models;

public partial class InventoryIssue
{
    public int IssueId { get; set; }

    public string? IssueCode { get; set; }

    public int? WarehouseId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Description { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<InventoryIssueDetail> InventoryIssueDetails { get; set; } = new List<InventoryIssueDetail>();

    public virtual Warehouse? Warehouse { get; set; }
}
