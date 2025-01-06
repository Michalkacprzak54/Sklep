using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SklepMVC.DAL;
using SklepMVC.Models;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace SklepMVC.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;
        private readonly GetData _getData;

        public CheckoutController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, GetData getData)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ConnectionString")["Polaczenie"];
            _getData = getData;
        }
        // GET: CheckoutController
        public ActionResult Index()
        {
            Checkout checkout = new Checkout();
            checkout.Bussines = _getData.GetBussines(3);
            return View(checkout);
        }

        [HttpPost]
        public ActionResult Index([Bind] Checkout checkout)
        {
            //checkout.Bussines = null;
            if (ModelState.IsValid)
            { 
                if(Request.Cookies["CookieUserID"] != null)
                {
                    int IdOrder;
                    if (checkout.bussinesCheckbox == true)
                    {
                        int? IdBussines = AddBussines(checkout.Bussines);
                        if(IdBussines == -1)
                        {
                            IdBussines = null;
                        }
                        IdOrder = AddOrder(checkout.Order, Convert.ToInt32(Request.Cookies["CookieUserKlientID"]), IdBussines);
                    }
                    else
                    {
                        IdOrder = AddOrder(checkout.Order, Convert.ToInt32(Request.Cookies["CookieUserKlientID"]), null);
                    }
                    Debug.WriteLine(IdOrder);
                    if (IdOrder == -1)
                    {
                        return View();
                    }
                    List<Cart> cart = _getData.GetCart();

                    foreach (Cart cartItem in cart)
                    {
                        AddOrderProduct(cartItem.Size, IdOrder);
                    }
                    DeleteCart();

                }
                else
                {
                    int IdClient = -1;

                    IdClient = _getData.GetCustomerId(checkout.Customer.Email);
                    if ( IdClient == 0)
                    {
                        IdClient = AddClient(checkout.Customer);
                    }

                    if (IdClient == -1)
                    {
                        return View();
                    }
                    int IdOrder;
                    if (checkout.bussinesCheckbox == true)
                    {
                        int? IdBussines = AddBussines(checkout.Bussines);
                        if (IdBussines == -1)
                        {
                            IdBussines = null;
                        }
                        IdOrder = AddOrder(checkout.Order, IdClient, IdBussines);
                    }
                    else
                    {
                        IdOrder = AddOrder(checkout.Order, IdClient, null);
                    }
                    
                    if (IdOrder == -1)
                    {
                        return View();
                    }

                    List<Cart> cart = _getData.GetCart();

                    foreach (Cart cartItem in cart)
                    {
                        AddOrderProduct(cartItem.Size, IdOrder);
                    }
                    DeleteCart();
                }
                return RedirectToAction("ConfirmOrder");
            }

            return View();
        }

        public ActionResult ConfirmOrder()
        {
            return View();
        }

        private int AddOrder(Order o, int IdCustomer, int? IdBussines)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("dodajZamowienieIstniejeKlient", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_klient", IdCustomer);
                cmd.Parameters.AddWithValue("@miasto", o.City);
                cmd.Parameters.AddWithValue("@kod_pocztowy", o.ZipCode);
                cmd.Parameters.AddWithValue("@ulica", o.Street);
                cmd.Parameters.AddWithValue("@nr_domu", o.HouseNumber);
                cmd.Parameters.AddWithValue("@nr_mieszkania", o.ApartmentNumber);
                cmd.Parameters.AddWithValue("@id_firma", IdBussines);

                SqlParameter outputParameter = new SqlParameter("@id_zamowienie", SqlDbType.Int);
                outputParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParameter);

                conn.Open();
                cmd.ExecuteNonQuery();

                int orderId = Convert.ToInt32(outputParameter.Value);

                conn.Close();
                return (orderId);
            }
            catch (Exception ex)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                Debug.WriteLine(ex.Message);
                return -1;
            }

        }

        private int AddBussines(Bussines b)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("dodajFirme", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nazwa", b.Name);
                cmd.Parameters.AddWithValue("@miasto", b.City);
                cmd.Parameters.AddWithValue("@kod_pocztowy", b.ZipCode);
                cmd.Parameters.AddWithValue("@ulica", b.Street);
                cmd.Parameters.AddWithValue("@nr_domu", b.HouseNumber);
                cmd.Parameters.AddWithValue("@nr_mieszkania", b.ApartmentNumber);
                cmd.Parameters.AddWithValue("@nip", b.Nip);
                cmd.Parameters.AddWithValue("@regon", b.Regon);

                SqlParameter outputParameter = new SqlParameter("@id_firma", SqlDbType.Int);
                outputParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParameter);

                conn.Open();
                cmd.ExecuteNonQuery();

                int bussinesId = Convert.ToInt32(outputParameter.Value);

                conn.Close();
                return (bussinesId);
            }
            catch (Exception ex)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                Debug.WriteLine(ex.Message);
                return -1;
            }

        }

        private int AddClient(CustomerOrder c)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("dodajKlienta", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@imie", c.FirstName);
                cmd.Parameters.AddWithValue("@nazwisko", c.LastName);
                cmd.Parameters.AddWithValue("@nr_telefonu", c.PhoneNumber);
                cmd.Parameters.AddWithValue("@email", c.Email);
                cmd.Parameters.AddWithValue("@czy_kobieta", c.IsWoman);         

                SqlParameter outputParameter = new SqlParameter("@id_klient", SqlDbType.Int);
                outputParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParameter);

                conn.Open();
                cmd.ExecuteNonQuery();

                int klientId = Convert.ToInt32(outputParameter.Value);

                conn.Close();
                return (klientId);
            }
            catch (Exception ex)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                Debug.WriteLine(ex.Message);
                return -1;
            }

        }

        private string AddOrderProduct(int IdProduct, int IdOrder)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("dodajZamowionyProdukt", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_zamowienie", IdOrder);
                cmd.Parameters.AddWithValue("@id_magazyn", IdProduct);
                
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                return ("Succes");
            }
            catch (Exception ex)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                Debug.WriteLine(ex.Message);
                return (ex.Message.ToString());
            }

        }

        private void DeleteCart()
        {
            List<Cart> cart = new List<Cart>();
            SaveShoppingCart(cart);
        }

        private void SaveShoppingCart(List<Cart> cart)
        {
            string cartJson = JsonConvert.SerializeObject(cart);
            _httpContextAccessor.HttpContext.Session.SetString("Cart", cartJson);
        }

    }
}
