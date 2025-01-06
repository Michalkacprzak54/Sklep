using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SklepMVC.Models
{
    public class CustomerOrder
    {
        [DisplayName("ID")]
        public int? IdCustomer { get; set; }

        [Required(ErrorMessage = "Imie jest wymagane!")]
        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-zĄ-Źą-źĘ-ęŁ-łŃ-ńÓóŚśŻ-żŹ-ź -]{1,50}", ErrorMessage = "Imię zaczyna się z wielkiej litery")]
        [DisplayName("Imie")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane!")]
        [DataType(DataType.Text)]
        [RegularExpression(@"[A-Za-zĄ-Źą-źĘ-ęŁ-łŃ-ńÓóŚśŻ-żŹ-ź -]{1,50}", ErrorMessage = "Nazwisko zaczyna się z wielkiej litery")]
        [DisplayName("Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Numer telefonu jest wymagany!")]
        [DataType(DataType.Text)]
        [RegularExpression(@"[0-9]{9}", ErrorMessage = "Numer Telefonu w formacie 000000000")]
        [DisplayName("Numer Telefonu:")]
        public string PhoneNumber { get; set; }

        [DisplayName("Płeć")]
        public bool IsWoman { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany!")]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }

        public CustomerOrder() { }
    }
}
