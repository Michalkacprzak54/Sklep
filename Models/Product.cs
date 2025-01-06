using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SklepMVC.Models
{
    public class Product
    {
        [DisplayName("ID")]
        public int Id_product { get; set; }

        [DisplayName("Marka")]
        [StringLength(50, ErrorMessage = "Nazwa nie może być dłuższa niż 50 znaków.")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string? Brand { get; set; }

        [DisplayName("Model")]
        [StringLength(50, ErrorMessage = "Nazwa nie może być dłuższa niż 50 znaków.")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string? Model { get; set; }

        [DisplayName("Kolor")]
        [RegularExpression(@"^[a-zA-ZęóąśłżźćńĘÓĄŚŁŻŹĆŃ\s]+$", ErrorMessage = "Kolor może zawierać tylko litery.")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string? Color { get; set; }

        [DisplayName("Opis")]

        [Required(ErrorMessage = "To pole jest wymagane")]
        public string? Description { get; set; }

        [DisplayName("Kod produktu")]
        [StringLength(25, ErrorMessage = "Kod nie może być dłuższy niż 25 znaków.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\.\-]+$", ErrorMessage = "Kod może zawierać tylko litery, cyfry, spacje, myślniki i kropki.")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string? ProductCode { get; set; }

        [DisplayName("Cena")]
        [RegularExpression(@"\d+([,.]\d{1,2})?", ErrorMessage = "Nieprawidłowy format liczby.")]
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string? Price { get; set; }


        [DisplayName("Czy dla dzieci")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string? IsForKids { get; set; }

        [DisplayName("Plec")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string? Sex { get; set; }

        [DataType(DataType.Upload)]
        public byte[]? Photo { get; set; }
        [DisplayName("Czy dostępny")]
        [Required(ErrorMessage = "To pole jest wymagane")]
        public string? isAvailable { get; set; }

        public Product()
        { 

        }
    }
}
