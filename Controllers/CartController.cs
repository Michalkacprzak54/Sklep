using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using SklepMVC.DAL;
using SklepMVC.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace SklepMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;
        private readonly GetData _getData;

        public CartController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, GetData getData)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ConnectionString")["Polaczenie"];
            _getData = getData;
        }

        // GET: CartController
        public ActionResult Index()
        {
            List<Cart> cart = GetCart();
            List<CartProduct> products = new List<CartProduct>();

            foreach (Cart cartProduct in cart)
            {
                products.Add(GetProduct(cartProduct.Size));
            }

            ViewBag.Sum = (decimal)products.Select(p => p.Price).Sum();
            ViewBag.ProductsCount = products.Count();

            foreach (var cartItem in cart)
            {
                Debug.WriteLine($"{cartItem.Id} {cartItem.Size}");
            }

            return View(products);
        }
        
        public ActionResult AddToCart([Bind] Cart c)
        {
            if (ModelState.IsValid)
            {
                List<Cart> cart = GetCart();
                cart.Add(new Cart { Id = c.Id, Size = c.Size });
                SaveShoppingCart(cart);
                //Debug.WriteLine($"{c.Id} {c.Size}");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Details", "Product", new { id = c.Id });
        }

        public ActionResult DeleteFromCart(int id_warehouse)
        {
            List<Cart> cart = GetCart();
            Cart itemToremove = cart.FirstOrDefault(item => item.Size == id_warehouse);
            cart.Remove(itemToremove);
            SaveShoppingCart(cart);
            return RedirectToAction("Index"); ;
        }

        private void SaveShoppingCart(List<Cart> cart)
        {
            string cartJson = JsonConvert.SerializeObject(cart);
            _httpContextAccessor.HttpContext.Session.SetString("Cart", cartJson);
        }

        private List<Cart> GetCart()
        {
            string cartJson = _httpContextAccessor.HttpContext.Session.GetString("Cart");

            if (cartJson != null)
            {
                return JsonConvert.DeserializeObject<List<Cart>>(cartJson);
            }

            return new List<Cart>();
        }

        public CartProduct GetProduct(int id_warehouse)
        {
            CartProduct product = new CartProduct();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM widokProduktFull WHERE id_magazyn=@id_magazyn";
                    command.Parameters.AddWithValue("@id_magazyn", id_warehouse);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CartProduct newProduct = new CartProduct()
                            {
                                Id_product = Convert.ToInt32(reader["id_produkt"]),
                                Brand = reader["marka"].ToString(),
                                Model = reader["model"].ToString(),
                                Color = reader["kolor"].ToString(),
                                Price = (decimal)reader["cena"],
                                Id_warehouse = Convert.ToInt32(reader["id_magazyn"]),
                                Size = reader["rozmiar"].ToString(),
                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("zdjecie")))
                            {
                                newProduct.Photo = (byte[])reader["zdjecie"];
                            }
                            product = newProduct;
                        }
                    }
                }
                connection.Close();
            }
            return product;
        }

    }
}
