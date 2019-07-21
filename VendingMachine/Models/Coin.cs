using System.Collections.Generic;
using System.ComponentModel;

namespace VendingMachine.Models
{
    public class Coin
    {
        public int Id { get; set; }
        [DisplayName("Номинал")]
        public decimal Value { get; set; }
        [DisplayName("Количество")]
        public int TotalCount { get; set; }
        [DisplayName("Заблокировано")]
        public bool Blocked { get; set; }

        public Machine Machine { get; set; }
        public int MachineId { get; set; }
    }
}