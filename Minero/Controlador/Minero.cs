// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;

public class Minero{
    static void Main(string[] args){
	// Verificamos que nos pasaron las rutas mendiante los argumentos 
	if((args.Length == 0)||(args.Length == 1)){
	    Vista.imprime("Error: debes indicar el directorio a escanear y la ruta a la base de datos.");
	    Vista.imprime("Uso: dotnet run -- <ruta_del_directorio> <ruta_base_datos>");
	    Vista.imprime("Ejemplo: dotnet run -- /home/gio/Musica /home/gio/Documentos");
	    return;
	}

	// Verficamos si la ruta del directorio existe
	string carpeta = args[0];
	if(!Directory.Exists(carpeta)){
	    Vista.imprime("ERROR La carpeta no existe");    
	    return; 
	}

	// Verificamos si la base de datos existe
	string rutaBD = args[1];
	if(!File.Exists(rutaBD)){
	    Vista.imprime("ERROR La base de datos no existe");    
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

	BaseDeDatos.conectar(rutaBD, fichas);
    }
    
}

