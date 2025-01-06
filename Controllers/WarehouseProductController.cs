using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using SklepMVC.DAL;
using SklepMVC.Models;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace SklepMVC.Controllers
{
    public class WarehouseProductController : Controller
    {


        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        private readonly string? _connectionString;
        private readonly GetData _getData;

        public WarehouseProductController(IConfiguration configuration, GetData getData)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ConnectionString")["Polaczenie"];
            _getData = getData;
        }
        // GET: WarehouseController
        public ActionResult Index(int id)
        {

            if (Request.Cookies["CookieWorkerLoginID"] != null)
            {
                
                List<Warehouse> warehouse = _getData.GetWarehouse(id);
                return View(warehouse);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: WarehouseController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create(int id)
        {

            if (Request.Cookies["CookieWorkerLoginID"] != null)
            {
                List<Sizes> sizes = _getData.GetSizes();
                List<Product> products = _getData.GetProductById(id);

                WarehouseProduct objWarehouseProduct = new WarehouseProduct
                {
                    SizesViewModel = sizes,
                    ProductViewModel = products,
                    WarehouseViewModel = new List<Warehouse> { new Warehouse() }
                };
                return View(objWarehouseProduct);
            }

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
            

        // POST: WarehouseController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Warehouse warehouse, int id)
        {
            if (Request.Cookies["CookieWorkerLoginID"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (addWarehouse(warehouse, id))
                        {
                            TempData["SuccessMessage"] = "Operacja zakończona sukcesem.";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Operacja nie powiodła się.";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Formularz zawiera błędy.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Wystąpił błąd: {ex.Message}";
                }

                return RedirectToAction("Index", new { id = id });
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
                List<Warehouse> warehouse = _getData.GetSingleWarehouse(id);
                return View(warehouse);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            
        }

        // POST: WarehouseController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Warehouse warehouse,int id)
        {

            if (Request.Cookies["CookieWorkerLoginID"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Debug.WriteLine(warehouse.Quantity);
                        if (updateWarehouse(warehouse, id))
                        {
                            TempData["SuccessMessage"] = "Operacja zakończona sukcesem.";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Operacja nie powiodła się.";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Formularz zawiera błędy.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Wystąpił błąd: {ex.Message}";
                }

                return RedirectToAction("Index", new { id = warehouse.Id_product });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            
        }
    
        public bool addWarehouse(Warehouse warehouse, int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "dodajStan";

                        command.Parameters.AddWithValue("@id_produkt", id); 
                        command.Parameters.AddWithValue("@rozmiar", warehouse.Size);
                        command.Parameters.AddWithValue("@ilosc", warehouse.Quantity);
;

                        int rowsAffected = command.ExecuteNonQuery();
                        
                        connection.Close();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
        public bool updateWarehouse(Warehouse warehouse, int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "edytujStan";

                        command.Parameters.AddWithValue("@id_magazyn", id);
                        command.Parameters.AddWithValue("@ilosc", warehouse.Quantity);
                        

                        int rowsAffected = command.ExecuteNonQuery();

                        connection.Close();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
