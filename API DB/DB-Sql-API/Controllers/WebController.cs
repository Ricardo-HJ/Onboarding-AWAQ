using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DB_Sql_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WebController : ControllerBase
	{
		private readonly IConfiguration config;
		public WebController(IConfiguration configuration)
		{
			config = configuration;
		}

		/* Obtener nombre y correo */
		[Route("getNombre/{correo}")]
		[HttpGet]
		public Usuario? GetNombreUsuario(string correo)
		{
			Usuario usuario = new Usuario();
			string connectionString = config.GetConnectionString("AWAQLocal");
			MySqlConnection conexion = new MySqlConnection(connectionString);
			conexion.Open();

			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = conexion;
			cmd.CommandText = "getUsuarioByCorreo";
			cmd.Parameters.AddWithValue("correo", correo);

			using (var registro = cmd.ExecuteReader())
			{
				if (registro.HasRows)
				{
					registro.Read();
					usuario.idUsuario = Convert.ToInt32(registro["idUsuario"]);
					usuario.nombre = (registro["nombre"]).ToString();
				}
				else
				{
					conexion.Close();
					return null;
				}
			}
			conexion.Close();
			return usuario;
		}

		/* Login de la web */
		[Route("getLoginData-W/{correo}")]
		[HttpGet]
		public Usuario? GetLoginUsuario(string correo)
		{
			Usuario usuario = new Usuario();
			string connectionString = config.GetConnectionString("AWAQLocal");
			MySqlConnection conexion = new MySqlConnection(connectionString);
			conexion.Open();

			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = conexion;
			cmd.CommandText = "loginWeb";
			cmd.Parameters.AddWithValue("correo", correo);

			using (var registro = cmd.ExecuteReader())
			{
				if (registro.HasRows)
				{
					registro.Read();
					usuario.idUsuario = Convert.ToInt32(registro["idUsuario"]);
					usuario.src = (registro["src"]).ToString();
					usuario.superUser = Convert.ToBoolean(registro["superUsuario"]);
					usuario.contrasena = (registro["contrasena"]).ToString();
				}
				else
				{
					conexion.Close();
					return null;
				}
			}
			conexion.Close();
			return usuario;
		}

		/* Obtener Id del departamento */
		[Route("getDepartamento/{departamento}")]
		[HttpGet]
		public int? GetDepartamento(string departamento)
		{
			string connectionString = config.GetConnectionString("AWAQLocal");
			MySqlConnection conexion = new MySqlConnection(connectionString);
			conexion.Open();

			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = conexion;
			cmd.CommandText = "selectDepartmentID";
			cmd.Parameters.AddWithValue("departamento", departamento);

			using (var registro = cmd.ExecuteReader())
			{
				if (registro.HasRows)
				{
					registro.Read();
					conexion.Close();
					return Convert.ToInt32(registro["idDepartamento"]);
				}
				else
				{
					conexion.Close();
					return null;
				}
			}
		}

		/* Obtener puntajes del usuario */
		[Route("getLeaderboard/")]
		[HttpGet]
		public IEnumerable<Puntaje>? getLeaderboard()
		{
			List<Puntaje> ListaPuntaje = new List<Puntaje>();
			string connectionString = config.GetConnectionString("AWAQLocal");
			MySqlConnection conexion = new MySqlConnection(connectionString);
			conexion.Open();

			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = conexion;
			cmd.CommandText = "getLeaderboard";

			using (var registro = cmd.ExecuteReader())
			{
				if (!registro.HasRows) {
					conexion.Close();
					return null;
				}
				int i = 1;
				while (registro.Read())
				{
					Puntaje puntaje = new Puntaje();
					puntaje.position = i;
					puntaje.src = registro["src"].ToString();
					puntaje.name = registro["nombre"].ToString();
					puntaje.tiempoJugado = Convert.ToInt32(registro["tiempoJugado"]);
					puntaje.terminado = registro["terminado"].ToString();
					puntaje.departamento = registro["departamento"].ToString();
					puntaje.puntaje = Convert.ToInt32(registro["puntos"]);
					ListaPuntaje.Add(puntaje);
					i++;
				}
			}
			conexion.Close();
			return ListaPuntaje;
		}

		/* Obtener estadisticas de preguntas */
		[Route("getQuestionStatistics/{idUsuario}")]
		[HttpGet]
		public List<EstadisticasPregunta>? getQuestionStatistics(int idUsuario)
		{
			List<EstadisticasPregunta> Estadisticas = new List<EstadisticasPregunta>();
			string connectionString = config.GetConnectionString("AWAQLocal");
			MySqlConnection conexion = new MySqlConnection(connectionString);
			conexion.Open();

			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = conexion;
			cmd.CommandText = "getStatsPreguntas";
			cmd.Parameters.AddWithValue("idUsuario", idUsuario);

			using (var registro = cmd.ExecuteReader())
			{
				if (!registro.HasRows)
				{
					conexion.Close();
					return null;
				}
				while (registro.Read())
				{
					EstadisticasPregunta estadistica = new EstadisticasPregunta();
					estadistica.clase = registro["Clase"].ToString();
					estadistica.cantidad = Convert.ToInt32(registro["Cantidad"]);
					Estadisticas.Add(estadistica);
				}
			}
			conexion.Close();
			return Estadisticas;
		}

        /* Obtener cambios en el puntaje */
        [Route("getHistroicPoints/{idUsuario}")]
        [HttpGet]
        public List<HistorialPuntos>? getHistroicPoints(int idUsuario)
        {
            List<HistorialPuntos> Estadisticas = new List<HistorialPuntos>();
            string connectionString = config.GetConnectionString("AWAQLocal");
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conexion;
            cmd.CommandText = "getPointsChange";
            cmd.Parameters.AddWithValue("idUsuario", idUsuario);

            using (var registro = cmd.ExecuteReader())
            {
                if (!registro.HasRows)
                {
                    conexion.Close();
                    return null;
                }
                while (registro.Read())
                {
                    HistorialPuntos estadistica = new HistorialPuntos();
					DateTime fecha = Convert.ToDateTime(registro["fecha"]);
                    estadistica.fecha = DateOnly.FromDateTime(fecha);
                    estadistica.puntos = Convert.ToInt32(registro["puntos"]);
                    Estadisticas.Add(estadistica);
                }
            }
            conexion.Close();
            return Estadisticas;
        }

        /* Obtener puntos por zona */
        [Route("getZonePoints/{idUsuario}")]
        [HttpGet]
        public List<Area>? getZonePoints(int idUsuario)
        {
            List<Area> Estadisticas = new List<Area>();
            string connectionString = config.GetConnectionString("AWAQLocal");
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conexion;
            cmd.CommandText = "getZonePoints";
            cmd.Parameters.AddWithValue("idUsuario", idUsuario);

            using (var registro = cmd.ExecuteReader())
            {
                if (!registro.HasRows)
                {
                    conexion.Close();
                    return null;
                }
                while (registro.Read())
                {
                    Area estadistica = new Area();
                    estadistica.zona = registro["zona"].ToString();
                    estadistica.progreso = Convert.ToInt32(registro["progreso"]);
                    estadistica.puntos = Convert.ToInt32(registro["puntos"]);
                    Estadisticas.Add(estadistica);
                }
            }
            conexion.Close();
            return Estadisticas;
        }

        /* Obtener tiempo promedio por pregunta */
        private string[] Transform(double time)
		{
			string[] values = new string[2];
            if (time >= 60 && time < 3600)
            {
				values[0] = time / 60 + ":" + time % 60;
				values[1] = "minutos";
            }
            else if (time >= 3600)
            {
				values[0] = time / 3600 + ":" + (time % 3600) / 60;
				values[1] = "horas";
            }
            else
            {
				values[0] = time.ToString();
				values[1] = "segundos";
            }
            return values;
		}


        [Route("getAverageTimeQuestion/{idUsuario}")]
        [HttpGet]
        public Tiempo? getAverageTimeQuestion(int idUsuario)
        {
			Tiempo promedio = new Tiempo();
            string connectionString = config.GetConnectionString("AWAQLocal");
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conexion;
            cmd.CommandText = "getAverageTime";
            cmd.Parameters.AddWithValue("idUsuario", idUsuario);

            using (var registro = cmd.ExecuteReader())
            {
				try
				{
					registro.Read();
					string[] data = Transform(Convert.ToDouble(registro["tiempo"]));
					promedio.tiempo = data[0];
					promedio.unidad = data[1];
				} 
				catch (InvalidCastException)
				{
					return null;
				}

            }
            conexion.Close();
            return promedio;
        }

        /* Obtener estadisticas por Area */
        private string TransformTime(int time)
		{
			if(time >= 60 && time < 3600)
			{
				return time / 60 + ":" + time % 60 + " minutos";
			}
			else if(time >= 3600)
			{
				return time / 3600 + ":" + (time % 3600) / 60 + " horas";
			}
			else
			{
                return time + " segundos";
			}
		}


        [Route("getAreaStats/{idUsuario}")]
        [HttpGet]
        public List<Area>? getAreaStats(int idUsuario)
        {
            List<Area> Estadisticas = new List<Area>();
            string connectionString = config.GetConnectionString("AWAQLocal");
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conexion;
            cmd.CommandText = "getAreaStats";
            cmd.Parameters.AddWithValue("idUsuario", idUsuario);

            using (var registro = cmd.ExecuteReader())
            {
                if (!registro.HasRows)
                {
                    conexion.Close();
                    return null;
                }
                while (registro.Read())
                {
                    Area estadistica = new Area();
                    estadistica.zona = registro["zona"].ToString();
                    estadistica.progreso = Convert.ToInt32(registro["progreso"]);
					estadistica.puntos = Convert.ToInt32(registro["puntos"]);
                    estadistica.tiempo = TransformTime(Convert.ToInt32(registro["tiempo"]));
                    estadistica.pCorrectas = Convert.ToInt32(registro["pCorrectas"]);
                    estadistica.pIncorrectas = Convert.ToInt32(registro["pIncorrectas"]);
                    Estadisticas.Add(estadistica);
                }
            }
            conexion.Close();
            return Estadisticas;
        }

        /* Obtener el ultimo token */
        [Route("getLastToken/{idUsuario}")]
		[HttpGet]
		public int? getLastToken(int idUsuario)
		{
			string connectionString = config.GetConnectionString("AWAQLocal");
			MySqlConnection conexion = new MySqlConnection(connectionString);
			conexion.Open();

			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = conexion;
			cmd.CommandText = "getToken";
			cmd.Parameters.AddWithValue("idUsuario", idUsuario);

			using (var registro = cmd.ExecuteReader())
			{
				if (registro.HasRows)
				{
					registro.Read();
					conexion.Close();
					return Convert.ToInt32(registro["token"]);
				}
				else
				{
					conexion.Close();
					return null;
				}
			}
		}

		/* Crear el departamento del usuario */
		[Route("createUsuarioDepartamento/{idDepartamento}/{idUsuario}")]
		[HttpPost]
		public int? createUsuarioDepartamento(int idDepartamento, int idUsuario)
		{
			string connectionString = config.GetConnectionString("AWAQLocal");
			MySqlConnection conexion = new MySqlConnection(connectionString);
			conexion.Open();

			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = conexion;
			cmd.CommandText = "createUsuarioDepartamento";
			cmd.Parameters.AddWithValue("idDepartamento", idDepartamento);
			cmd.Parameters.AddWithValue("idUsuario", idUsuario);
			
			try
			{
				cmd.ExecuteNonQuery();
			} catch (MySqlException)
			{
				conexion.Close();
				return null;
			}
			conexion.Close();
			return 200;
		}

		/* Insertar token */
		[Route("insertToken/{idUsuario}/{token}")]
		[HttpPost]
		public int? insertToken(int idUsuario, string token)
		{
			string connectionString = config.GetConnectionString("AWAQLocal");
			MySqlConnection conexion = new MySqlConnection(connectionString);
			conexion.Open();

			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = conexion;
			cmd.CommandText = "insertToken";
			cmd.Parameters.AddWithValue("idUsuario", idUsuario);
			cmd.Parameters.AddWithValue("token", token);

			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (MySqlException)
			{
				conexion.Close();
				return null;
			}
			conexion.Close();
			return 200;
		}

		/* Cambiar imagen de perfil */ 
		[Route("changeProfileImage/{correo}/{src}")]
		[HttpPut]
		public int? changeProfileImage(string correo, string src)
		{
			string connectionString = config.GetConnectionString("AWAQLocal");
			MySqlConnection conexion = new MySqlConnection(connectionString);
			conexion.Open();

			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = conexion;
			cmd.CommandText = "changeSrc";
			cmd.Parameters.AddWithValue("correo", correo);
			cmd.Parameters.AddWithValue("src", src);

			bool result = Convert.ToBoolean(cmd.ExecuteNonQuery());
			if (result)
			{
				conexion.Close();
				return 200;
			} else
			{
				conexion.Close();
				return null;
			}
		}

		/* Cambiar contraseña */
		[Route("changePassword/{correo}/{contrasena}")]
		[HttpPut]
		public int? changePassword(string correo, string contrasena)
		{
			string connectionString = config.GetConnectionString("AWAQLocal");
			MySqlConnection conexion = new MySqlConnection(connectionString);
			conexion.Open();

			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = conexion;
			cmd.CommandText = "cambiarContra";
			cmd.Parameters.AddWithValue("correo", correo);
			cmd.Parameters.AddWithValue("contrasena", contrasena);

			bool result = Convert.ToBoolean(cmd.ExecuteNonQuery());
			if (result)
			{
				conexion.Close();
				return 200;
			}
			else
			{
				conexion.Close();
				return null;
			}
		}

		/* Borrar tokens */
		[Route("deleteTokens/{idUsuario}")]
		[HttpDelete]
		public int? deleteTokens(int idUsuario)
		{
			string connectionString = config.GetConnectionString("AWAQLocal");
			MySqlConnection conexion = new MySqlConnection(connectionString);
			conexion.Open();

			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = conexion;
			cmd.CommandText = "deleteToken";
			cmd.Parameters.AddWithValue("idUsuario", idUsuario);

			bool result = Convert.ToBoolean(cmd.ExecuteNonQuery());
			if (result)
			{
				conexion.Close();
				return 200;
			}
			else
			{
				conexion.Close();
				return null;
			}
		}
	}
}
