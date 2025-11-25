using System;
using System.Collections.Generic;

namespace Inventory_manager.Models;

public partial class InventoryReceiptDetail
{
    public int DetailId { get; set; }

    public int? ReceiptId { get; set; }

    public int? MaterialId { get; set; }

    public int Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual Material? Material { get; set; }

    public virtual InventoryReceipt? Receipt { get; set; }
}
