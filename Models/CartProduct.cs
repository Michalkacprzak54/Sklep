using System.ComponentModel;

namespace SklepMVC.Models
{
    public class CartProduct
    {
        [DisplayName("ID")]
        public int Id_product { get; set; }

        [DisplayName("Marka")]
        public string? Brand { get; set; }

        [DisplayName("Model")]
        public string? Model { get; set; }

        [DisplayName("Kolor")]
        public string? Color { get; set; }

        [DisplayName("Cena")]
        public decimal? Price { get; set; }

        public int Id_warehouse { get; set; }
        [DisplayName("Rozmiar")]
        public string Size { get; set; }

        public byte[]? Photo { get; set; }
        public int Quantity { get; set; }

        public CartProduct() { }
    }
}
