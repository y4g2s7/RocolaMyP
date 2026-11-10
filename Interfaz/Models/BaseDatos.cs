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
    
    public static Artista? getDatosArtista(int idRola){
	using var conexion = conectar();

	// Obtenemos el id_type y el nombre desde la tabla performers
	int tipoPerformer = 2; // 2 = Unknown por defecto
	string nombrePerformer = "";
	int idPerformer = 0;
	
	using (var comando = conexion.CreateCommand()){
	    comando.CommandText = @"
            SELECT p.id_performer, p.id_type, p.name 
            FROM rolas r
            JOIN performers p ON r.id_performer = p.id_performer
            WHERE r.id_rola = @id";
            
	    comando.Parameters.AddWithValue("@id", idRola);

	    using(var reader = comando.ExecuteReader()){
		if(reader.Read()){
		    idPerformer = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
		    tipoPerformer = reader.IsDBNull(0) ? 2 : reader.GetInt32(1);
		    nombrePerformer = reader.IsDBNull(1) ? "" : reader.GetString(2);
		}
		else{
		    // Si no se encontro
		    return null;
		}
	    }
	}

	//Creamos el objeto Artista base (esto aplica para todos los casos)
	var artista = new Artista{
	    id = idPerformer,
	    tipo = tipoPerformer,
	    //El nombre del performer siempre va al seudónimo (sea Unknown, Banda o Persona)
	    seudonimo = nombrePerformer 
	};

	// Dependiendo del tipo, buscamos datos extra por NOMBRE
	// ES PERSONA
	if (tipoPerformer == 0){
	    using(var comando = conexion.CreateCommand()){
		// Buscamos en persons usando el nombre del performer (asumiendo que se guardó en stage_name)
		comando.CommandText = "SELECT real_name, birth_date, death_date FROM persons WHERE stage_name = @name";
		comando.Parameters.AddWithValue("@name", nombrePerformer);

		using (var reader = comando.ExecuteReader()){
		    if (reader.Read()){
			artista.nombreA = reader.IsDBNull(0) ? "" : reader.GetString(0);
			artista.cumpleañosA = reader.IsDBNull(1) ? "" : reader.GetString(1);
			artista.muerteA = reader.IsDBNull(2) ? "" : reader.GetString(2);
		    }
		}
	    }
	}
	// ES BANDA
	else if(tipoPerformer == 1){
	    using(var comando = conexion.CreateCommand())
	    {
		// Buscamos en groups usando el nombre del performer
		comando.CommandText = "SELECT start_date, end_date FROM groups WHERE name = @name";
		comando.Parameters.AddWithValue("@name", nombrePerformer);

		using(var reader = comando.ExecuteReader()){
		    if(reader.Read()){
			artista.inicioB = reader.IsDBNull(0) ? "" : reader.GetString(0);
			artista.finB = reader.IsDBNull(1) ? "" : reader.GetString(1);
		    }
		}
	    }
	}
	// Si es tipo 2 (Unknown), el codigo no hace nada mas
	// Se queda solo con el seudonimo que ya le asignamos en el paso 2
	return artista;
    }

    public static void registrarArtista(Artista artista){
	using var conexion = conectar();
	using var comando = conexion.CreateCommand();
	
	// Actualizamos el nombre y el tipo en la tabla performers
	comando.CommandText = @"
        UPDATE performers 
        SET name = @nombre, id_type = @tipo 
        WHERE id_performer = @id";
        
	comando.Parameters.AddWithValue("@nombre", artista.seudonimo);
	comando.Parameters.AddWithValue("@tipo", artista.tipo);
	comando.Parameters.AddWithValue("@id", artista.id);
	
	comando.ExecuteNonQuery();

	// Si es persona actualizamos
	if(artista.tipo == 0){

	    using(var comandoVerifica = conexion.CreateCommand()){
		// Le ponemos el texto
		comandoVerifica.CommandText = "SELECT id_person FROM persons Where stage_name = @nombre";
		comandoVerifica.Parameters.AddWithValue("@nombre", artista.seudonimo);
		// Vemos que nos dice
		using(var lector = comandoVerifica.ExecuteReader()){
		    // Si nos dio algo es porque el performer ya existe, asi que terminamos
		    if (lector.Read()){
		        return;
		    }
		}
	    }
	    
	    using(var cmdInsert = conexion.CreateCommand()){
		cmdInsert.CommandText = "INSERT INTO persons (stage_name, real_name, birth_date, death_date) VALUES (@stage,@real, @birth, @death)";
		cmdInsert.Parameters.AddWithValue("@stage", artista.seudonimo);
		cmdInsert.Parameters.AddWithValue("@real", artista.nombreA);
		cmdInsert.Parameters.AddWithValue("@birth", artista.cumpleañosA);
		cmdInsert.Parameters.AddWithValue("@death", artista.muerteA);
		cmdInsert.ExecuteNonQuery();
	    }
	} else if(artista.tipo==1){
	    
	    using(var comandoVerifica = conexion.CreateCommand()){
		// Le ponemos el texto
		comandoVerifica.CommandText = "SELECT id_group FROM groups Where name = @nombre";
		comandoVerifica.Parameters.AddWithValue("@nombre", artista.seudonimo);
		// Vemos que nos dice
		using(var lector = comandoVerifica.ExecuteReader()){
		    // Si nos dio algo es porque el performer ya existe, asi que terminamos
		    if (lector.Read()){
		        return;
		    }
		}
	    }
	    
	    using(var cmdInsert = conexion.CreateCommand()){
		cmdInsert.CommandText = "INSERT INTO groups (name, start_date, end_date) VALUES (@name,@start, @end)";
		cmdInsert.Parameters.AddWithValue("@name", artista.seudonimo);
		cmdInsert.Parameters.AddWithValue("@start", artista.inicioB);
		cmdInsert.Parameters.AddWithValue("@end", artista.finB);
		cmdInsert.ExecuteNonQuery();
	    }
	}
    }

    
    public static void eliminarPersona(Artista artista){
	// Validación de seguridad: si no hay nombre, no podemos buscar a quién borrar
	if (string.IsNullOrWhiteSpace(artista.seudonimo)) return;
	
	using var conexion = conectar();
	using var comando = conexion.CreateCommand();
	
        comando.CommandText = @"
        DELETE FROM persons 
        WHERE stage_name = @nombre";
        
	comando.Parameters.AddWithValue("@nombre", artista.seudonimo);
        comando.ExecuteNonQuery();
    }

    public static void eliminarBanda(Artista artista){
	// Validación de seguridad: si no hay nombre, no podemos buscar a quién borrar
	if (string.IsNullOrWhiteSpace(artista.seudonimo)) return;
	
	using var conexion = conectar();
	using var comando = conexion.CreateCommand();
	
        comando.CommandText = @"
        DELETE FROM groups 
        WHERE name = @nombre";
        
	comando.Parameters.AddWithValue("@nombre", artista.seudonimo);
        comando.ExecuteNonQuery();
    }

    public static Album? getDatosAlbum(int id){
	using var conexion = conectar();
	
	using (var comando = conexion.CreateCommand()){
	    comando.CommandText = @"
            SELECT a.name, a.year 
            FROM rolas r
            JOIN albums a ON r.id_album = a.id_album
            WHERE r.id_rola = @id";
            
	    comando.Parameters.AddWithValue("@id", id);
	    
	    using (var reader = comando.ExecuteReader()){
            if(reader.Read()){
                var album = new Album
		    {
		        nombre = reader.IsDBNull(0) ? "" : reader.GetString(0),
			year = reader.IsDBNull(1) ? 0 : reader.GetInt32(1)
		    };
                return album;
            }
        }
    }
    return null;
    }
}
