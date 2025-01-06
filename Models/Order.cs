using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SklepMVC.Models
{
    public class Order
    {
        public int? Id_order { get; set; }
        [Required(ErrorMessage = "Miasto jest wymagane!")]
        [RegularExpression(@"[A-Za-zĄ-Źą-źĘ-ęŁ-łŃ-ńÓóŚśŻ-żŹ-ź -]{1,50}", ErrorMessage = "Miasto zaczyna się z wielkiej litery")]
        [DataType(DataType.Text)]
        [DisplayName("Miasto")]
        public string City { get; set; }

        [Required(ErrorMessage = "Kod pocztowy jest wymagany!")]
        [DataType(DataType.Text)]
        [DisplayName("Kod pocztowy")]
        public string ZipCode { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-zĄ-Źą-źĘ-ęŁ-łŃ-ńÓóŚśŻ-żŹ-ź -]{1,50}", ErrorMessage = "Ulica zaczyna się z wielkiej litery")]
        [DisplayName("Ulica")]
        public string? Street { get; set; }

        [Required(ErrorMessage = "Numer domu jest wymagany!")]
        [DataType(DataType.Text)]
        [DisplayName("Numer domu")]
        public string HouseNumber { get; set; }

        [DisplayName("Numer mieszkania")]
        [Range(0, int.MaxValue, ErrorMessage = "Wartość pola Numer mieszkania musi być większa lub równa 0.")]
        public int? ApartmentNumber { get; set; }
        public string? Status { get; set; }

    }
}
