using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Onboarding_AWAQ;
using Org.BouncyCastle.Crypto.Macs;
using System.Net;
using System.Net.Mail;

namespace WebApp_AWAQ.Pages
{
    public class RecuperacionModel : PageModel
    {
        [BindProperty] public string stage { get; set; }
        [BindProperty] public string token { get; set; }
        [BindProperty] public bool validToken { get; set; }
        [BindProperty] public string mensaje { get; set; }
        [BindProperty] public string correo {  get; set; }
        [BindProperty] public string contraseña { get; set; }
        [BindProperty] public string verificarContraseña { get; set; }

        public void OnGet()
        {
            stage = "";
            token = "";
            validToken = false;
        }

        public void OnPost()
        {
            if (stage == "recuperacion")
            {
                string ConexionDB = "Server=127.0.0.1;Port=3306;Database=OnBoardingAWAQ;Uid=root;password=Ts3A8AC2@23";
                MySqlConnection Conexion = new MySqlConnection(ConexionDB);
                Conexion.Open();

                MySqlCommand CMD = new MySqlCommand();
                CMD.Connection = Conexion;
                string Comand = "select `idUsuario` from usuario where correo = \"" + correo + "\";";
                CMD.CommandText = Comand;

                try
                {
                    using (var registro = CMD.ExecuteReader())
                    {
                        registro.Read();
                    }
                    string caracteresPosibles = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    Random randomizer = new Random();
                    for (int i = 0; i < 4; i++)
                    {
                        int index = randomizer.Next(0, caracteresPosibles.Length - 1);
                        token += caracteresPosibles[index]; 
                    }

                    MailMessage Correo = new MailMessage();
                    Correo.From = new MailAddress("AWAQ.NoReply@gmail.com");
                    Correo.To.Add("A00837072@tec.mx");
                    Correo.Subject = "Recuperar contraseña OnBoarding AWAQ";
                    Correo.Body = "Tu codigo de recuperacion es" + token;
                    Correo.Priority = MailPriority.Normal;

                    SmtpClient servidorCorreo = new SmtpClient();
                    servidorCorreo.UseDefaultCredentials = false;
                    servidorCorreo.Credentials = new NetworkCredential("awaq.noreply@gmail.com", "AWAQOnBoarding@14");
                    servidorCorreo.Host = "smtp.gmail.com";
                    servidorCorreo.Port = 587;
                    servidorCorreo.EnableSsl = true;
                    try
                    {
                        servidorCorreo.Send(Correo);
                    } catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    finally
                    {
                        Correo.Dispose();
                        servidorCorreo.Dispose();
                    }
                }
                catch (MySqlException)
                {
                    mensaje = "El correo propocionado no se encuentra registrado";
                }
                catch (IndexOutOfRangeException)
                {
                    mensaje = "No se pudo obtener la informacion";
                }
                Conexion.Dispose();
            }
            
        }
    }
}
