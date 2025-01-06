using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SklepMVC.Controllers;
using SklepMVC.Models;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SklepMVC.DAL
{
    public class GetData
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        private readonly string? _connectionString;
        private readonly string? _connectionStringMysql;


        public GetData(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ConnectionString")["Polaczenie"];
            _connectionStringMysql = _configuration.GetSection("ConnectionString")["Mysql"];
        }
        public List<Product> GetProducts() 
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM widokProdukt";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product newProduct = new Product()
                            {
                                Id_product = Convert.ToInt32(reader["id"]),
                                Brand = reader["marka"].ToString(),
                                Model = reader["model"].ToString(),
                                Color = reader["kolor"].ToString(),
                                Description = reader["opis"].ToString(),
                                ProductCode = reader["kod_produkt"].ToString(),
                                //Price = Convert.ToDecimal(reader["cena"]),
                                Price = reader["cena"].ToString(),
                                Sex = reader["plec"].ToString(),
                                IsForKids = reader["dla_dzieci"].ToString(),
                                isAvailable = reader["dostepny"].ToString(),
                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("zdjecie")))
                            {
                                newProduct.Photo = (byte[])reader["zdjecie"];
                            }
                            products.Add(newProduct);
                        }
                    }

                }
                connection.Close();
            }
            return products;
        }

        public List<Product> GetProductById(int idProduct)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM widokProdukt WHERE id = @productId";
                    command.Parameters.AddWithValue("@productId", idProduct);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product newProduct = new Product()
                            {
                                Id_product = Convert.ToInt32(reader["id"]),
                                Brand = reader["marka"].ToString(),
                                Model = reader["model"].ToString(),
                                Color = reader["kolor"].ToString(),
                                Description = reader["opis"].ToString(),
                                ProductCode = reader["kod_produkt"].ToString(),
                                //Price = Convert.ToDecimal(reader["cena"]),
                                Price = reader["cena"].ToString(),
                                Sex = reader["plec"].ToString(),
                                IsForKids = reader["dla_dzieci"].ToString(),
                                isAvailable = reader["dostepny"].ToString(),
                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("zdjecie")))
                            {
                                newProduct.Photo = (byte[])reader["zdjecie"];
                            }
                            products.Add(newProduct);
                        }
                    }

                }
                connection.Close();
            }
            return products;
        }

        public List<Warehouse> GetWarehouse(int idProuct)
        {
            List<Warehouse> warehouse = new List<Warehouse>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT * FROM widokMagazyn WHERE Id_produkt = @productId";
                    command.Parameters.AddWithValue("@productId", idProuct);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Warehouse newWarehouse = new Warehouse()
                            {
                                Id_warehouse = Convert.ToInt32(reader["id_magazyn"]),
                                Size = reader["rozmiar"].ToString(),
                                Id_product = Convert.ToInt32(reader["Id_produkt"]),
                                Shoe = reader["Buty"].ToString(),
                                //Color = reader["Kolor"].ToString(),
                                Quantity = Convert.ToInt32(reader["ilosc"]),
                            };
                            warehouse.Add(newWarehouse);
                        }
                    }
                }
                connection.Close();
            }
            return warehouse;
        }

        public List<Warehouse> GetWarehouseData()
        {
            List<Warehouse> warehouse = new List<Warehouse>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT * FROM widokMagazyn";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Warehouse newWarehouse = new Warehouse()
                            {
                                Id_warehouse = Convert.ToInt32(reader["id_magazyn"]),
                                Size = reader["rozmiar"].ToString(),
                                Id_product = Convert.ToInt32(reader["Id_produkt"]),
                                Shoe = reader["Buty"].ToString(),
                                //Color = reader["Kolor"].ToString(),
                                Quantity = Convert.ToInt32(reader["ilosc"]),
                            };
                            warehouse.Add(newWarehouse);
                        }
                    }
                }
                connection.Close();
            }
            return warehouse;
        }

        public List<Warehouse> GetSingleWarehouse(int idWarehouse)
        {
            List<Warehouse> warehouse = new List<Warehouse>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT * FROM widokMagazyn WHERE id_magazyn = @productId";
                    command.Parameters.AddWithValue("@productId", idWarehouse);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Warehouse newWarehouse = new Warehouse()
                            {
                                Id_warehouse = Convert.ToInt32(reader["id_magazyn"]),
                                Size = reader["rozmiar"].ToString(),
                                Id_product = Convert.ToInt32(reader["Id_produkt"]),
                                Shoe = reader["Buty"].ToString(),
                                //Color = reader["Kolor"].ToString(),
                                Quantity = Convert.ToInt32(reader["ilosc"]),
                            };
                            warehouse.Add(newWarehouse);
                        }
                    }
                }
                connection.Close();
            }
            return warehouse;
        }
        public List<string> GetSex()
        {
            List<string> sex = new List<string>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT * FROM Plec";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sex.Add(reader["nazwa"].ToString());
                        }
                    }
                }
                connection.Close();
            }
            return sex;
        }

        public List<Sizes> GetSizes()
        {
            List<Sizes> size = new List<Sizes>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT * FROM Rozmiar";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Sizes newSize = new Sizes()
                            {
                                id_size = Convert.ToInt32(reader["id_rozmiar"]),
                                size = reader["rozmiar"].ToString()
                            };
                            size.Add(newSize);
                        }
                    }
                }
                connection.Close();
            }
            return size;
        }

        public string GetUserName(int idKlient)
        {
            string userName = "";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT imie FROM Klient where id_klient=@id_klient";
                    command.Parameters.AddWithValue("@id_klient", idKlient);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userName = reader["imie"].ToString();
                        }
                    }
                }
                connection.Close();
            }
            return userName;
        }

        public List<Comment> GetComments(int productID)
        {
            List<Comment> comments = new List<Comment>();

            using (MySqlConnection connection = new MySqlConnection(_connectionStringMysql))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandTimeout = 60;
                    command.CommandText = "SELECT * FROM comments WHERE id_produkt=@id_produkt";
                    command.Parameters.Add("@id_produkt", MySqlDbType.Int32).Value = productID;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Comment newComment = new Comment()
                            {
                                Id_comment = Convert.ToInt32(reader["id_komentarz"]),
                                Id_client_login = Convert.ToInt32(reader["id_klient_login"]),
                                Id_client = Convert.ToInt32(reader["id_klient"]),
                                Id_product = Convert.ToInt32(reader["id_produkt"]),
                                Title = reader["tytul"].ToString(),
                                Description = reader["opis"].ToString(),
                                Added_date = Convert.ToDateTime(reader["data_dodania"]),
                                Rate = Convert.ToInt32(reader["ocena"])
                            };
                            comments.Add(newComment);
                        }
                    }
                    connection.Close();
                }

            }
            return comments;
        }
        public Comment GetComment(int idComment)
        {
            Comment comment = new Comment();

            using (MySqlConnection connection = new MySqlConnection(_connectionStringMysql))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandTimeout = 60;
                    command.CommandText = "SELECT * FROM comments WHERE id_komentarz=@id_komentarz";
                    command.Parameters.Add("@id_komentarz", MySqlDbType.Int32).Value = idComment;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Comment newComment = new Comment()
                            {
                                Id_comment = Convert.ToInt32(reader["id_komentarz"]),
                                Id_client_login = Convert.ToInt32(reader["id_klient_login"]),
                                Id_client = Convert.ToInt32(reader["id_klient"]),
                                Id_product = Convert.ToInt32(reader["id_produkt"]),
                                Title = reader["tytul"].ToString(),
                                Description = reader["opis"].ToString(),
                                Added_date = Convert.ToDateTime(reader["data_dodania"]),
                                Rate = Convert.ToInt32(reader["ocena"])
                            };
                            comment = newComment;
                        }
                    }
                    connection.Close();
                }

            }
            return comment;
        }
        public List<string> GetBrand()
        {
            List<string> brands = new List<string>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT * FROM marka";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            brands.Add(reader["nazwa"].ToString());

                        }
                    }
                    connection.Close();
                }
                return brands;
            }
        }

        public Bussines GetBussines(int id)
        {
            Bussines bussines = new Bussines();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT * FROM Firma";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bussines.Id = Convert.ToInt32(reader["id_firma"]);
                            bussines.Name = (reader["nazwa"].ToString());
                            bussines.City = "brak";
                            bussines.ZipCode = "brak";
                            bussines.Street = (reader["ulica"].ToString());
                            bussines.HouseNumber = (reader["nr_domu"].ToString());
                            bussines.ApartmentNumber = Convert.ToInt32(reader["nr_mieszkania"]);
                            bussines.Nip = Convert.ToDecimal(reader["nip"]);
                            bussines.Regon = Convert.ToDecimal(reader["regon"]);

                        }
                    }
                    connection.Close();
                }
                return bussines;
            }
        }

        public List<Cart> GetCart()
        {
            string cartJson = _httpContextAccessor.HttpContext.Session.GetString("Cart");

            if (cartJson != null)
            {
                return JsonConvert.DeserializeObject<List<Cart>>(cartJson);
            }

            return new List<Cart>();
        }

        public int GetCustomerId(string email)
        {
            int idCustomer = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT id_klient FROM Klient WHERE email = @email";
                    command.Parameters.AddWithValue("@email", email);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idCustomer = Convert.ToInt32(reader["id_klient"]);
                        }
                    }
                }
                connection.Close();
            }
            return idCustomer;
        }

        public List<WorkerOrderData> GetOrderData()
        {
            List<WorkerOrderData> orderData = new List<WorkerOrderData>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT * FROM WidokZamowienie";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            WorkerOrderData newOrderData = new WorkerOrderData()
                            {
                                idOrder = Convert.ToInt32(reader["id_zamowienie"]),
                                Client = reader["Klient"].ToString(),
                                City = reader["Miasto"].ToString(),
                                Zip_code = reader["Kod"].ToString(),
                                Street = reader["ulica"].ToString(),
                                HouseNumber = reader["nr_domu"].ToString(),
                                ApartmentNumber = reader["nr_mieszkania"] != DBNull.Value ? Convert.ToInt32(reader["nr_mieszkania"]) : (int?)null,
                                CompanyName = reader["Firma"].ToString(),
                                Status = reader["Status"].ToString()

                            };
                            orderData.Add(newOrderData);
                        }
                    }
                }
                connection.Close();
            }
            return orderData;
        }

        public WorkerOrderData GetSingleOrder(int id)
        {
            WorkerOrderData orderData = new WorkerOrderData();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT * FROM WidokZamowienie WHERE Id_zamowienie = @OrderId";
                    command.Parameters.AddWithValue("@OrderId", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            WorkerOrderData newOrderData = new WorkerOrderData()
                            {
                                idOrder = Convert.ToInt32(reader["id_zamowienie"]),
                                Client = reader["Klient"].ToString(),
                                City = reader["Miasto"].ToString(),
                                Zip_code = reader["Kod"].ToString(),
                                Street = reader["ulica"].ToString(),
                                HouseNumber = reader["nr_domu"].ToString(),
                                ApartmentNumber = reader["nr_mieszkania"] != DBNull.Value ? Convert.ToInt32(reader["nr_mieszkania"]) : (int?)null,
                                CompanyName = reader["Firma"].ToString(),
                                Status = reader["Status"].ToString()

                            };
                            orderData = newOrderData;
                            
                        }
                    }
                }
                connection.Close();
            }
            return orderData;
        }

        public List<WorkerOrderedProducts> GetOrderProduct(int id)
        {
            List<WorkerOrderedProducts> orderedProducts = new List<WorkerOrderedProducts>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT * FROM widokZamowienieProdukt WHERE Id_zamowienie = @OrderId";
                    command.Parameters.AddWithValue("@OrderId", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            WorkerOrderedProducts newOrderedProduct = new WorkerOrderedProducts()
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                WarehouseId = Convert.ToInt32(reader["id_magazyn"]),
                                OrderId = Convert.ToInt32(reader["id_zamowienie"]),
                                CustomerId = Convert.ToInt32(reader["id_klient"]),
                                Shoes = reader["Buty"].ToString(),
                                Size = reader["rozmiar"].ToString(),
                                Color = reader["Kolor"].ToString(),
                                Price = Convert.ToDecimal(reader["Cena"]),
                            };
                            if (!reader.IsDBNull(reader.GetOrdinal("zdjecie")))
                            {
                                newOrderedProduct.Photo = (byte[])reader["zdjecie"];
                            }
                            orderedProducts.Add(newOrderedProduct);
                        }
                    }
                }
                connection.Close();
            }
            return orderedProducts;
        }

        public List<CustomerOrderData> GetCustomerOrders(int idCustomer)
        {
            List<CustomerOrderData> orders = new List<CustomerOrderData>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT Z.id_zamowienie, Z.id_klient, Z.status, SUM(P.cena) AS wartosc, Z.data_zamowienie " +
                        "FROM Zamowienie AS Z " +
                        "INNER JOIN Zamowienie_produkt AS ZP ON Z.id_zamowienie = Zp.id_zamowienie " +
                        "INNER JOIN Magazyn AS M ON ZP.id_magazyn = M.id_magazyn " +
                        "INNER JOIN Produkt AS P ON M.id_produkt = P.id_produkt " +
                        "WHERE Z.id_klient = @id_klient " +
                        "GROUP BY Z.id_zamowienie, Z.id_klient, Z.status, Z.data_zamowienie ";
                    command.Parameters.AddWithValue("@id_klient", idCustomer);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerOrderData newOrder = new CustomerOrderData()
                            {
                                OrderId = Convert.ToInt32(reader["id_zamowienie"]),
                                CustomerId = Convert.ToInt32(reader["id_klient"]),
                                Status = reader["status"].ToString(),
                                OrderValue = Convert.ToDecimal(reader["wartosc"]),
                                OrderDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["data_zamowienie"])),
                            };
                            orders.Add(newOrder);
                        }
                    }
                }
                connection.Close();
            }
            return orders;
        }

        public CustomerOrderData GetCustomerOrder(int idCustomer, int idOrder)
        {
            CustomerOrderData order = new CustomerOrderData();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT Z.id_zamowienie, Z.id_klient, Z.status, SUM(P.cena) AS wartosc, Z.data_zamowienie " +
                        "FROM Zamowienie AS Z " +
                        "INNER JOIN Zamowienie_produkt AS ZP ON Z.id_zamowienie = Zp.id_zamowienie " +
                        "INNER JOIN Magazyn AS M ON ZP.id_magazyn = M.id_magazyn " +
                        "INNER JOIN Produkt AS P ON M.id_produkt = P.id_produkt " +
                        "WHERE Z.id_klient = @id_klient AND Z.id_zamowienie = @id_zamowienie " +
                        "GROUP BY Z.id_zamowienie, Z.id_klient, Z.status, Z.data_zamowienie ";
                    command.Parameters.AddWithValue("@id_klient", idCustomer);
                    command.Parameters.AddWithValue("@id_zamowienie", idOrder);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerOrderData newOrder = new CustomerOrderData()
                            {
                                OrderId = Convert.ToInt32(reader["id_zamowienie"]),
                                CustomerId = Convert.ToInt32(reader["id_klient"]),
                                Status = reader["status"].ToString(),
                                OrderValue = Convert.ToDecimal(reader["wartosc"]),
                                OrderDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["data_zamowienie"])),
                            };
                            order = newOrder;
                        }
                    }
                }
                connection.Close();
            }
            return order;
        }

        public CustomerOrder GetCustomer(int idCustomer)
        {
            CustomerOrder order = new CustomerOrder();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = "SELECT id_klient, imie, nazwisko, nr_telefonu, email FROM Klient WHERE id_klient = @id_klient";
                    command.Parameters.AddWithValue("@id_klient", idCustomer);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerOrder newOrder = new CustomerOrder()
                            {
                                IdCustomer = Convert.ToInt32(reader["id_klient"]),
                                FirstName = reader["imie"].ToString(),
                                LastName = reader["nazwisko"].ToString(),
                                PhoneNumber = reader["nr_telefonu"].ToString(),
                                Email = reader["email"].ToString()
                            };
                            order = newOrder;
                        }
                    }
                }
                connection.Close();
            }
            return order;
        }


    }
}
