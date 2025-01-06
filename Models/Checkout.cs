using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SklepMVC.Models
{
    public class Checkout
    {
        public bool bussinesCheckbox { get; set; }
        public CustomerOrder? Customer { get; set; }
        public Order Order { get; set; }
        public Bussines? Bussines { get; set; }
        public List<Cart>? CartProdcts { get; set; }

        public Checkout() { }


    }
}
