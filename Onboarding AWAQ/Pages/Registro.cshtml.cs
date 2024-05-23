using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Mysqlx.Crud;
using Onboarding_AWAQ;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;


namespace Onboarding_AWAQ.Pages
{
    public class RegistroModel : PageModel
    {
        public bool admin { get; set; }
        public string src { get; set; }
        public string mensajePais { get; set; }
        public string mensajeCiudad { get; set; }
        public string mensajeDepartamento { get; set; }
        [BindProperty] public string pais {  get; set; }
		[BindProperty] public string ciudad { get; set; }
		[BindProperty] public string departamento { get; set; }
		[BindProperty][Required(ErrorMessage = "Ingresar nombre")] public string nombre { get; set; }
		[BindProperty][Required(ErrorMessage = "Ingresar correo")] public string correo { get; set; }
		[BindProperty][Required(ErrorMessage = "Ingresar telefono")] public string telefono { get; set; }
		[BindProperty][Required(ErrorMessage = "Ingresar pais")] public string contrasena { get; set; }
        [BindProperty][Required(ErrorMessage = "Ingresar una imagen")] public IFormFile perfil { get; set; }

        public void OnGet()
        {
            admin = Convert.ToBoolean(HttpContext.Session.GetString("permisos"));
            src = HttpContext.Session.GetString("src");

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("usuario")) == true)
            {
                Response.Redirect("index");
            } else if (HttpContext.Session.GetString("permisos") == "False")
            {
                /*Regresarlo a la ultima pagina*/
                Response.Redirect("Leaderboard");
            }
        }

        public async void OnPost() 
        {
            admin = Convert.ToBoolean(HttpContext.Session.GetString("permisos"));
            src = HttpContext.Session.GetString("src");

            if (!((pais == "Selecciona" || pais == null) || (ciudad == "Selecciona" || ciudad == null) || 
                (departamento == "Selecciona" || departamento == null) || (nombre == "" || nombre == null) || 
                (correo == "" || correo == null) || (telefono == "" || telefono == null) || (contrasena == "" || contrasena == null)))
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
                CMD.CommandText = "select `idUsuario` from usuario where correo = @correo;";
                CMD.Parameters.AddWithValue("@correo", correo);
                int idUsuario = 0;

                using (var registro = CMD.ExecuteReader())
                {
                    if (registro.HasRows)
                    {
                        registro.Read();
                        idUsuario = Convert.ToInt32(registro["idUsuario"]);
                    }
                }
                CMD.Dispose();
                string imagePath = await ImageUpload(perfil, idUsuario);

                CMD = new MySqlCommand();
                CMD.Connection = Conexion;
                CMD.CommandText = "update usuario set `src` = @src where correo = @correo;";
                CMD.Parameters.AddWithValue("@src", imagePath);
                CMD.Parameters.AddWithValue("@correo", correo);
                CMD.ExecuteNonQuery();

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
                }
                CMD.Dispose();

                CMD = new MySqlCommand();
                CMD.Connection = Conexion;
                CMD.CommandText = "insert into usuarioDepartamento (idDepartamento, idUsuario) values (@departamento, @usuario);";
                CMD.Parameters.AddWithValue("@departamento", idDepartamento);
                CMD.Parameters.AddWithValue("@usuario", idUsuario);
                CMD.ExecuteNonQuery();
                CMD.Dispose();

                SendMail(contrasena, correo, nombre, correo, contrasena).Wait();
                Conexion.Dispose();
                Response.Redirect("index");
            } else
            {
                if(pais == "Selecciona" || pais == null)
                {
                    mensajePais = "Ingresar pais";
                }

                if(ciudad == "Selecciona" || ciudad == null)
                {
                    mensajeCiudad = "Ingresar ciudad";
                }

                if (departamento == "Selecciona" || departamento == null)
                {
                    mensajeDepartamento = "Ingresar Departamento";
                }
            }
        }
        public async Task<string> ImageUpload(IFormFile image, int ID)
        {
            if (image.Length > 0)
            {
                /*Obtener el ultimo ID para setearlo como el nombre de la imagen*/
                var relativePath = "/profileImages/user" + ID + "ProfileImage" + System.IO.Path.GetExtension(image.FileName);
                var filePath = (Directory.GetCurrentDirectory()) + relativePath;
                using (var stream = System.IO.File.OpenWrite(filePath))
                {
                    await image.CopyToAsync(stream);
                }
                return "." + relativePath;
            }
            return "No image Provided";
        }
        static async Task SendMail(string token, string direccion, string nombre, string correo, string contrasena)
        {
            DotNetEnv.Env.Load();
            var apiKey = Environment.GetEnvironmentVariable("ASPNETCORE_API_KEY");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("awaq.noreply@gmail.com", "AWAQ Support"));
            msg.AddTo(new EmailAddress(direccion, nombre));
            msg.SetTemplateId("d-b24a58d6ff2349d29d6fab2d2f176e81");

            var dynamicTemplateData = new ExampleTemplateData
            {
                Name = nombre,
                Mail = correo,
                Pass = contrasena
            };

            msg.SetTemplateData(dynamicTemplateData);
            var response = await client.SendEmailAsync(msg);
        }

        private class ExampleTemplateData
        {
            [JsonProperty("Name")]
            public string Name { get; set; }

            [JsonProperty("Mail")]
            public string Mail { get; set; }

            [JsonProperty("Pass")]
            public string Pass { get; set; }
        }
    }
}
