using System;
using System.Collections.Generic;

namespace Inventory_manager.Models;

public partial class MaterialCategory
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
