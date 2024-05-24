using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using MySqlX.XDevAPI;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;


namespace Onboarding_AWAQ.Pages
{
    public class LeaderboardModel : PageModel
    {
        public bool admin { get; set; }
        public string src { get; set; }
        public List<Puntaje> Leaderboard { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("usuario")) == true)
            {
                Response.Redirect("index");
            };
            admin = Convert.ToBoolean(HttpContext.Session.GetString("permisos"));
            src = HttpContext.Session.GetString("src");

            Leaderboard = await PuntajeUsuario();
            return Page();
        }

        static async Task<List<Puntaje>> PuntajeUsuario()
        {
            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://localhost:7117/");
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            List<Puntaje> Leaderboard = new List<Puntaje>();
            try
            {
                //Path interno del end point
                HttpResponseMessage Res = await cliente.GetAsync("getLeaderboard");
                //Checar si el estatus es correcto del HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Obtener el response recibido web api
                    var apiResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing la respuesta del web api y guardarlo en la lista
                    Leaderboard = JsonConvert.DeserializeObject<List<Puntaje>>(apiResponse);
                    Console.WriteLine(Leaderboard);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.Message);
            }
            return Leaderboard;
        }
    }
}
