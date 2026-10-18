using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class BaseDeDatos{
    public static void conectar(string ruta, List<Ficha> fichas){
	// Nos conectamos a la base de datos
	string conexionString = $"Data Source={ruta};Mode=ReadWrite;Foreign Keys=True";
	using var conexion = new SqliteConnection(conexionString);
	conexion.Open();
	//Comando prueba
        using var comando = conexion.CreateCommand();
	comando.CommandText = "SELECT id_type, description FROM types";
	
	using var lector = comando.ExecuteReader();
	
	while (lector.Read()){
	    int id = lector.GetInt32(0);
            string descripcion = lector.GetString(1);
            Console.WriteLine($"{id} - {descripcion}");
	}
	//
	
	foreach(Ficha f in fichas){
	    long idP = performer(conexion, f.performer,2);
	    long idA = album(conexion,f.apath, f.album, f.year);
	    
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
		    Console.WriteLine(lector.GetInt64(0));
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
		Console.WriteLine(lector2.GetInt64(0));
		return lector2.GetInt64(0);
	    }
	}
    }
    // Metodo para obtener el id del album, ya sea buscandolo o insertandolo a la base de datos
    private static long album(SqliteConnection conexion, string path, string name, uint year){
	// Creamos comando
	using(var comando = conexion.CreateCommand()){
	    // Le ponemos el texto
	    comando.CommandText = "SELECT id_album FROM albums Where name = @nombre";
	    comando.Parameters.AddWithValue("@nombre", name);
	    // Vemos que nos dice
	    using(var lector = comando.ExecuteReader()){
		// Si nos dio algo es porque el album ya existe, asi que terminamos
		if (lector.Read()){
		    Console.WriteLine(lector.GetInt64(0));
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
		Console.WriteLine(lector2.GetInt64(0));
		return lector2.GetInt64(0);
	    }
	}
    }
}
