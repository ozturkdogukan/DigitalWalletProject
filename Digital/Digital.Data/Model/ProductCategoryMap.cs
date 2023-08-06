using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digital.Data.Model
{
    [Table("ProductCategoryMap")]
    public class ProductCategoryMap
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

    }


    public class ProductCategoryMapConfiguration : IEntityTypeConfiguration<ProductCategoryMap>
    {
        public void Configure(EntityTypeBuilder<ProductCategoryMap> builder)
        {
            builder
        .HasKey(pc => new { pc.ProductId, pc.CategoryId });

            builder
                .HasOne(pc => pc.Product)
                .WithMany(p => p.Categories)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(pc => pc.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(pc => pc.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Kategorinin silinmesini engellemek için Restrict kullanılır.

        }
    }
}
