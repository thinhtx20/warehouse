using System;
using System.Collections.Generic;

namespace Inventory_manager.Models;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public string? WarehouseName { get; set; }

    public string? Location { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<InventoryIssue> InventoryIssues { get; set; } = new List<InventoryIssue>();

    public virtual ICollection<InventoryReceipt> InventoryReceipts { get; set; } = new List<InventoryReceipt>();

    public virtual ICollection<StockLog> StockLogs { get; set; } = new List<StockLog>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
