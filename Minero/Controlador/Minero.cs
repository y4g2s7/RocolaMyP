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
	
	List<Ficha> fichas = new List<Ficha>();
	MetaDatos.procesar(carpeta,fichas);
	int contador = 0;
	foreach(Ficha f in fichas){
	    if(f == null){
		Vista.imprime("ERROR al leer mp3\n");
	    }else{
		Vista.imprime(f.imprimir());
		contador ++;
	    }
	}
	Vista.imprime($"Se encontraron {contador} canciones");
    }
    
}

