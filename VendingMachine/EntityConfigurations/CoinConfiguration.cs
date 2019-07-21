using System.Data.Entity.ModelConfiguration;
using VendingMachine.Models;

namespace VendingMachine.EntityConfigurations
{
    public class CoinConfiguration : EntityTypeConfiguration<Coin>
    {
        public CoinConfiguration()
        {
            Property(c => c.Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(c => c.Value)
                .IsRequired();

            Property(c => c.Blocked)
                .IsRequired();

            Property(c => c.TotalCount)
                .IsRequired();
        }
    }
}