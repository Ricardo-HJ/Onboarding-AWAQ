using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DB_Sql_API
{
	public class Usuario
	{
		public int idUsuario {  get; set; }
		public string nombre { get; set; }
		public string correo { get; set; }
		public string contrasena { get; set; }
		public string src { get; set; }	
		public string departamento { get; set;}
		public int tiempoJugado { get; set; }
		public bool superUser { get; set; }
	}
}
