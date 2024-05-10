using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Onboarding_AWAQ;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace Onboarding_AWAQ.Pages
{
    public class RegistroModel : PageModel
    {
        [BindProperty] public string pais {  get; set; }
        [BindProperty] public string ciudad { get; set; }
        [BindProperty] public string departamento { get; set; }
        [BindProperty] public string nombre { get; set; }
        [BindProperty] public string correo { get; set; }
        [BindProperty] public string telefono { get; set; }
        [BindProperty] public string contrasena { get; set; }
        [BindProperty] public string perfil { get; set; }


        public void OnGet()
        {
        }

        public void OnPost() 
        {
            DotNetEnv.Env.Load();
            string ConexionDB = "Server=127.0.0.1;Port=3306;Database=OnBoardingAWAQ;Uid=root;password=" + Environment.GetEnvironmentVariable("ASPNETCORE_DB_PASS");
            MySqlConnection Conexion = new MySqlConnection(ConexionDB);
            Conexion.Open();

            MySqlCommand CMD = new MySqlCommand();
            CMD.Connection = Conexion;
            CMD.CommandText = "insert into usuario (nombre, pais, ciudad, correo, telefono, contrasena) values (@nombre, @pais, @ciudad, @correo, @telefono, @contrasena);";

            CMD.Parameters.AddWithValue("@nombre", nombre);
            CMD.Parameters.AddWithValue("@pais", pais);
            CMD.Parameters.AddWithValue("@ciudad", ciudad);
            CMD.Parameters.AddWithValue("@correo", correo);
            CMD.Parameters.AddWithValue("@telefono", telefono);
            CMD.Parameters.AddWithValue("@contrasena", contrasena);
            CMD.ExecuteNonQuery();
            CMD.Dispose();

            CMD = new MySqlCommand();
            CMD.Connection = Conexion;
            CMD.CommandText = "select `idUsuario` from usuario where correo = @correo and nombre = @nombre;";
            CMD.Parameters.AddWithValue("@nombre", nombre);
            CMD.Parameters.AddWithValue("@correo", correo);
            int idUsuario = 0;

            using (var registro = CMD.ExecuteReader())
            {
                if (registro.HasRows)
                {
                    registro.Read();
                    idUsuario = Convert.ToInt32(registro["idUsuario"]);
                }
                else
                {
                    // Validacion y decir que no jala
                }
            }
            CMD.Dispose();

            CMD = new MySqlCommand();
            CMD.Connection = Conexion;
            CMD.CommandText = "select `idDepartamento` from departamento where departamento = @departamento;";
            CMD.Parameters.AddWithValue("@departamento", departamento);
            int idDepartamento = 0;

            using (var registro = CMD.ExecuteReader())
            {
                if (registro.HasRows)
                {
                    registro.Read();
                    idDepartamento = Convert.ToInt32(registro["idDepartamento"]);
                }
                else
                {
                    // Validacion y decir que no jala
                }
            }
            CMD.Dispose();

            CMD = new MySqlCommand();
            CMD.Connection = Conexion;
            CMD.CommandText = "insert into usuarioDepartamento (idDepartamento, idUsuario) values (@departamento, @usuario);";
            CMD.Parameters.AddWithValue("@departamento", idDepartamento);
            CMD.Parameters.AddWithValue("@usuario", idUsuario);
            CMD.ExecuteNonQuery();
            CMD.Dispose();

            SendMail(contrasena, correo).Wait();
            Conexion.Dispose();
            pais = "";
            ciudad = "";
            departamento = "";
            nombre = "";
            correo = "";
            telefono = "";
            contrasena = "";
            Response.Redirect("index");
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
            var htmlContent = "<p>Su contraseña temporal es <strong>" + token + "</strong></p>";

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
