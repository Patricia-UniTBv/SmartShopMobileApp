using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Category
{
    public int CategoryID { get; set; }

    public string Name { get; set; } = null!;

    public int? ParentCategoryID { get; set; }

    public virtual ICollection<Category> InverseParentCategory { get; set; } = new List<Category>();

    public virtual Category? ParentCategory { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
