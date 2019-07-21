using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace VendingMachine.Models
{
    public class Product
    {
        public int Id { get; set; }
        [DisplayName("Наименование")]
        public string Name { get; set; }
        [DisplayName("Изображение")]
        public string Image { get; set; }
        [DisplayName("Стоимость")]
        public decimal Price { get; set; }
        [DisplayName("Остаток")]
        public int Storage { get; set; }

        public Machine Machine { get; set; }
        public int MachineId { get; set; }

        [NotMapped]
        [DisplayName("Изображение")]
        public HttpPostedFileBase UploadLogo { get; set; }
    }
}