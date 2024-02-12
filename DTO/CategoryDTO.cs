using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CategoryDTO
    {
        public int CategoryID { get; set; }

        public string Name { get; set; } = null!;

        public int? ParentCategoryID { get; set; }
    }
}
