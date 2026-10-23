using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class BaseDeDatos{
    public static void conectar(string ruta, List<Ficha> fichas){
	// Nos conectamos a la base de datos
	string conexionString = $"Data Source={ruta};Mode=ReadWrite;Foreign Keys=True";
	using var conexion = new SqliteConnection(conexionString);
	conexion.Open();
        
	// Iteramos la lista para agregar los datos a la base
	foreach(Ficha f in fichas){
	    // Obtenemos los id
	    long idP = performer(conexion, f.performer,2);
	    long idA = album(conexion,f.apath, f.album, f.year);

	    rola(conexion,idP,idA,f);
	}
    }


    private static void rola(SqliteConnection conexion, long idP, long idA, Ficha f){

	using(var comando = conexion.CreateCommand()){
	    // Le ponemos el texto
	    comando.CommandText = "SELECT id_rola FROM rolas Where title = @titulo";
	    comando.Parameters.AddWithValue("@titulo", f.titulo);
	    // Vemos que nos dice
	    using(var lector = comando.ExecuteReader()){
		// Si nos dio algo es porque el performer ya existe, asi que terminamos
		if (lector.Read()){
		    return;
		}
	    }
	}
	
	// Agregamos a rolas
	using(var cmdInsert = conexion.CreateCommand()){
	    cmdInsert.CommandText = "INSERT INTO rolas (id_performer, id_album, path, title, track, year, genre) VALUES (@idP,@idA, @path, @title, @track, @year, @genre)";
	    cmdInsert.Parameters.AddWithValue("@idP", idP);
	    cmdInsert.Parameters.AddWithValue("@idA", idA);
	    cmdInsert.Parameters.AddWithValue("@path", f.rpath);
	    cmdInsert.Parameters.AddWithValue("@title", f.titulo);
	    cmdInsert.Parameters.AddWithValue("@track", f.track);
	    cmdInsert.Parameters.AddWithValue("@year", f.year);
	    cmdInsert.Parameters.AddWithValue("@genre", f.genero);
	    cmdInsert.ExecuteNonQuery();
	}
    }

    
    // Metodo para obtener el id del performer, ya sea buscandolo o insertandolo a la base de datos
    private static long performer(SqliteConnection conexion, string artista, int tipo){
	// Creamos comando
	using(var comando = conexion.CreateCommand()){
	    // Le ponemos el texto
	    comando.CommandText = "SELECT id_performer FROM performers Where name = @nombre";
	    comando.Parameters.AddWithValue("@nombre", artista);
	    // Vemos que nos dice
	    using(var lector = comando.ExecuteReader()){
		// Si nos dio algo es porque el performer ya existe, asi que terminamos
		if (lector.Read()){
		    return lector.GetInt64(0);
		}
	    }
	}
	// Como no lo encontramos lo insertamos
	using(var cmdInsert = conexion.CreateCommand()){
	    cmdInsert.CommandText = "INSERT INTO performers (id_type, name) VALUES (@tipo,@nombre)";
	    cmdInsert.Parameters.AddWithValue("@tipo", tipo);
	    cmdInsert.Parameters.AddWithValue("@nombre", artista);
	    cmdInsert.ExecuteNonQuery();
	}
	// Lo buscamos para regresar
	using(var cmdId = conexion.CreateCommand()){
	    cmdId.CommandText = "SELECT id_performer FROM performers Where name = @nombre";
	    cmdId.Parameters.AddWithValue("@nombre", artista);
	    using(var lector2 = cmdId.ExecuteReader()){
		lector2.Read();
	        return lector2.GetInt64(0);
	    }
	}
    }
    // Metodo para obtener el id del album, ya sea buscandolo o insertandolo a la base de datos
    private static long album(SqliteConnection conexion, string path, string name, uint year){
	// Creamos comando
	using(var comando = conexion.CreateCommand()){
	    // Le ponemos el texto
	    comando.CommandText = "SELECT id_album FROM albums WHERE path = @path AND name = @nombre";
	    comando.Parameters.AddWithValue("@nombre", name);
	    comando.Parameters.AddWithValue("@path", path);
	    
	    // Vemos que nos dice
	    using(var lector = comando.ExecuteReader()){
		// Si nos dio algo es porque el album ya existe, asi que terminamos
		if (lector.Read()){
		    return lector.GetInt64(0);
		}
	    }
	}
	// Como no lo encontramos lo insertamos
	using(var cmdInsert = conexion.CreateCommand()){
	    cmdInsert.CommandText = "INSERT INTO albums (path, name, year) VALUES (@path,@nombre,@year)";
	    cmdInsert.Parameters.AddWithValue("@path", path);
	    cmdInsert.Parameters.AddWithValue("@nombre", name);
	    cmdInsert.Parameters.AddWithValue("@year", year);
	    cmdInsert.ExecuteNonQuery();
	}
	// Lo buscamos para regresar
	using(var cmdId = conexion.CreateCommand()){
	    cmdId.CommandText = "SELECT id_album FROM albums Where name = @nombre";
	    cmdId.Parameters.AddWithValue("@nombre", name);
	    using(var lector2 = cmdId.ExecuteReader()){
		lector2.Read();
	        return lector2.GetInt64(0);
	    }
	}
    }
}
