using System;
using System.Collections.Generic;

namespace Inventory_manager.Models;

public partial class StockLog
{
    public int LogId { get; set; }

    public int? MaterialId { get; set; }

    public int? WarehouseId { get; set; }

    public int? RefType { get; set; }

    public int? RefId { get; set; }

    public decimal? QuantityChange { get; set; }

    public decimal? FinalQuantity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Material? Material { get; set; }

    public virtual Warehouse? Warehouse { get; set; }
}
