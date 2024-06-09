using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Data;
using Microsoft.Win32;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DB_Sql_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class VJController : ControllerBase
	{
		private readonly IConfiguration config;
		public VJController(IConfiguration configuration)
		{
			config = configuration;
		}

		/* Login del Videojuego */
		[Route("getLoginData-VJ/{correo}")]
		[HttpGet]
		public Usuario? GetLoginVideoJuego(string correo)
		{
			Usuario usuario = new Usuario();
			string connectionString = config.GetConnectionString("AWAQLocal");
			MySqlConnection conexion = new MySqlConnection(connectionString);
			conexion.Open();

			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = conexion;
			cmd.CommandText = "loginVideoJuego";
			cmd.Parameters.AddWithValue("correo", correo);

			using (var registro = cmd.ExecuteReader())
			{
				if (registro.HasRows)
				{
					registro.Read();
					usuario.idUsuario = Convert.ToInt32(registro["idUsuario"]);
					usuario.nombre = (registro["nombre"]).ToString();
                    usuario.contrasena = (registro["contrasena"]).ToString();
					usuario.departamento = (registro["departamento"]).ToString();
					usuario.tiempoJugado = Convert.ToInt32(registro["tiempoJugado"]);
				}
				else
				{
					return null;
				}
			}
			return usuario;
		}

        /* Leaderboard del Videojuego */
        [Route("getLeaderboard-VJ/")]
        [HttpGet]
        public IEnumerable<Puntaje>? GetLeaderboard()
        {	
            List<Puntaje> usuarioList = new List<Puntaje>();
            string connectionString = config.GetConnectionString("AWAQLocal");
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conexion;
            cmd.CommandText = "getGameLeaderboard";

			int i = 1;
            using (var registro = cmd.ExecuteReader())
            {
				if (!registro.HasRows) { return null; }
                while (registro.Read())
                {
					Puntaje usuario = new Puntaje();
					usuario.position = i;
                    usuario.name = (registro["nombre"]).ToString();
                    usuario.puntaje = Convert.ToInt32(registro["puntos"]);
					usuarioList.Add(usuario);
					i++;
                }
            }
            return usuarioList;
        }

        /* Obtener preguntas del minijuego */
        [Route("getPreguntas/{idMinijuego}")]
		[HttpGet]
		public IEnumerable<Pregunta>? GetLoginVideoJuego(int idMinijuego)
		{
			List<Pregunta> ListaPreguntas = new List<Pregunta>();
			string connectionString = config.GetConnectionString("AWAQLocal");
			MySqlConnection conexion = new MySqlConnection(connectionString);
			conexion.Open();

			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = conexion;
			cmd.CommandText = "getPreguntasMinijuego";
			cmd.Parameters.AddWithValue("idMinijuego", idMinijuego);

			using (var registro = cmd.ExecuteReader())
			{
				if (!registro.HasRows) {return null;}
				while (registro.Read())
				{
					Pregunta pregunta = new Pregunta();
					pregunta.idPregunta = Convert.ToInt32(registro["idPregunta"]);
					pregunta.pregunta = registro["pregunta"].ToString();
					ListaPreguntas.Add(pregunta);
				}
			}

            foreach (var pregunta in ListaPreguntas)
            {
				MySqlCommand CMD = new MySqlCommand();
				CMD.CommandType = CommandType.StoredProcedure;
				CMD.Connection = conexion;
				CMD.CommandText = "getRespuestasPregunta";
				CMD.Parameters.AddWithValue("idPregunta", pregunta.idPregunta);

				using (var registroResp = CMD.ExecuteReader())
				{
					if (!registroResp.HasRows)
					{
						Respuesta respuesta = new Respuesta();
						pregunta.respuestas.Add(respuesta);
					}
					while (registroResp.Read())
					{
						Respuesta respuesta = new Respuesta();
						respuesta.idRespuesta = Convert.ToInt32(registroResp["idRespuesta"]);
						respuesta.respuesta = registroResp["respuesta"].ToString();
						respuesta.respuestaCorrecta = Convert.ToBoolean(registroResp["correcta"]);
						pregunta.respuestas.Add(respuesta);
					}
				}
			}
            return ListaPreguntas;
		}

		/* Cambiart tiempo jugado por usuario */
		[Route("updatePlayTime/{idUsuario}/{time}")]
		[HttpPut]
		public int? updatePlayTime(int idUsuario, int time)
		{

			string connectionString = config.GetConnectionString("AWAQLocal");
			MySqlConnection conexion = new MySqlConnection(connectionString);
			conexion.Open();

			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = conexion;
			cmd.CommandText = "changePlayTime";
			cmd.Parameters.AddWithValue("idUsuario", idUsuario);
			cmd.Parameters.AddWithValue("tiempoJugado", time);
			
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

        /* Cambiar informacion de preguntas */
        [Route("updateMijiuego/{minijuego}/{time}/{points}/{idUsuario}")]
        [HttpPut]
        public int? updateMinijuego(string minijuego, int time, int points, int idUsuario)
        {

            string connectionString = config.GetConnectionString("AWAQLocal");
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conexion;
            cmd.CommandText = "updateMinijuego";
            cmd.Parameters.AddWithValue("minijuego", minijuego);
            cmd.Parameters.AddWithValue("tiempo", time);
            cmd.Parameters.AddWithValue("puntos", points);
            cmd.Parameters.AddWithValue("idUsuario", idUsuario);


			try
			{
				cmd.ExecuteNonQuery();
			}
			catch (MySqlException ex)
			{
                conexion.Close();
                return null;
            }
            conexion.Close();
            return 200;
        }

        /* Cambiar informacion de preguntas */
        [Route("updatePregunta/{idPregunta}/{time}/{acierto}/{idUsuario}")]
		[HttpPut]
		public int? updatePregunta(int idPregunta, int time, bool acierto, int idUsuario)
		{

			string connectionString = config.GetConnectionString("AWAQLocal");
			MySqlConnection conexion = new MySqlConnection(connectionString);
			conexion.Open();

			MySqlCommand cmd = new MySqlCommand();
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Connection = conexion;
			cmd.CommandText = "changePregunta";
			cmd.Parameters.AddWithValue("idPregunta", idPregunta);
			cmd.Parameters.AddWithValue("segundos", time);
			cmd.Parameters.AddWithValue("acierto", acierto);
            cmd.Parameters.AddWithValue("idUsuario", idUsuario);


            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                conexion.Close();
                return null;
            }
            conexion.Close();
            return 200;
        }
	}
}
