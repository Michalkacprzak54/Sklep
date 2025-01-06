using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SklepMVC.Models
{
    public class Bussines
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Nazwa jest wymagana!")]
        [RegularExpression(@"[A-Za-zĄ-Źą-źĘ-ęŁ-łŃ-ńÓóŚśŻ-żŹ-ź -]{1,50}", ErrorMessage = "Nazwa zaczyna się z wielkiej litery")]
        [DataType(DataType.Text)]
        [DisplayName("Nazwa")]
        public string Name { get; set; }
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
        public int? ApartmentNumber { get; set; }

        public decimal Nip { get; set; }
        public decimal Regon { get; set; }

    }
}
