using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SklepMVC.DAL;
using SklepMVC.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace SklepMVC.Controllers
{
    public class CustomerOrderController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        private readonly string? _connectionString;
        private readonly GetData _getData;

        public CustomerOrderController(IConfiguration configuration, GetData getData)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ConnectionString")["Polaczenie"];
            _getData = getData;
        }
        // GET: ClientOrderController
        public ActionResult Index()
        {
            List<CustomerOrderData> customerOrders;

            if (Request.Cookies["CookieUserID"] != null)
            {
                customerOrders = _getData.GetCustomerOrders(Convert.ToInt32(Request.Cookies["CookieUserKlientID"]));
                return View(customerOrders);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Details(int id)
        {
            CustomerOrderDataDetails customerOrderDetails = new CustomerOrderDataDetails();

            if (Request.Cookies["CookieUserID"] != null)
            {
                customerOrderDetails.OrderData = _getData.GetCustomerOrder(Convert.ToInt32(Request.Cookies["CookieUserKlientID"]), id);
                customerOrderDetails.Order = _getData.GetSingleOrder(id);
                customerOrderDetails.Customer = _getData.GetCustomer(Convert.ToInt32(Request.Cookies["CookieUserKlientID"]));
                customerOrderDetails.Products = _getData.GetOrderProduct(id); ;
                return View(customerOrderDetails);
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
                Debug.WriteLine($"{id} {newStatus}");
                if (UpdateStatus(id, newStatus))
                {
                    UpdateQuantity(id);
                    return Json(new { success = true, message = "Status zamówienia został zaktualizowany." });
                }
                else
                {
                    return Json(new { success = false, message = "Nie udało się zaktualizować statusu zamówienia." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Wystąpił błąd podczas aktualizacji statusu zamówienia." });
            }
        }

        public bool UpdateStatus(int id, string newStatus)
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

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }
        public bool UpdateQuantity(int id)
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
