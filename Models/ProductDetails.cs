namespace SklepMVC.Models
{
    public class ProductDetails
    {
        public IEnumerable<Product> ProductViewModel { get; set;}
        public IEnumerable<Warehouse> WarehouseViewModel { get; set;}
        public IEnumerable<Comment> CommentViewModel { get; set;}
        public Comment CreateCommentViewModel { get; set;}
    }
}
