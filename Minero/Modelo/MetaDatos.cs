using System.Collections.Generic;

// Libreria para buscar en un directorio
using System.IO;
// Libreria para leer la etiqueta Id3v2.4[1,2,3]
using TagLib;

public class MetaDatos{
    public static void procesar(string carpeta, List<Ficha> fichas){
        
	foreach(string ruta in Directory.EnumerateFiles(carpeta, "*.mp3", SearchOption.AllDirectories)){
	    Ficha? f = datos(ruta);
	    if(f != null) fichas.Add(f);
	   
	}
        
    }
    
    private static Ficha? datos(string ruta){
	try{
	    using(TagLib.File archivo = TagLib.File.Create(ruta)){
		var etiqueta = archivo.GetTag(TagTypes.Id3v2, false) as TagLib.Id3v2.Tag;
		if(etiqueta == null || etiqueta.Version!=4){
		    Console.WriteLine($"ERROR {ruta}: sin ID3v2.4\n");
		    return null;
		}

	        
		Ficha f = new Ficha();
		
		string titulo = etiqueta.Title;
		desconocer(ref titulo);
		f.titulo = titulo;
		    
		string genero = etiqueta.FirstGenre;
	        desconocer(ref genero);
		f.genero = genero;
		
		string performer = etiqueta.FirstPerformer;
		desconocer(ref performer);
		f.performer = performer;
		
		string album = etiqueta.Album;
		string? carpeta = Path.GetDirectoryName(ruta);
		if(string.IsNullOrWhiteSpace(album)){
		    if(carpeta != null){
			album = new DirectoryInfo(carpeta).Name;
		    }
		}
		f.album = album;

		uint track = etiqueta.Track;
	        f.track = track;

		uint year = etiqueta.Year;
	        f.year = year;
		
	        if(carpeta != null) f.apath = carpeta;
		
	        f.rpath = ruta;
		return f;
	    }
	}
	catch(Exception e){
	    Console.WriteLine(e.Message);
	    Console.WriteLine($"Error al intentar leer el mp3 {ruta}\n");
	    return null;
	}
    }

    private static void desconocer(ref string parametro){
	if(string.IsNullOrWhiteSpace(parametro)){
	    parametro = "Unknown";
	}
    }
    
}
