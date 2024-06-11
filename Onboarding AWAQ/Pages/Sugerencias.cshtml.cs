using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
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
    public class SugerenciasModel : PageModel
    {
        public bool admin { get; set; }
        public string src { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string mensajeComment { get; set; }
        public string mensajeArea { get; set; }
        [BindProperty] public string area {  get; set; }
		[BindProperty] public string comment { get; set; }

        public void OnGet()
        {
            admin = Convert.ToBoolean(HttpContext.Session.GetString("permisos"));
            nombre = HttpContext.Session.GetString("nombre");
            correo = HttpContext.Session.GetString("correo");
            src = HttpContext.Session.GetString("src");
            Console.WriteLine("Nombre: " + nombre);
            Console.WriteLine("Correo: " + correo);
        }

        public async Task<IActionResult> OnPost() 
        {

            admin = Convert.ToBoolean(HttpContext.Session.GetString("permisos"));
            src = HttpContext.Session.GetString("src");

            if (!((area == "Selecciona" || area == null) || (comment == "" || comment == null)))

            {
                /* Envia el correo en segundo plano */
                Task.Run(() => SendMail(correo, area, comment, nombre).Wait());
                if (admin)
                {
                    return RedirectToPage("DashboardAdmin");
                }
                else
                {
                    return RedirectToPage("Dashboard");
                }
            }
            else
            {
                if(area == "Selecciona" || area == null)
                {
                    mensajeArea = "Ingresar area";
                }

                if(comment == "" || comment == null)
                {
                    mensajeComment = "Ingresar comentario";
                }
                return Page();
            }
        }

        static async Task SendMail(string correo, string area, string comment, string nombre)
        {
            DotNetEnv.Env.Load();
            var apiKey = Environment.GetEnvironmentVariable("ASPNETCORE_API_KEY");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("awaq.noreply@gmail.com", "AWAQ Support"));
            msg.AddTo(new EmailAddress("ricardo_antoniohj@hotmail.com", nombre));
            msg.SetTemplateId("d-b9bf6a8e046b47dc913ba429b44be0c3");

            var dynamicTemplateData = new ExampleTemplateData
            {
                Name = nombre,
                Area = area,
                Comment = comment
            };

            msg.SetTemplateData(dynamicTemplateData);
            var response = await client.SendEmailAsync(msg);
        }

        private class ExampleTemplateData
        {
            [JsonProperty("Area")]
            public string Area { get; set; }

            [JsonProperty("Comment")]
            public string Comment { get; set; }

            [JsonProperty("Name")]
            public string Name { get; set; }
        }
    }
}
