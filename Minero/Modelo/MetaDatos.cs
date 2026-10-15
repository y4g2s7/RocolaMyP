using System.Collections.Generic;
using System.IO;
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
		    string mensaje = ($"Título: {archivo.Tag.Title}\n");
		    mensaje +=($"Artista: {archivo.Tag.FirstPerformer}\n");
		    mensaje +=($"Álbum: {archivo.Tag.Album}\n");
		    mensaje +=($"Duración: {archivo.Properties.Duration}");
		    contador ++;
		    return mensaje;
		}
	    }
	    catch{
	        return $"Error al intentar leer el mp3 {ruta}";
		
	    }
    }
}
