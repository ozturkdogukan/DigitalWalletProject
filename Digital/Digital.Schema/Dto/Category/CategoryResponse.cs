using Digital.Base.Model;
using Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Schema.Dto.Category
{
    public class CategoryResponse : BaseResponse
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public string Tags { get; set; }
        public virtual ICollection<ProductCategoryMap> Products { get; set; }
    }
}
