using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class CancionesBD {

    // Metodo que obtiene los datos de las canciones del .db y crea una lista
    public static List<CancionFila> ObtenerCanciones(){	
	List<CancionFila> lista = new List<CancionFila>();
	
	using var conexion = conectar();
	using (var comando = conexion.CreateCommand())
	{
	    comando.CommandText = @"
    SELECT r.title, r.genre, p.name, a.name
    FROM rolas r
    LEFT JOIN performers p ON r.id_performer = p.id_performer
    LEFT JOIN albums a     ON r.id_album = a.id_album";
	    
	    using (var reader = comando.ExecuteReader()){
		while (reader.Read()){
		    var cancion = new CancionFila
			{
			    titulo = reader.IsDBNull(0) ? "" : reader.GetString(0),
			    genero = reader.IsDBNull(1) ? "" : reader.GetString(1),
			    performer = reader.IsDBNull(2) ? "" : reader.GetString(2),
			    album = reader.IsDBNull(3) ? "" : reader.GetString(3)
			};

		    lista.Add(cancion);
		}
	    }
	}
	return lista;
    }

    //Metodo para conectarnos al .db
    public static SqliteConnection conectar(){
	//Carpeta ~/.local/share/NombreApp
        string carpetaDatos = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"rocola");

        //Crearla en el disco (si ya existe, no hace nada)
        Directory.CreateDirectory(carpetaDatos);

        //Ruta del .db y revisar si ya existía ANTES de abrir la conexión
        string rutaDb = Path.Combine(carpetaDatos, "musica.db");
        bool existia = File.Exists(rutaDb);

        //Abrir la conexión (si el archivo no existe, aquí se crea vacío)
	string conexionString = $"Data Source={rutaDb};Mode=ReadWriteCreate;Foreign Keys=True";
	var conexion = new SqliteConnection(conexionString);
        conexion.Open();
	
        return conexion;
    }
}
