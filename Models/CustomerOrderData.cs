namespace SklepMVC.Models
{
    public class CustomerOrderData
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string Status { get; set; }
        public decimal OrderValue { get; set; }
        public DateOnly OrderDate { get; set; }

        public CustomerOrderData() { }
    }
}
