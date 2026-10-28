// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;

public class Mina{
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
	if(!Directory.Exists(carpeta)){
	    Vista.imprime("ERROR La carpeta no existe");    
	    return; 
	}
	m.minar(carpeta);
	Vista.imprime("MINADO LISTO");
    }
}

