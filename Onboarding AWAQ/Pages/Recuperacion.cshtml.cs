using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Onboarding_AWAQ;
using Org.BouncyCastle.Crypto.Macs;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Security.Cryptography;
using System.Text;

namespace WebApp_AWAQ.Pages
{
    public class RecuperacionModel : PageModel
    {
        public string token { get; set; }
        [BindProperty] public string inputToken { get; set; }
        [BindProperty] public string correo { get; set; }
        [BindProperty] public bool validToken { get; set; }
        [BindProperty] public bool validCorreo { get; set; }
        [BindProperty] public bool validContra { get; set; }

        public void OnGet()
        {
            token = "";
            validToken = false;
            validContra = false;
            validCorreo = false;
        }

        public void OnPost()
        {
            token = TokenGenerator.GenerateToken();
            foreach (var key in Request.Form)
            {
                if (key.Key == "isValidCorreo")
                {
                    validCorreo = bool.Parse(key.Value);
                }
                else if (key.Key == "isValidToken")
                {
                    validToken = bool.Parse(key.Value);
                }
                else if (key.Key == "isValidContra")
                {
                    validContra = bool.Parse(key.Value);
                }
            }

            if (!validCorreo)
            {
                string ConexionDB = "Server=127.0.0.1;Port=3306;Database=OnBoardingAWAQ;Uid=root;password=STM02";

                MySqlConnection Conexion = new MySqlConnection(ConexionDB);
                Conexion.Open();

                MySqlCommand CMD = new MySqlCommand();
                CMD.Connection = Conexion;
                CMD.CommandText = "select `idUsuario` from usuario where correo = @correo;";
                CMD.Parameters.AddWithValue("@correo", correo);
                string idUsuario = "";
                

                using (var registro = CMD.ExecuteReader())
                {
                    if (registro.HasRows)
                    {
                        validCorreo = true;
                        SendMail(token, correo).Wait();
                    
                        registro.Read();

                        idUsuario = (registro["idUsuario"]).ToString();
                        Response.Cookies.Append("ID", idUsuario);
                        Response.Cookies.Append("Correo", correo);

                    }
                    else
                    {
                        validCorreo = false;
                    }
                }

                if (validCorreo)
                {
                    MySqlCommand insert = new MySqlCommand();
                    insert.Connection = Conexion;
                    insert.CommandText = "insert into token (token, idUsuario, generado) values (@token, @idUsuario, now());";
                    insert.Parameters.AddWithValue("@idUsuario", idUsuario);
                    insert.Parameters.AddWithValue("@token", token);
                    insert.ExecuteNonQuery();
                    insert.Dispose();
                }
                Conexion.Dispose();

            } 
            else if(!validToken)
            {
                string ConexionDB = "Server=127.0.0.1;Port=3306;Database=OnBoardingAWAQ;Uid=root;password=STM02";
                MySqlConnection Conexion = new MySqlConnection(ConexionDB);
                Conexion.Open();

                MySqlCommand CMD = new MySqlCommand();
                CMD.Connection = Conexion;
                CMD.CommandText = "select token from token where idUsuario = @ID order by generado desc limit 1;";
                CMD.Parameters.AddWithValue("@ID", Request.Cookies["ID"]);

                using (var tokenRow = CMD.ExecuteReader())
                {
                    if (tokenRow.HasRows)
                    {
                        tokenRow.Read();
                        token = tokenRow["token"].ToString();
                    }
                }
                if(token == inputToken)
                {
                    validCorreo = true;
                    validToken = true;
                } 
                else
                {
                    validCorreo = true;
                    validToken = false;
                }

            } 
            else if(!validContra && validToken)
            {
                if (Request.Form["contrasena"] == Request.Form["verificarContrasena"])
                {
                    string ConexionDB = "Server=127.0.0.1;Port=3306;Database=OnBoardingAWAQ;Uid=root;password=STM02";
                    MySqlConnection Conexion = new MySqlConnection(ConexionDB);
                    Conexion.Open();

                    MySqlCommand CMD = new MySqlCommand();
                    CMD.Connection = Conexion;

                    CMD.CommandText = "update usuario set `contrasena` = @contrasena where correo = @correo;";
                    CMD.Parameters.AddWithValue("@contrasena", Request.Form["contrasena"]);
                    CMD.Parameters.AddWithValue("@correo", Request.Cookies["Correo"]);

                    CMD.ExecuteNonQuery();
                    CMD.Dispose();

                    CMD = new MySqlCommand();
                    CMD.Connection = Conexion;
                    CMD.CommandText = "delete from token where idUsuario = @ID;";
                    CMD.Parameters.AddWithValue("@ID", Request.Cookies["ID"]);
                    CMD.ExecuteNonQuery();

                    Conexion.Dispose();
                    Response.Cookies.Delete("ID");
                    Response.Cookies.Delete("Correo");
                    Response.Redirect("index");
                }
            }
        }

        static async Task SendMail(string token, string direccion)
        {
            DotNetEnv.Env.Load();
            string apiKey = Environment.GetEnvironmentVariable("ASPNETCORE_API_KEY");
            var cliente = new SendGridClient(apiKey);
            var from = new EmailAddress("awaq.noreply@gmail.com", "Support AWAQ");
            var to = new EmailAddress(direccion, "Support AWAQ");
            var subject = "Recuperar contrasena OnBoarding AWAQ";
            var plainText = "Su codigo de recuperacion es" + token;
            var htmlContent = "<p>Su codigo de recuperacion es <strong>" + token+ "</strong></p>";

            var correo = MailHelper.CreateSingleEmail(
                from,
                to,
                subject,
                plainText,
                htmlContent
            );
            var response = await cliente.SendEmailAsync(correo);
            Console.WriteLine(response.StatusCode);
        }

        public class TokenGenerator
        {
            private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            private const int TokenLength = 2;

            public static string GenerateToken()
            {
                byte[] data = new byte[TokenLength];
                using (var random = RandomNumberGenerator.Create())
                {
                    random.GetBytes(data);
                }

                var token = new string(Enumerable.Range(0, TokenLength).Select(x => Characters[new Random().Next(0, Characters.Length)]).ToArray());
                return token;
            }
        }
    }

}