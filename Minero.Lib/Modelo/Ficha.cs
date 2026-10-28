public class Ficha{
    public string titulo { get; set; } = "";
    public string genero { get; set; }  = "";
    public string performer { get; set; } = "";
    public string album { get; set; } = "";
    public uint track { get; set; }
    public uint year { get; set; } 
    public string apath { get; set; } = "";
    public string rpath { get; set; } = "";

    public string imprimir(){
	string mensaje= "";
	mensaje += ($"Título: {titulo}\n");
	mensaje +=($"Genero: {genero}\n");
	mensaje +=($"Performer: {performer}\n");
	mensaje +=($"Álbum: {album}\n");
	mensaje +=($"Track: {track}\n");
	mensaje +=($"Año: {year}\n");
	mensaje +=($"Album path: {apath}\n");
	mensaje +=($"Rola path: {rpath}\n");
        return mensaje;
    }
}
