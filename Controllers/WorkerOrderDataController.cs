using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SklepMVC.DAL;
using SklepMVC.Models;
using System.Data.SqlClient;
using System.Diagnostics;

namespace SklepMVC.Controllers
{
    public class WorkerOrderDataController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        private readonly string? _connectionString;
        private readonly GetData _getData;

        public WorkerOrderDataController(IConfiguration configuration, GetData getData)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ConnectionString")["Polaczenie"];
            _getData = getData;
        }

        List<WorkerOrderData> orderData = new List<WorkerOrderData>();

        [HttpGet]
        public ActionResult Index(string? statusZamowienia)
        {
            if (Request.Cookies["CookieWorkerLoginID"] != null)
            {
                orderData = _getData.GetOrderData();
                if(!string.IsNullOrEmpty(statusZamowienia))
                {
                    orderData = orderData.Where(v => v.Status.ToLower() == statusZamowienia.ToLower()).ToList();
                }
                return View(orderData);
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
                WorkerOrderData orderData = _getData.GetSingleOrder(id);
                List<WorkerOrderedProducts> orderProductData = _getData.GetOrderProduct(id);

                WorkerFullOrder newWorkerFullOrder = new WorkerFullOrder()
                {
                    WorkerOrderData = orderData,
                    WorkerOrderedProducts = orderProductData
                };

                return View(newWorkerFullOrder);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        [HttpPost]
        public ActionResult ChangeOrderStatus(int id, string newStatus)
        {
            try
            {
                if (updateStatus(id, newStatus))
                {
                    if(newStatus != "Wysłane")
                    {
                        updateQuantity(id);
                    }
                    return Json(new { success = true, message = "Status zamówienia został zaktualizowany." });
                }
                else
                {
                    return Json(new { success = false, message = "Nie udało się zaktualizować statusu zamówienia." });
                }
            }
            catch (Exception ex)
            {
                // Zaloguj błąd
                return Json(new { success = false, message = "Wystąpił błąd podczas aktualizacji statusu zamówienia." });
            }
        }

        public bool updateStatus(int id, string newStatus)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();  
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "UPDATE Zamowienie SET Status = @nowyStatus WHERE id_zamowienie = @OrderId";
                    command.Parameters.AddWithValue("@nowyStatus", newStatus);
                    command.Parameters.AddWithValue("@OrderId", id);

                    int rowsAffected = 0;
                    rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0;
                }
                
            }
        }
        public bool updateQuantity(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "anulujZamowienie";

                    command.Parameters.AddWithValue("@id_zamowienie", id);

                    int rowsAffected = 0;
                    rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0;
                }

            }
        }


    }
}
