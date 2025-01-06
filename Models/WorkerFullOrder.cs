 namespace SklepMVC.Models
{
    public class WorkerFullOrder
    {
        public WorkerOrderData WorkerOrderData { get; set; }
        public List<WorkerOrderedProducts> WorkerOrderedProducts { get; set; }
    }
}
