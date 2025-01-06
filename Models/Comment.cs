using System.ComponentModel.DataAnnotations;

namespace SklepMVC.Models
{
    public class Comment
    {
        public int? Id_comment { get; set; }
        public int? Id_client_login { get; set; }
        public int Id_client { get; set; }
        public string? Name { get; set; }
        public int Id_product { get; set; }
        [Display(Name = "Tytuł")]
        public string Title { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        public DateTime Added_date { get; set; }
        [Display(Name = "Ocena")]
        [RegularExpression(@"^[1-5]$", ErrorMessage = "Ocena może być od 1 do 5")]
        public int Rate { get; set; }
        
        public Comment() { }
    }
}
