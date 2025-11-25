using System;
using System.Collections.Generic;

namespace Inventory_manager.Models;

public partial class Stock
{
    public int StockId { get; set; }

    public int? WarehouseId { get; set; }

    public int? MaterialId { get; set; }

    public decimal? Quantity { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual Material? Material { get; set; }

    public virtual Warehouse? Warehouse { get; set; }
}
