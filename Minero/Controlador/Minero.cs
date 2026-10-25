// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;

public class Minero{
    static void Main(string[] args){
	// Verificamos que nos pasaron las rutas mendiante los argumentos 
	if(args.Length == 0){
	    Vista.imprime("Error: debes indicar el directorio a escanear.");
	    Vista.imprime("Uso: dotnet run -- <ruta_del_directorio>");
	    Vista.imprime("Ejemplo: dotnet run -- /home/gio/Musica");
	    return;
	}

        string carpeta = args[0];
        Minero m = new Minero();
	m.minar(carpeta);
    }

    public void minar(string carpeta){
	// Verficamos si la ruta del directorio existe
	if(!Directory.Exists(carpeta)){
	    Vista.imprime("ERROR La carpeta no existe");    
	    return; 
	}
	List<Ficha> fichas = new List<Ficha>();
	// Llamamos al metodo procesar para obtener los datos de los mp3, lo cuales los ponemos en un objeto Ficha
	MetaDatos.procesar(carpeta,fichas);
	int contador = 0;
	// Mandamos a imprimir los datos obtenidos
	foreach(Ficha f in fichas){
	    Vista.imprime(f.imprimir());
	    contador ++;
	    
	}
	Vista.imprime($"Se encontraron {contador} canciones");

	using var conexion = BaseDeDatos.conectar();
	BaseDeDatos.agregar(conexion, fichas);
    }
    
}

