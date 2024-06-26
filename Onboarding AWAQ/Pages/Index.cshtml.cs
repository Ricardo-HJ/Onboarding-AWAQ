﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.Rewrite;
using static System.Net.Mime.MediaTypeNames;


namespace Onboarding_AWAQ.Pages
{

	public class IndexModel : PageModel
	{
		[BindProperty] public string correo { get; set; }
		[BindProperty] public string contrasena { get; set; }
		[BindProperty] public string mensaje { get; set; }
        [BindProperty] public string mensajeContra { get; set; }
        public string apiKey { get; set; }
		public void OnGet()
		{
			if (string.IsNullOrEmpty(HttpContext.Session.GetString("usuario")) == false)
			{
                Response.Redirect("Leaderboard");
            };
        }
		public void OnPost()
		{
            DotNetEnv.Env.Load();
            string ConexionDB = "Server=127.0.0.1;Port=3306;Database=OnBoardingAWAQ;Uid=root;password=" + Environment.GetEnvironmentVariable("ASPNETCORE_DB_PASS").ToString();
			MySqlConnection Conexion = new MySqlConnection(ConexionDB);
			Conexion.Open();

			MySqlCommand CMD = new MySqlCommand();
			CMD.Connection = Conexion;
			string Comand = "select `idUsuario`,`nombre`, `contrasena`, `src`, `superUsuario` from usuario where correo = \"" + correo + "\";";
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
					usr.Nombre = registro["nombre"].ToString();
					usr.Contrasena = registro["Contrasena"].ToString()!;
					usr.superUsuario = Convert.ToBoolean(registro["superUsuario"]);
					usr.src = registro["src"].ToString();

					if (BCrypt.Net.BCrypt.Verify(contrasena, usr.Contrasena))
					{
						Conexion.Dispose();
                        HttpContext.Session.SetString("usuario", usr.Id.ToString());
                        HttpContext.Session.SetString("permisos", usr.superUsuario.ToString());
						HttpContext.Session.SetString("nombre", usr.Nombre.ToString());
                        HttpContext.Session.SetString("correo", usr.Correo.ToString());
						if(usr.src != "")
                        {
                            HttpContext.Session.SetString("src", usr.src);
                        } else
						{
                            HttpContext.Session.SetString("src", "./img/User Circle.png");
                        }
						
						if(usr.superUsuario){
							Response.Redirect("DashboardAdmin");
						} else {
							Response.Redirect("Dashboard");
						}
                        
					} 
					else
					{
                        mensajeContra = "Contraseña incorrecta";
                    }
                }
			}
			catch (MySqlException)
			{
				if(correo == "" || correo == null){
					mensaje = "Favor de ingresar un correo";

				} else{
					mensaje = "Correo no válido";
				}
			}
			catch (IndexOutOfRangeException)
			{
				mensaje = "No se pudo obtener la información";
			} 
			catch (ArgumentNullException)
			{
				mensajeContra = "Favor de ingresar una contraseña";
			}

			Conexion.Dispose();
		}
	}
}
