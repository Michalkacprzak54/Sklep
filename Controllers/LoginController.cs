using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SklepMVC.Models;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace SklepMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;
        //private User user = new User();

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ConnectionString")["Polaczenie"];
        }


        [HttpGet]
        public ActionResult Index()
        {
            if (TempData["user"] != null)
            {
                string message = (string)TempData["user"];
                ViewData["Message"] = message;
            }
            if (Request.Cookies["CookieUserID"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
            // GET: LoginController
        [HttpPost]
        public ActionResult Index(User u)
        {
            if (ModelState.IsValid)
            {
                MD5 md5 = MD5.Create();
                var byte_data = md5.ComputeHash(Encoding.UTF8.GetBytes(HttpContext.Request.Form["password"]));
                string password = Convert.ToHexString(byte_data);

                u = LoginService(HttpContext.Request.Form["email"], password.ToLower());

                if (u != null)
                {
                    CookieOptions options = new CookieOptions();
                    options.Expires = DateTime.Now.AddMinutes(60);
                    Response.Cookies.Append("CookieUserID", u.id_klient_login.ToString(), options);
                    Response.Cookies.Append("CookieUserKlientID", u.id_klient.ToString(), options);
                    Response.Cookies.Append("CookieUserEmail", u.email, options);
                }
                else
                {
                    string data = "Logowanie nie powiodło się!";
                    TempData["user"] = data;

                }
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Logout()
        {
            Response.Cookies.Delete("CookieUserID");
            Response.Cookies.Delete("CookieUserKlientID");
            Response.Cookies.Delete("CookieUserEmail");
            return RedirectToAction("Index");
        }

        private User LoginService(string email, string password)
        {
            User user;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Klient_login WHERE email='" + email + "' and haslo='" + password + "';";
                    command.CommandText = "WITH Klient_dane AS (SELECT * FROM Klient WHERE email = '" + email + "') SELECT KL.id_klient_login, K.id_klient, K.email, KL.haslo FROM Klient_dane AS K INNER JOIN Klient_login AS KL ON K.id_klient = KL.id_klient WHERE KL.haslo = '" + password + "'";
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User()
                            {
                                id_klient_login = Convert.ToInt32(reader["id_klient_login"]),
                                id_klient = Convert.ToInt32(reader["id_klient"]),
                                email = reader["email"].ToString(),
                                password = reader["haslo"].ToString()
                            };
                        }
                        else
                        {
                            user = null;
                        }
                        
                    }
                }
                connection.Close();
            }
            return user;
        }



    }
}
