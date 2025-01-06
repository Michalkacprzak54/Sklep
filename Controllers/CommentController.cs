using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cms;
using SklepMVC.DAL;
using SklepMVC.Models;
using System.Data.SqlClient;

namespace SklepMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;
        private readonly string? _connectionStringMysql;
        private readonly GetData _getData;

        public CommentController(IConfiguration configuration, GetData getData)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetSection("ConnectionString")["Polaczenie"];
            _connectionStringMysql = _configuration.GetSection("ConnectionString")["Mysql"];
            _getData = getData;
        }

        [HttpPost]
        public ActionResult Add([Bind] Comment c)
        {
            if (ModelState.IsValid)
            {
                c.Id_client = Convert.ToInt32(Request.Cookies["CookieUserKlientID"]);
                c.Id_client_login = Convert.ToInt32(Request.Cookies["CookieUserID"]);
                c.Added_date = DateTime.Today;
                if (AddComment(c) == "Succes")
                {
                    return RedirectToAction("Details", "Product", new { id = c.Id_product });
                }
            }

            return RedirectToAction("Details", "Product", new { id = c.Id_product});
        }

        public ActionResult Edit(int id)
        {
            Comment comment = _getData.GetComment(id);
            if(comment.Id_client_login != Convert.ToInt32(Request.Cookies["CookieUserID"]))
            {
                return RedirectToAction("Details", "Product", new { id = comment.Id_product });
            }
            return View(comment);
        }
        
        [HttpPost]
        public ActionResult Edit([Bind] Comment c)
        {
            if (ModelState.IsValid)
            {
                c.Added_date = DateTime.Today;
                if (EditComment(c) == "Succes")
                {
                    return RedirectToAction("Details", "Product", new { id = c.Id_product });
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("Edit");
        }

        public string AddComment(Comment c)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionStringMysql))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand())
                {

                    command.Connection = connection;
                    command.CommandText = "Insert into comments  (id_klient_login, id_klient, id_produkt, tytul, opis, data_dodania, ocena)"
                        + "VALUES (@id_klient_login, @id_klient, @id_produkt, @tytul, @opis, @data_dodania, @ocena)";

                    command.Parameters.Add("@id_klient_login", MySqlDbType.Int32).Value = c.Id_client_login;
                    command.Parameters.Add("@id_klient", MySqlDbType.Int32).Value = c.Id_client;
                    command.Parameters.Add("@id_produkt", MySqlDbType.Int32).Value = c.Id_product;
                    command.Parameters.Add("@tytul", MySqlDbType.VarChar).Value = c.Title;
                    command.Parameters.Add("@opis", MySqlDbType.Text).Value = c.Description;
                    command.Parameters.Add("@data_dodania", MySqlDbType.Date).Value = c.Added_date;
                    command.Parameters.Add("@ocena", MySqlDbType.Int32).Value = c.Rate;

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            return ("Succes");
        }

        public string EditComment(Comment c)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionStringMysql))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand())
                {

                    command.Connection = connection;
                    command.CommandText = "Update comments SET tytul=@tytul, opis=@opis, data_dodania=@data_dodania, ocena=@ocena WHERE id_komentarz=@id_komentarz AND id_klient_login=@id_klient_login";

                    command.Parameters.Add("@tytul", MySqlDbType.VarChar).Value = c.Title;
                    command.Parameters.Add("@opis", MySqlDbType.Text).Value = c.Description;
                    command.Parameters.Add("@data_dodania", MySqlDbType.Date).Value = c.Added_date;
                    command.Parameters.Add("@ocena", MySqlDbType.Int32).Value = c.Rate;
                    command.Parameters.Add("@id_komentarz", MySqlDbType.Int32).Value = c.Id_comment;
                    command.Parameters.Add("@id_klient_login", MySqlDbType.Int32).Value = Convert.ToInt32(Request.Cookies["CookieUserID"]);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            return ("Succes");
        }


    }
}
