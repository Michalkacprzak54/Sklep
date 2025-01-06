using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SklepMVC.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Policy;
using SklepMVC.DAL;

namespace SklepMVC.Controllers
{
    public class WorkerProductController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        private readonly string? _connectionString;
        private readonly GetData _getData;

        public WorkerProductController(IConfiguration configuration, GetData getData)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ConnectionString")["Polaczenie"];
            _getData = getData;
        }
        List<Product> products { get; set; } = new List<Product>();
        List<Warehouse> warehouse { get; set; } = new List<Warehouse>();

        
        

        [HttpGet]
        public ActionResult Index(string? czyDostepny, string?[] plec, string?[] marka, string? czyDlaDzieci, decimal? minPrice, decimal? maxPrice)
        {

            if (Request.Cookies["CookieWorkerLoginID"] != null)
            {

                products = _getData.GetProducts();

                var availableBrands = _getData.GetBrand();
                ViewBag.availableBrands = availableBrands;

                var availableSizes = _getData.GetSizes();
                ViewBag.availableSizes = availableSizes;

                if(!string.IsNullOrEmpty(czyDostepny))
                {
                    products = products.Where(p => p.isAvailable == czyDostepny).ToList();
                }

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
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        [HttpGet]
        public ActionResult Details(int id)
        {

            if (Request.Cookies["CookieWorkerLoginID"] != null)
            {

                List<Product> product = _getData.GetProductById(id);
                List<Warehouse> warehouse = _getData.GetWarehouse(id);

                ProductDetails objProductDetailsViewModel = new ProductDetails();
                objProductDetailsViewModel.ProductViewModel = product;
                objProductDetailsViewModel.WarehouseViewModel = warehouse;

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
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        [HttpGet]
        public ActionResult Create()
        {

            if (Request.Cookies["CookieWorkerLoginID"] != null)
            {
                List<string> sex = _getData.GetSex();
                ViewBag.Sex = sex;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            
        }

        // POST: WorkerProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, IFormFile? Photo)
        {
            if (Request.Cookies["CookieWorkerLoginID"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (product.Price.Contains(','))
                        {
                            product.Price = product.Price.Replace(',', '.');
                        }
                        string errorMessage;
                        if (addProduct(product, Photo, out errorMessage))
                        {
                            TempData["ProductSuccess"] = "Produkt został dodany";
                        }
                        else
                        {
                            TempData["ProductError"] = "Dodanie produktu nie powiodło się. Spróbuj ponownie " + errorMessage;
                        }

                        return RedirectToAction("Index");


                    }
                    ViewBag.Sex = _getData.GetSex();
                    return View(product);
                }
                catch (Exception ex)
                {
                    TempData["ProductError"] = "Wystąpił błąd: " + ex.Message;
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Request.Cookies["CookieWorkerLoginID"] != null)
            {
                List<Product> product = _getData.GetProductById(id);
                ViewBag.Sex = _getData.GetSex();
                decimal decimalPrice;
                if (decimal.TryParse(product.First().Price, out decimalPrice))
                {
                    // Przypisz przekształconą wartość do modelu
                    product.First().Price = decimalPrice.ToString("F2");
                }

                if (product.Count == 0)
                {
                    TempData["ProductError"] = "Produkt o takim id nie istnieje";
                    return RedirectToAction("Index");
                }
                return View(product.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            
        }

        // POST: WorkerProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, IFormFile? Photo)
        {
            if (Request.Cookies["CookieWorkerLoginID"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (product.Price.Contains(','))
                        {
                            product.Price = product.Price.Replace(',', '.');
                        }

                        string errorMessage;
                        if (updateProduct(product, Photo, out errorMessage))
                        {
                            TempData["ProductSuccess"] = "Produkt został edytowany";
                        }
                        else
                        {
                            TempData["ProductError"] = "Edytowanie produktu nie powiodło się. Spróbuj ponownie " + errorMessage;
                        }

                        return RedirectToAction("Index");


                    }
                    ViewBag.Sex = _getData.GetSex();
                    return View(product);
                }
                catch (Exception ex)
                {
                    TempData["ProductError"] = "Wystąpił błąd: " + ex.Message;
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            
        }

        public bool addProduct(Product product, IFormFile? image, out string errorMessage)
        {
            errorMessage = null;
            byte[] imageData = null;

            if (image != null)
            {
                imageData = new byte[image.Length];
                image.OpenReadStream().Read(imageData, 0, imageData.Length);
            }

            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dodajProdukt";

                        command.Parameters.AddWithValue("@marka", product.Brand);
                        command.Parameters.AddWithValue("@model", product.Model);
                        command.Parameters.AddWithValue("@kolor", product.Color);
                        command.Parameters.AddWithValue("@opis", product.Description);
                        command.Parameters.AddWithValue("@kod_produkt", product.ProductCode);
                        command.Parameters.AddWithValue("@cena", product.Price);
                        command.Parameters.AddWithValue("@czy_dla_dzieci", product.IsForKids);
                        command.Parameters.AddWithValue("@plec", product.Sex);
                        command.Parameters.AddWithValue("@dostepny", product.isAvailable);
                        if (imageData != null)
                        {
                            command.Parameters.AddWithValue("@zdjecie", imageData);
                        }

                        int rowsAffected = command.ExecuteNonQuery();

                        connection.Close();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }

        }

        public bool updateProduct(Product product, IFormFile? image, out string errorMessage)
        {
            errorMessage = null;
            byte[] imageData = null;

            if (image != null)
            {
                imageData = new byte[image.Length];
                image.OpenReadStream().Read(imageData, 0, imageData.Length);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "edytujProdukt";

                        command.Parameters.AddWithValue("@id_produkt", product.Id_product);
                        command.Parameters.AddWithValue("@marka", product.Brand);
                        command.Parameters.AddWithValue("@model", product.Model);
                        command.Parameters.AddWithValue("@kolor", product.Color);
                        command.Parameters.AddWithValue("@opis", product.Description);
                        command.Parameters.AddWithValue("@kod_produkt", product.ProductCode);
                        command.Parameters.AddWithValue("@cena", product.Price);
                        command.Parameters.AddWithValue("@czy_dla_dzieci", product.IsForKids);
                        command.Parameters.AddWithValue("@plec", product.Sex);
                        command.Parameters.AddWithValue("@dostepny", product.isAvailable);
                        if (imageData != null)
                        {
                            command.Parameters.AddWithValue("@zdjecie", imageData);
                        }

                        int rowsAffected = command.ExecuteNonQuery();

                        connection.Close();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }

        }
    }
}
