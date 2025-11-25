using System;
using System.Collections.Generic;

namespace Inventory_manager.Models;

public partial class InventoryReceipt
{
    public int ReceiptId { get; set; }

    public string? ReceiptCode { get; set; }

    public int? WarehouseId { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Description { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<InventoryReceiptDetail> InventoryReceiptDetails { get; set; } = new List<InventoryReceiptDetail>();

    public virtual Warehouse? Warehouse { get; set; }
}
