// See https://aka.ms/new-console-template for more information
using System;
using System.IO;

public class Minero{
    static void Main(string[] args){
	string carpeta = "/home/y4g2s7/Documents/ProyectosMyP/Musica";
	if(!Directory.Exists(carpeta)){
	    Console.WriteLine("La carpeta no existe");    
	    return; 
	}

	int contador= 0;
	
	foreach(string ruta in Directory.EnumerateFiles(carpeta, "*.mp3", SearchOption.AllDirectories)){
	    contador++;
	    Console.WriteLine($"[{contador}] {ruta}");
	}
	Console.WriteLine($"Se encontraron {contador} canciones");
    }
    
}

