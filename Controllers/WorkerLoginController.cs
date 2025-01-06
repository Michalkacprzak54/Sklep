using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SklepMVC.DAL;
using SklepMVC.Models;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace SklepMVC.Controllers
{
    public class WorkerLoginController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        private readonly string? _connectionString;

        public WorkerLoginController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ConnectionString")["Polaczenie"];
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (TempData["worker"] != null)
            {
                string message = (string)TempData["worker"];
                ViewData["Message"] = message;
            }
            if (Request.Cookies["CookieWorkerLoginID"] != null)
            {
                return RedirectToAction("Index", "WorkerProduct");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Index(Worker worker)
        {

            if (ModelState.IsValid)
            {
                MD5 md5 = MD5.Create();
                var byte_data = md5.ComputeHash(Encoding.UTF8.GetBytes(HttpContext.Request.Form["password"]));
                string password = Convert.ToHexString(byte_data);

                worker = Login(HttpContext.Request.Form["email"], password.ToLower());

                if (worker != null)
                {
                    CookieOptions options = new CookieOptions();
                    options.Expires = DateTime.Now.AddMinutes(60);
                    Response.Cookies.Append("CookieWorkerLoginID", worker.id_worker_login.ToString(), options);
                    Response.Cookies.Append("CookieWorkerID", worker.id_worker.ToString(), options);
                    Response.Cookies.Append("CookieWorkerEmail", worker.email, options);
                    return RedirectToAction("Index", "WorkerProduct");
                }
                else
                {
                    string data = "Logowanie nie powiodło się!";
                    TempData["worker"] = data;

                    return RedirectToAction("Index", "WorkerLogin");
                }
            }
            return View();
        
        }


        public ActionResult Logout()
        {
            Response.Cookies.Delete("CookieWorkerLoginID");
            Response.Cookies.Delete("CookieWorkerID");
            Response.Cookies.Delete("CookieWorkerEmail");
            return RedirectToAction("Index");
        }

        private Worker Login(string email, string password)
        {
            Worker worker;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM LoginPracownik WHERE email = @email  AND haslo= @password";
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@password", password);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            worker = new Worker()
                            {
                                id_worker_login = Convert.ToInt32(reader["id_pracownik_login"]),
                                id_worker = Convert.ToInt32(reader["id_pracownik"]),
                                email = reader["email"].ToString(),
                                password = reader["haslo"].ToString()
                            };
                        }
                        else
                        {
                            worker = null;
                        }

                    }
                }
                connection.Close();
            }
            return worker;
        }
    }
}
