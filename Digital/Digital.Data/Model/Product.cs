using Digital.Base.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Data.Model
{
    [Table("Product")]
    public class Product : BaseModel
    {
        public string Name { get; set; }
        public string Features { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public int Stock { get; set; }
        public decimal PointEarningPercentage { get; set; }
        public decimal MaxPointAmount { get; set; }
        public virtual ICollection<ProductCategoryMap> Categories { get; set; } = new List<ProductCategoryMap>();
    }

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id).IsRequired(true).UseIdentityColumn();
            builder.Property(x => x.CreatedAt).IsRequired(false);
            builder.Property(x => x.CreatedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.UpdatedAt).IsRequired(false);
            builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(50);
            builder.Property(x => x.Description).IsRequired(true).HasMaxLength(200);
            builder.Property(x => x.Features).IsRequired(true).HasMaxLength(150);
            builder.Property(x => x.Stock).IsRequired(true);
            builder.Property(x => x.Price).IsRequired(true);
            builder.Property(x => x.IsActive).IsRequired(true).HasMaxLength(2);
            builder.Property(x => x.MaxPointAmount).IsRequired(true);
            builder.Property(x => x.PointEarningPercentage).IsRequired(true);
            //builder.Property(x => x.Categories).IsRequired(true);
        }
    }


}
