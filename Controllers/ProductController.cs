using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SklepMVC.DAL;
using SklepMVC.Models;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;

namespace SklepMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        private readonly string? _connectionString;
        private readonly GetData _getData;


        public ProductController(IConfiguration configuration, GetData getData)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ConnectionString")["Polaczenie"];
            _getData = getData;
        }

        List<Product> products = new List<Product>();
        List<Warehouse> warehouse = new List<Warehouse>();

        [HttpGet]
        public ActionResult Index(string?[] plec, string?[] marka, string? czyDlaDzieci, decimal? minPrice, decimal? maxPrice)
        {
            products = _getData.GetProducts().Where(p =>p.isAvailable=="Tak").ToList();


            var availableBrands = _getData.GetBrand();
            ViewBag.availableBrands = availableBrands;

            var availableSizes = _getData.GetSizes();
            ViewBag.availableSizes = availableSizes;

            if (plec != null && plec.Length > 0)
            {
                ViewBag.SelectedPlec = plec.ToList();
                products = products.Where(v => plec.Contains(v.Sex)).ToList();
            }

            if (marka != null && marka.Any())
            {
                ViewBag.SelectedBrand = marka.ToList();
                products = products.Where(p => marka.Contains(p.Brand)).ToList();
            }

            if (!string.IsNullOrEmpty(czyDlaDzieci))
            {
                ViewBag.CzyDlaDzieci = czyDlaDzieci;
                products = products.Where(p => p.IsForKids == czyDlaDzieci).ToList();
            }

            if (minPrice.HasValue)
            {
                products = products.Where(p => Convert.ToDecimal(p.Price) >= minPrice.Value).ToList();
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => Convert.ToDecimal(p.Price) <= maxPrice.Value).ToList();
            }




            return View(products);
        }





        [HttpGet]
        public ActionResult Details(int id)
        {
            List<Product> product = _getData.GetProductById(id);
            List<Warehouse> warehouse = _getData.GetWarehouse(id);
            List<Comment> comments;
            try
            {
                comments = _getData.GetComments(id);
            }
            catch
            {
                ViewBag.Comments = "Nie udało się pobrać komentarzy.";
                comments = new List<Comment>();
            }
             

            foreach (Comment c in comments)
            {
                c.Name = _getData.GetUserName(c.Id_client);
            }
            for(int i=warehouse.Count-1;i>=0;i--)
            {
                if (warehouse[i].Quantity == 0)
                {
                    
                    warehouse.RemoveAt(i);
                }
            }


            ProductDetails objProductDetailsViewModel = new ProductDetails();
            objProductDetailsViewModel.ProductViewModel = product;
            objProductDetailsViewModel.WarehouseViewModel = warehouse;
            objProductDetailsViewModel.CommentViewModel = comments;
            objProductDetailsViewModel.CreateCommentViewModel = new Comment();

            if (product == null)
            {
                TempData["ProductError"] = "Nie ma produktu o takim id";
                return RedirectToAction("Index");
            }
            else
            {
                return View(objProductDetailsViewModel);
            }

        }




    }

}
