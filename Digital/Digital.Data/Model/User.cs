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
    [Table("User")]
    public class User : BaseModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public byte Status { get; set; }
        public decimal DigitalWallet { get; set; }

    }


    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Id).IsRequired(true).UseIdentityColumn();
            builder.Property(x => x.CreatedAt).IsRequired(false);
            builder.Property(x => x.CreatedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.UpdatedAt).IsRequired(false);
            builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.Email).IsRequired(true).HasMaxLength(100);
            builder.Property(x => x.Password).IsRequired(true).HasMaxLength(100);
            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.LastName).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.Role).IsRequired(true).HasMaxLength(10);
            builder.Property(x => x.Status).IsRequired(true);
            builder.HasIndex(x => x.Email).IsUnique(true);

            Seed(builder);

        }

        private void Seed(EntityTypeBuilder<User> builder)
        {
            var existingUser = builder.HasData(new User
            {
                Id = 1,
                Name = "Admin",
                LastName = "User",
                Email = "admin@admin.com",
                Password = "21232f297a57a5a743894a0e4a801fc3",
                Role = "Admin",
                Status = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }
    }



}
