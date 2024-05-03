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
using Mysqlx.Crud;

namespace WebApp_AWAQ.Pages
{
    public class RecuperacionModel : PageModel
    {
        public string token { get; set; }
        [BindProperty] public string inputToken { get; set; }
        [BindProperty] public string correo {  get; set; }
        [BindProperty] public bool validToken { get; set; }
        [BindProperty] public bool validCorreo { get; set; }
        [BindProperty] public bool validContra { get; set; }

        public void OnGet()
        {
            token = "";
            validToken = false;
            validContra = false;
            validCorreo = false;
            Console.WriteLine("isValidCorreo: " + validCorreo);
            Console.WriteLine("isValidToken: " + validToken);
            Console.WriteLine("isValidContra: " + validContra);
        }

        public void OnPost()
        {
            foreach (var key in Request.Form)
            {
                Console.WriteLine("" + key.Key + ": " + key.Value);
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

            Console.WriteLine("isValidCorreo: " + validCorreo);
            Console.WriteLine("isValidToken: " + validToken);
            Console.WriteLine("isValidContra: " + validContra);

            if (!validCorreo)
            {
                Console.WriteLine("Correo");
                string ConexionDB = "Server=127.0.0.1;Port=3306;Database=OnBoardingAWAQ;Uid=root;password=Ts3A8AC2@23";
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
                        string caracteresPosibles = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                        Random randomizer = new Random();
                        for (int i = 0; i < 4; i++)
                        {
                            int index = randomizer.Next(0, caracteresPosibles.Length - 1);
                            token += caracteresPosibles[index];
                        }
                        validCorreo = true;
                        SendMail(token, correo).Wait();

                        registro.Read();
                        idUsuario = (registro["idUsuario"]).ToString();

                    } else
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

            } else if(!validToken)
            {
                string ConexionDB = "Server=127.0.0.1;Port=3306;Database=OnBoardingAWAQ;Uid=root;password=Ts3A8AC2@23";
                MySqlConnection Conexion = new MySqlConnection(ConexionDB);
                Conexion.Open();

                MySqlCommand CMD = new MySqlCommand();
                CMD.Connection = Conexion;
                CMD.CommandText = "select token from token where idUsuario = @ID order by generado desc limit 1;";
                CMD.Parameters.AddWithValue("@ID", correo);
                Console.WriteLine(token);
                Console.WriteLine(Request.Form["token"]);
                if(token == Request.Form["token"])
                {
                    validCorreo = true;
                    validToken = true;
                } else
                {
                    validCorreo = true;
                    validToken = false;
                }

            } else if(!validContra && validToken)
            {   
                Console.WriteLine("Contraseña");
                if (Request.Form["contraseña"] == Request.Form["verificarContraseña"])
                {
                    Console.WriteLine("Cambiando Contraseña");
                    string ConexionDB = "Server=127.0.0.1;Port=3306;Database=OnBoardingAWAQ;Uid=root;password=Ts3A8AC2@23";
                    MySqlConnection Conexion = new MySqlConnection(ConexionDB);
                    Conexion.Open();

                    MySqlCommand CMD = new MySqlCommand();
                    CMD.Connection = Conexion;
                    CMD.CommandText = "update usuario set `contraseña` = @contraseña where correo = @correo ;";
                    CMD.Parameters.AddWithValue("@contraseña", Request.Form["contraseña"]);
                    CMD.Parameters.AddWithValue("@correo", correo);

                    CMD.ExecuteNonQuery();
                    Conexion.Dispose();
                    Response.Redirect("index");
                }
            }
        }

        static async Task SendMail(string token, string direccion)
        {
            var cliente = new SendGridClient(leo);
            var from = new EmailAddress("awaq.noreply@gmail.com", "Support AWAQ");
            var to = new EmailAddress(direccion, "Support AWAQ");
            var subject = "Recuperar contraseña OnBoarding AWAQ";
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
        }
    }
}