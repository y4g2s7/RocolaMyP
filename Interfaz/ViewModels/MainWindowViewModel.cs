using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Interfaz.Models;
using System.Linq; // Asegúrate de tener este using arriba

namespace Interfaz.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    //Coleccion para guardar las canciones y mostrarlas
    public ObservableCollection<CancionFila> canciones { get; } = new();  

    //Constructor y agregamos dos canciones de prueba
    public MainWindowViewModel(){	
    }
    
    // Metodo que llama a ObtenerCanciones que nos da una lista de CancionFila
    // para agregarlo a canciones
    public void cargarCanciones(){
	canciones.Clear();
	foreach (var c in BaseDatos.ObtenerCanciones()){
	    canciones.Add(c);
	}
    }

    // Metodo que recibe la ruta de View y minamos llamando al metodo de la biblioteca
    // y agregamos a canciones
    public async Task MinarAsync(string ruta){
	if(!Directory.Exists(ruta)){
	    return; 
	}
	var minero = new Minero();
        await Task.Run(() => minero.minar(ruta));
	cargarCanciones();
    }

    // propiedad que comunica a la lista principal de canciones con la barra de cancion seleccionada
    [ObservableProperty]
    private CancionFila? _cancionSeleccionada;

    public Titulo? datosTitulo (CancionFila cf){
	int id = cf.id;
	return BaseDatos.getDatosTitulo(id);
    }

    public Artista? datosArtista(CancionFila cf){
	int id = cf.id;
	return BaseDatos.getDatosArtista(id);
    }

    public static void agregarPersona(Artista artistaActual, Artista artistaModificado){
	BaseDatos.eliminarBanda(artistaActual);
	BaseDatos.registrarArtista(artistaModificado);   
    }
    
    public static void agregarBanda(Artista artistaActual, Artista artistaModificado){
	BaseDatos.eliminarPersona(artistaActual);
	BaseDatos.registrarArtista(artistaModificado);
    }

    public static void agregarDesconocido(Artista artistaActual, Artista artistaModificado){
	BaseDatos.eliminarBanda(artistaActual);
	BaseDatos.eliminarPersona(artistaActual);
	BaseDatos.registrarArtista(artistaModificado);
    }

    public Album? datosAlbum(CancionFila cf){
	int id = cf.id;
	return BaseDatos.getDatosAlbum(id);
    }

    public static void agregarNuevaPersona(Artista artista){
        BaseDatos.registrarArtista(artista);   
    }
    public static void agregarPersonaGrupo(int id, string nombre){
	BaseDatos.AgregarPersonaAGrupo(id, nombre);
    }

    public static void eliminarPersonaGrupo(int id, string nombre){
	BaseDatos.EliminarPersonaDeGrupo(id, nombre);
    }

    public void ActualizarCancionEnLista(Titulo tituloEditado){
	//  Buscamos la posición de la canción en la lista
	var cancionOriginal = canciones.FirstOrDefault(c => c.id == tituloEditado.id_rola);

	if(cancionOriginal != null){
	    int index = canciones.IndexOf(cancionOriginal);
	    //Creamos una NUEVA instancia de CancionFila con los datos actualizados
	    var cancionNueva = new CancionFila
		{
		    id = cancionOriginal.id,
		    titulo = tituloEditado.titulo,
		    genero = tituloEditado.genero,
		    // Conservamos los datos que no se editan en esa ventana
		    performer = cancionOriginal.performer, 
		    album = cancionOriginal.album
		};
	    //Reemplazamos el objeto viejo por el nuevo en esa posición
	    canciones[index] = cancionNueva;
	}
        BaseDatos.ActualizarCancion(tituloEditado);
    }
}
