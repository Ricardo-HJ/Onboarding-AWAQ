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
        }

        public void OnPost()
        {
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

            } else if(!validToken)
            {
                string ConexionDB = "Server=127.0.0.1;Port=3306;Database=OnBoardingAWAQ;Uid=root;password=Ts3A8AC2@23";
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
                } else
                {
                    validCorreo = true;
                    validToken = false;
                }

            } else if(!validContra && validToken)
            {   
                if (Request.Form["contraseña"] == Request.Form["verificarContraseña"])
                {
                    string ConexionDB = "Server=127.0.0.1;Port=3306;Database=OnBoardingAWAQ;Uid=root;password=Ts3A8AC2@23";
                    MySqlConnection Conexion = new MySqlConnection(ConexionDB);
                    Conexion.Open();

                    MySqlCommand CMD = new MySqlCommand();
                    CMD.Connection = Conexion;
                    CMD.CommandText = "update usuario set `contraseña` = @contraseña where correo = @correo;";
                    CMD.Parameters.AddWithValue("@contraseña", Request.Form["contraseña"]);
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
            var apiKey = "";
            var cliente = new SendGridClient(apiKey);
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
            Console.WriteLine(response.StatusCode);
        }
    }
}