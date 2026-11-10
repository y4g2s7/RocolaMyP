namespace Interfaz.Models;
public class Artista{
    public string seudonimo { get; set;} = "";
    public string nombreA { get; set;} = "";
    public string cumpleañosA { get; set;} = "";
    public string muerteA { get; set;} = "";
    public int tipo  { get; set;}
    public string inicioB { get; set;} = "";
    public string finB { get; set;} = "";
    public string TipoTexto{
        get{
            if(tipo == 0){
		return "Persona";
            } if(tipo== 1){
		return "Banda";
            }return "Desconocido";
        }
    }
    public int id{ get; set;}

    public bool PuedeSerPersona => tipo != 0;
    public bool PuedeSerBanda => tipo != 1;
    public bool PuedeSerDesconocido => tipo != 2;

}
