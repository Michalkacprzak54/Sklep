using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SklepMVC.Models
{
    public class Warehouse
    {
        public int? Id_warehouse { get; set; }

        [DisplayName("Rozmiar")]
        public string? Size { get; set; }
        public int? Id_product { get; set; }

        [DisplayName("But")]
        public string? Shoe { get; set; }
        //public string? Color { get; set; }

        [DisplayName("Ilość")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Ilość nie może być ujemna.")]

        public int? Quantity { get; set; }

        public string? sizeAndQuantity => $"{Size} - {Quantity.ToString()}"; 
    }
}
