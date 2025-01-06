using Microsoft.AspNetCore.Mvc;
using SklepMVC.Models;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using SklepMVC.DAL;
using SklepMVC.Models;

namespace SklepMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        private readonly GetData _getData;
        private readonly string? _connectionString;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger, GetData getData)
        {
            _configuration = configuration;
            _logger = logger;
            _getData = getData;
            _connectionString = _configuration.GetSection("ConnectionString")["Polaczenie"];
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<Product> products = _getData.GetProducts();

            return View(products);
        }

        //Wyświetla regulamin 
        [HttpGet]
        public ActionResult Statute()
        {

            return View();
        }


    }
}