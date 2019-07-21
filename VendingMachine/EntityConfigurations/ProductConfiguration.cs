using System.Data.Entity.ModelConfiguration;
using VendingMachine.Models;

namespace VendingMachine.EntityConfigurations
{
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            Property(p => p.Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            Property(p => p.Image)
                .IsRequired()
                .HasMaxLength(255);

            Property(p => p.Price)
                .IsRequired();

            Property(p => p.Storage)
                .IsRequired();
        }
    }
}