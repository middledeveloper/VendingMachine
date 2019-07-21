using System.Data.Entity.ModelConfiguration;
using VendingMachine.Models;

namespace VendingMachine.EntityConfigurations
{
    public class MachineConfiguration : EntityTypeConfiguration<Machine>
    {
        public MachineConfiguration()
        {
            Property(m => m.Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(m => m.ClientBalance)
                .IsRequired();
        }
    }
}