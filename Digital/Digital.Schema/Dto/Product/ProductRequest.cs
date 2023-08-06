using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Schema.Dto.Product
{
    public class ProductRequest
    {
        public string Name { get; set; }
        public string Features { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int Stock { get; set; }
        public decimal PointEarningPercentage { get; set; }
        public decimal Price { get; set; }
        public decimal MaxPointAmount { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
