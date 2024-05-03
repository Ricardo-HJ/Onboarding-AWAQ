using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Onboarding_AWAQ.Pages
{

	public class IndexModel : PageModel
	{
		[BindProperty] public string correo { get; set; }
		[BindProperty] public string contrasena { get; set; }
		[BindProperty] public string mensaje { get; set; }

		public string apiKey { get; set; }
		private readonly ILogger<IndexModel> _logger;

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
		public void OnPost()
		{
			mensaje = "Correo:" + correo + ", Contrasena: " + contrasena;

			string ConexionDB = "Server=127.0.0.1;Port=3306;Database=OnBoardingAWAQ;Uid=root;password=Ts3A8AC2@23";
			MySqlConnection Conexion = new MySqlConnection(ConexionDB);
			Conexion.Open();

			MySqlCommand CMD = new MySqlCommand();
			CMD.Connection = Conexion;
			string Comand = "select `idUsuario`, `contrasena` from usuario where correo = \"" + correo + "\";";
			CMD.CommandText = Comand;

			Usuario usr = new Usuario();
			try
			{
				using (var registro = CMD.ExecuteReader())
				{
                    registro.Read();
                    usr = new Usuario();
					usr.Id = Convert.ToInt32(registro["idUsuario"]);
					usr.Correo = correo;
					usr.Contrasena = registro["Contrasena"].ToString()!;

					if (usr.Contrasena == contrasena)
					{
						mensaje = "Correo y contrasena validos, inicio de sesion";
						Conexion.Dispose();
						Response.Redirect("Dashboard");
					}
					else
					{
						mensaje = "Contrasena invalida, no se pudo iniciar de sesion";
					}
				}
			}
			catch (MySqlException)
			{
				mensaje = "Correo o contrasena invalidos, no se pudo iniciar sesion";
			}
			catch (IndexOutOfRangeException)
			{
				mensaje = "No se pudo obtener la informacion";
			}
			Conexion.Dispose();
		}
	}
}
