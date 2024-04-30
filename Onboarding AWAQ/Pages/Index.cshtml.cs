using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Onboarding_AWAQ.Pages
{
	public class IndexModel : PageModel
	{
		[BindProperty] public string correo { get; set; }
		[BindProperty] public string contraseña { get; set; }
		[BindProperty] public string mensaje { get; set; }

		private readonly ILogger<IndexModel> _logger;

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{
			mensaje = "";
		}
		public void OnPost()
		{
			mensaje = "Correo:" + correo + ", Contraseña: " + contraseña;

			string ConexionDB = "Server=127.0.0.1;Port=3306;Database=OnBoardingAWAQ;Uid=root;password=Ts3A8AC2@23";
			MySqlConnection Conexion = new MySqlConnection(ConexionDB);
			Conexion.Open();

			MySqlCommand CMD = new MySqlCommand();
			CMD.Connection = Conexion;
			string Comand = "select `idUsuario`, `contraseña` from usuario where correo = \"" + correo + "\";";
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
					usr.Contraseña = registro["Contraseña"].ToString();

					if (usr.Contraseña == contraseña)
					{
						mensaje = "Correo y contraseña validos, inicio de sesion";
						Conexion.Dispose();
						Response.Redirect("Dashboard");
					}
					else
					{
						mensaje = "Contraseña invalida, no se pudo iniciar de sesion";
					}
				}
			}
			catch (MySqlException)
			{
				mensaje = "Correo o contraseña invalidos, no se pudo iniciar sesion";
				Conexion.Dispose();
			}
			catch (IndexOutOfRangeException)
			{
				mensaje = "No se pudo obtener la informacion";
				Conexion.Dispose();
			}
			Conexion.Dispose();
		}
	}
}
