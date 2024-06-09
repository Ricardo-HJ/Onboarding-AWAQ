using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Policy;

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
			int idDepartamento = 0;
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
                    idDepartamento = Convert.ToInt32(registro["idDepartamento"]);
				}
				else
				{
					conexion.Close();
					return null;
				}
			}
            conexion.Close();
			return idDepartamento;

        }

        /* Obtener promedio de los usuarios */
        [Route("getAverage/")]
        [HttpGet]
        public double? getAverage()
        {
			double promedio = 0;
            string connectionString = config.GetConnectionString("AWAQLocal");
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conexion;
            cmd.CommandText = "getAveragePoints";

            using (var registro = cmd.ExecuteReader())
            {
                if (registro.HasRows)
                {
                    registro.Read();
                    promedio = Convert.ToDouble(registro["promedio"]);
                }
                else
                {
                    conexion.Close();
                    return null;
                }
            }
            conexion.Close();
			return promedio;
        }

        /* Obtener puntos por departamento */
        [Route("getUserStats/")]
        [HttpGet]
        public List<Zona>? getUserStats()
        {
            List<Zona> ListaStat = new List<Zona>();
            string connectionString = config.GetConnectionString("AWAQLocal");
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conexion;
            cmd.CommandText = "getUserStats";

            using (var registro = cmd.ExecuteReader())
            {
                if (!registro.HasRows)
                {
                    conexion.Close();
                    return null;
                }
                while (registro.Read())
                {
                    Zona stat = new Zona();
                    stat.zona = registro["zona"].ToString();
                    stat.progreso = Convert.ToInt32(registro["cantidad"]);
                    ListaStat.Add(stat);
                }
            }
            conexion.Close();
            return ListaStat;
        }

        /* Obtener puntos por departamento */
        [Route("getQuestions/{idUsuario}")]
        [HttpGet]
        public List<PreguntaUsuario>? getQuestionStats(int idUsuario)
        {
            List<PreguntaUsuario> ListaStat = new List<PreguntaUsuario>();
            string connectionString = config.GetConnectionString("AWAQLocal");
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conexion;
            cmd.CommandText = "getUserQuestionStats";
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
                    PreguntaUsuario stat = new PreguntaUsuario();
                    stat.usuario = registro["nombre"].ToString();
                    stat.minijuego = registro["minijuego"].ToString();
                    stat.pregunta = registro["pregunta"].ToString();
                    stat.tiempo = Convert.ToInt32(registro["segundos"]);
                    stat.acierto = Convert.ToBoolean(registro["acierto"]);
                    ListaStat.Add(stat);
                }
            }
            conexion.Close();
            return ListaStat;
        }

        /* Obtener puntos por departamento */
        [Route("getAverageZones/")]
        [HttpGet]
        public List<Area>? getAvergaeZone()
        {
            List<Area> ListaDepartamento = new List<Area>();
            string connectionString = config.GetConnectionString("AWAQLocal");
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conexion;
            cmd.CommandText = "getAverageZoneProgres";

            using (var registro = cmd.ExecuteReader())
            {
                if (!registro.HasRows)
                {
                    conexion.Close();
                    return null;
                }
                while (registro.Read())
                {
                    Area zona = new Area();
                    zona.zona = registro["zona"].ToString();
                    zona.progreso = Convert.ToInt32(registro["progreso"]);
                    ListaDepartamento.Add(zona);
                }
            }
            conexion.Close();
            return ListaDepartamento;
        }

        /* Obtener progreso por departamento */
        [Route("getAverageDepartamento/")]
        [HttpGet]
        public List<Departamento>? getAvergaeDepartamento()
        {
            List<Departamento> ListaDepartamento = new List<Departamento>();
            string connectionString = config.GetConnectionString("AWAQLocal");
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conexion;
            cmd.CommandText = "getDepartmentProgress";

            using (var registro = cmd.ExecuteReader())
            {
                if (!registro.HasRows)
                {
                    conexion.Close();
                    return null;
                }
				while (registro.Read())
                {
                    Departamento departamento = new Departamento();
                    departamento.departamento = registro["departamento"].ToString();
                    departamento.progreso = Convert.ToInt32(registro["Progreso Total"]);
                    ListaDepartamento.Add(departamento);
                }
            }
            conexion.Close();
            return ListaDepartamento;
        }

        /* Obtener puntajes del usuario */
        [Route("getLeaderboard/")]
		[HttpGet]
		public List<Puntaje>? getLeaderboard()
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
					puntaje.tiempoJugado = TransformTime(Convert.ToInt32(registro["tiempoJugado"]));
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
