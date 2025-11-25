using System;
using System.Collections.Generic;

namespace Inventory_manager.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public string? PasswodHash { get; set; }

    public string? FullName { get; set; }

    public string? Role { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<InventoryIssue> InventoryIssues { get; set; } = new List<InventoryIssue>();

    public virtual ICollection<InventoryReceipt> InventoryReceipts { get; set; } = new List<InventoryReceipt>();

    public virtual ICollection<StockLog> StockLogs { get; set; } = new List<StockLog>();
}
