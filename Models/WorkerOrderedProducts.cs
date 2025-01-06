using System.ComponentModel;

namespace SklepMVC.Models
{
    public class WorkerOrderedProducts
    {

        public int Id { get; set; }

        [DisplayName("Id magazyn")]
        public int WarehouseId { get; set; }
        [DisplayName("Id zamówienie")]
        public int OrderId { get; set; }
        [DisplayName("Id klient")]
        public int CustomerId { get; set; }
        [DisplayName("Buty")]
        public string Shoes { get; set; }
        [DisplayName("Cena")]
        public decimal? Price { get; set; }
        [DisplayName("Rozmiar")]
        public string? Size { get; set; }
        [DisplayName("Kolor")]
        public string Color { get; set; }
        public byte[]? Photo { get; set; }
    }
}