using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SklepMVC.Models
{
    public class Sizes
    {
        public int? id_size { get; set; }

        [DisplayName("Rozmiar")]
        public string? size { get; set; }

        public string sizeAndNumber => $"{id_size} - {size}";
    }
}
