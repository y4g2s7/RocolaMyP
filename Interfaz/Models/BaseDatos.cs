using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Interfaz.Models;

public class BaseDatos{

    // Metodo que obtiene los datos de las canciones del .db y crea una lista
    public static List<CancionFila> ObtenerCanciones(){	
	List<CancionFila> lista = new List<CancionFila>();
	
	using var conexion = conectar();
	using (var comando = conexion.CreateCommand())
	{
	    comando.CommandText = @"
    SELECT r.id_rola, r.title, r.genre, p.name, a.name
    FROM rolas r
    LEFT JOIN performers p ON r.id_performer = p.id_performer
    LEFT JOIN albums a     ON r.id_album = a.id_album";

	    using (var reader = comando.ExecuteReader()){
		while (reader.Read()){
		    var cancion = new CancionFila
			{
			    id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
			    titulo = reader.IsDBNull(1) ? "" : reader.GetString(1),
			    genero = reader.IsDBNull(2) ? "" : reader.GetString(2),
			    performer = reader.IsDBNull(3) ? "" : reader.GetString(3),
			    album = reader.IsDBNull(4) ? "" : reader.GetString(4)
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
    
    public static Titulo? getDatosTitulo(int id){
	using var conexion = conectar();
	using (var comando = conexion.CreateCommand())
	{
	    comando.CommandText = "SELECT title, track, year, genre FROM rolas Where id_rola = @id";
	    comando.Parameters.AddWithValue("@id", id); 
	    using (var reader = comando.ExecuteReader()){
		if (reader.Read()){
		    var titulo = new Titulo
			{
			    
			    titulo = reader.IsDBNull(0) ? "" : reader.GetString(0),
			    track = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
			    year = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
			    genero = reader.IsDBNull(3) ? "" : reader.GetString(3)
			};
		    return titulo;
		}
	    }
	}
	return null;
    }
}
