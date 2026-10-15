// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using TagLib;

public class Minero{
    static void Main(string[] args){
	string carpeta = "/home/y4g2s7/Documents/Musica";
	if(!Directory.Exists(carpeta)){
	    Console.WriteLine("La carpeta no existe");    
	    return; 
	}

	int contador= 0;
	
	foreach(string ruta in Directory.EnumerateFiles(carpeta, "*.mp3", SearchOption.AllDirectories)){
	    try{
		using(TagLib.File archivo = TagLib.File.Create(ruta)){
		    var etiqueta = archivo.GetTag(TagTypes.Id3v2, false) as TagLib.Id3v2.Tag;
		    if(etiqueta == null || etiqueta.Version!=4){
			Console.WriteLine($"ERROR {ruta}: sin ID3v2.4");
			continue;
		    }
		    Console.WriteLine($"Título: {archivo.Tag.Title}");
		    Console.WriteLine($"Artista: {archivo.Tag.FirstPerformer}");
		    Console.WriteLine($"Álbum: {archivo.Tag.Album}");
		    Console.WriteLine($"Duración: {archivo.Properties.Duration}");
		}
		contador ++;
	    }
	    catch(Exception ex){
		Console.WriteLine($"Error al intentar leer el mp3 {ruta}");
		Console.WriteLine($"Tipo: {ex.GetType().Name}");
		Console.WriteLine($"Mensaje: {ex.Message}");
	    }
	}
	Console.WriteLine($"Se encontraron {contador} canciones");
    }
    
}

