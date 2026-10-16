using System.Collections.Generic;

// Libreria para buscar en un directorio
using System.IO;
// Libreria para leer la etiqueta Id3v2.4[1,2,3]
using TagLib;

public class MetaDatos{
    public static void procesar(string carpeta, List<string> mensajes){
	int contador= 0;
	
	foreach(string ruta in Directory.EnumerateFiles(carpeta, "*.mp3", SearchOption.AllDirectories)){
	    mensajes.Add(datos(ruta,ref contador));
	    
	}
        mensajes.Add($"Se encontraron {contador} canciones");
    }

    private static string datos(string ruta,ref int contador){
	try{
	    using(TagLib.File archivo = TagLib.File.Create(ruta)){
		var etiqueta = archivo.GetTag(TagTypes.Id3v2, false) as TagLib.Id3v2.Tag;
		if(etiqueta == null || etiqueta.Version!=4){
		    return $"ERROR {ruta}: sin ID3v2.4";    
		}
		string mensaje = ($"Título: {etiqueta.Title}\n");

		string genero = etiqueta.FirstGenre;
		if(string.IsNullOrWhiteSpace(genero)){
		    genero = "Unknown";
		}
	        mensaje +=($"Genero: {genero}\n");
		
		mensaje +=($"Performer: {etiqueta.FirstPerformer}\n");
		mensaje +=($"Álbum: {etiqueta.Album}\n");
		mensaje +=($"Track: {etiqueta.Track}\n");
		mensaje +=($"Año: {etiqueta.Year}\n");

		string? carpeta = Path.GetDirectoryName(ruta);
		mensaje +=($"Album path: {carpeta}\n");
		mensaje +=($"Rola path: {ruta}");
		contador ++;
		return mensaje;
	    }
	}
	catch{
	    return $"Error al intentar leer el mp3 {ruta}";
	    
	}
    }
}
