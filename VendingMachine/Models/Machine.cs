using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendingMachine.Models
{
    public class Machine
    {
        public int Id { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Coin> Coins { get; set; }

        [DisplayName("Баланс")]
        public decimal ClientBalance { get; set; }
        [DisplayName("Баланс автомата")]
        [NotMapped]
        public decimal TotalBalance { get; set; }
    }
}