namespace DB_Sql_API
{
	public class Pregunta
	{
		public int idPregunta { get; set; }
		public string pregunta { get; set; }
		public bool acierto { get; set; }
		public int tiempo { get; set; }
		public List<Respuesta> respuestas { get; set; }

		/* Constructor para inicializar la lista*/
		public Pregunta()
		{
			respuestas = new List<Respuesta>();
		}

	}

}
