using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Google.Protobuf.WellKnownTypes;

namespace Onboarding_AWAQ.Pages
{
    public class DashboardAdminModel : PageModel
    {
        public bool admin { get; set; }
        public string src { get; set; }
        public List<Preguntas> ListaPreguntas { get; set; }
        public List<Area> ListaAreas { get; set; }
        public float promedio { get; set; }
        static async Task<float> promedioPreguntasUsuario(int id)
        {
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://localhost:7117/");
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            float promedio = 0;
            try
            {
                //Path interno del end point
                string url = "api/Web/getAverage";
                HttpResponseMessage Res = await cliente.GetAsync(url);
                //Checar si el estatus es correcto del HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Obtener el response recibido web api
                    var apiResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing la respuesta del web api y guardarlo en la lista
                    promedio = JsonConvert.DeserializeObject<float>(apiResponse);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.Message);
            }
            return promedio;
        }

        static async Task<List<Area>> Area(int id)
        {
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://localhost:7117/");
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            List<Area> Estadisticas = new List<Area> { };
            try
            {
                //Path interno del end point
                string url = "api/Web/getAverageZones";
                HttpResponseMessage Res = await cliente.GetAsync(url);
                //Checar si el estatus es correcto del HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Obtener el response recibido web api
                    var apiResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing la respuesta del web api y guardarlo en la lista
                    Estadisticas = JsonConvert.DeserializeObject<List<Area>>(apiResponse);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.Message);
            }
            return Estadisticas;
        }

        static async Task<List<Preguntas>> StatsPregunta(int id)
        {
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://localhost:7117/");
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            List<Preguntas> Estadisticas = new List<Preguntas> { };
            try
            {
                //Path interno del end point
                string url = "api/Web/getQuestionStatistics/" + id;
                HttpResponseMessage Res = await cliente.GetAsync(url);
                //Checar si el estatus es correcto del HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Obtener el response recibido web api
                    var apiResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing la respuesta del web api y guardarlo en la lista
                    Estadisticas = JsonConvert.DeserializeObject<List<Preguntas>>(apiResponse);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.Message);
            }
            return Estadisticas;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("usuario")) == true)
            {
                Response.Redirect("index");
            };
            admin = Convert.ToBoolean(HttpContext.Session.GetString("permisos"));
            src = HttpContext.Session.GetString("src");

            promedio = await promedioPreguntasUsuario(Convert.ToInt32(HttpContext.Session.GetString("usuario")));
            ListaAreas = await Area(Convert.ToInt32(HttpContext.Session.GetString("usuario")));
            ListaPreguntas = await StatsPregunta(Convert.ToInt32(HttpContext.Session.GetString("usuario")));
            return Page();
        }
    }
}
