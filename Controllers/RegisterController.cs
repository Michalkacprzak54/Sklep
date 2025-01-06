using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SklepMVC.Models;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace SklepMVC.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        public RegisterController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ConnectionString")["Polaczenie"];
        }
        // GET: RegisterController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([Bind] Customer c)
        {
            if (ModelState.IsValid)
            {
                if (AddCustomer(c) == "Succes")
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("Index");
        }

        private string AddCustomer(Customer c)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            try
            {
                MD5 md5 = MD5.Create();
                var byte_data = md5.ComputeHash(Encoding.UTF8.GetBytes(c.Password));
                string password = Convert.ToHexString(byte_data).ToLower();

                SqlCommand cmd = new SqlCommand("dodawanie_uzytkownika", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@imie", c.FirstName);
                cmd.Parameters.AddWithValue("@nazwisko", c.LastName);
                cmd.Parameters.AddWithValue("@nr_telefonu", c.PhoneNumber);
                cmd.Parameters.AddWithValue("@czy_kobieta", c.IsWoman);
                //cmd.Parameters.AddWithValue("@miasto", c.City);
                //cmd.Parameters.AddWithValue("@kod_pocztowy", c.ZipCode);
                //cmd.Parameters.AddWithValue("@ulica", c.Street);
                //cmd.Parameters.AddWithValue("@nr_domu", c.HouseNumber);
                //cmd.Parameters.AddWithValue("@nr_mieszkania", c.ApartmentNumber);
                cmd.Parameters.AddWithValue("@email", c.Email);
                cmd.Parameters.AddWithValue("@haslo", password);
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

    }
}
