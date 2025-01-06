namespace SklepMVC.Models
{
    public class CustomerOrderDataDetails
    {
        public CustomerOrderData OrderData { get; set; }
        public CustomerOrder Customer { get; set; }
        public WorkerOrderData Order { get; set; }
        public List<WorkerOrderedProducts> Products { get; set; }

        public CustomerOrderDataDetails() { }
    }
}
