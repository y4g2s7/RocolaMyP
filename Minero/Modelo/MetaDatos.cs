using System.Collections.Generic;
// Libreria para buscar en un directorio
using System.IO;
// Libreria para leer la etiqueta Id3v2.4[1,2,3]
using TagLib;

public class MetaDatos{
    // Metodo que busca los mp3 en el direcotorio, para guardarlos en una lista
    public static void procesar(string carpeta, List<Ficha> fichas ){
        // Buscamos recursivamente
	foreach(string ruta in Directory.EnumerateFiles(carpeta, "*.mp3", SearchOption.AllDirectories)){
	    // Creamos la Ficha con el metodo datos
	    Ficha? f = datos(ruta);
	    //Si es null es porque hubo un error y pudimos leer el mp3, asi que no lo agregamos a la lista
	    if(f != null) fichas.Add(f);
	   
	}
        
    }
    
    private static Ficha? datos(string ruta){
	try{
	    // Abrimos el mp3
	    using(TagLib.File archivo = TagLib.File.Create(ruta)){
		// Buscamos la etiqueta
		var etiqueta = archivo.GetTag(TagTypes.Id3v2, false) as TagLib.Id3v2.Tag;
		// Si el mp3 no tiene etiqueta o no es Id3v2.4 lo ignoramos
		if(etiqueta == null || etiqueta.Version!=4){
		    //Si no es especificamente id3v2.4 lo ignoramos
		    Console.WriteLine($"ERROR {ruta}: sin ID3v2.4\n");
		    return null;
		}

	        
		Ficha f = new Ficha();
		// Agregamos los parametros de la etiqueta de un mp3 a la Ficha
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
	    // Si hubo un error simplemente ignoramos el mp3
	    Console.WriteLine(e.Message);
	    Console.WriteLine($"Error al intentar leer el mp3 {ruta}\n");
	    return null;
	}
    }
    // Metodo para verificar si un parametro es null o vacio para rellenarlo como Unknown
    private static void desconocer(ref string parametro){
	if(string.IsNullOrWhiteSpace(parametro)){
	    parametro = "Unknown";
	}
    }
    
}
