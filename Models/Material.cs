using System;
using System.Collections.Generic;

namespace Inventory_manager.Models;

public partial class Material
{
    public int MaterialId { get; set; }

    public string? MaterialCode { get; set; }

    public string? MaterialName { get; set; }

    public decimal Unit { get; set; }

    public int? CategoryId { get; set; }

    public int Quantity { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public virtual MaterialCategory? Category { get; set; }

    public virtual ICollection<InventoryIssueDetail> InventoryIssueDetails { get; set; } = new List<InventoryIssueDetail>();

    public virtual ICollection<InventoryReceiptDetail> InventoryReceiptDetails { get; set; } = new List<InventoryReceiptDetail>();

    public virtual ICollection<StockLog> StockLogs { get; set; } = new List<StockLog>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
