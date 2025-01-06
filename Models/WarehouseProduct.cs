namespace SklepMVC.Models
{
    public class WarehouseProduct
    {
        public IEnumerable<Sizes> SizesViewModel { get; set; }
        public IEnumerable<Product> ProductViewModel { get; set; }
        public IEnumerable<Warehouse> WarehouseViewModel { get; set; }
    }
}
