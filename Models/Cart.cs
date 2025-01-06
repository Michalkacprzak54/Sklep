using System.ComponentModel.DataAnnotations;

namespace SklepMVC.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [Required]
        public int Size { get; set; }
    }
}
