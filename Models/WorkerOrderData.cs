using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;

namespace SklepMVC.Models
{
    public class WorkerOrderData
    {
        [DisplayName("Id zamówienie")]
        public int idOrder { get; set; }
        [DisplayName("Dane klienta")]
        public string Client { get; set; }
        [DisplayName("Miasto")]
        public string City { get; set; }
        [DisplayName("Kod poczowy")]
        public string Zip_code { get; set; }

        [DisplayName("Ulica")]
        public string Street { get; set; }

        [DisplayName("Numer domu")]
        public string HouseNumber { get; set; }
        [DisplayName("Numer lokalu")]
        public int? ApartmentNumber { get; set; }

        [DisplayName("Firma")]
        public string CompanyName { get; set; }

        [DisplayName("Status zamówienia")]
        public string? Status { get; set; }
        
    }
}
