// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;

public class Minero{
    static void Main(string[] args){
	if (args.Length == 0){
	    Vista.imprime("Error: debes indicar la carpeta a escanear.");
	    Vista.imprime("Uso: dotnet run -- <ruta_de_la_carpeta>");
	    Vista.imprime("Ejemplo: dotnet run -- /home/ana/Musica");
	    return;
	}
	
	string carpeta = args[0];
	if(!Directory.Exists(carpeta)){
	    Vista.imprime("La carpeta no existe");    
	    return; 
	}

	List<string> mensajes = new List<string>();
	MetaDatos.procesar(carpeta,mensajes);

	foreach(string mensaje in mensajes){
	    Vista.imprime(mensaje);
	}
    }
    
}

