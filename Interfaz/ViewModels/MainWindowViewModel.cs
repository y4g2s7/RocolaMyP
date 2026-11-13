using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Interfaz.Models;
using System.Linq; 
using CommunityToolkit.Mvvm.Input;
using LibVLCSharp.Shared;
using System; 

namespace Interfaz.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    //Coleccion para guardar las canciones y mostrarlas
    public ObservableCollection<CancionFila> canciones { get; } = new();  
    
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
    
    public void agregarPersona(string nombreViejo, Artista artistaModificado){
	BaseDatos.eliminarBanda(nombreViejo);
	BaseDatos.registrarArtista(artistaModificado);   
	var cancionesAfectadas = canciones.Where(c => c.performer == nombreViejo).ToList();
    
	foreach(var cancionVieja in cancionesAfectadas){
	    int index = canciones.IndexOf(cancionVieja);
	    var cancionNueva = new CancionFila
		{
		    id = cancionVieja.id,
		    id_performer = artistaModificado.id,
		    id_album = cancionVieja.id_album,
		    path = cancionVieja.path, 
		    titulo = cancionVieja.titulo,
		    genero = cancionVieja.genero,
		    album = cancionVieja.album,
		    performer = artistaModificado.seudonimo 
		};
	    canciones[index] = cancionNueva;
	}
    }
    
    public void agregarBanda(string nombreViejo, Artista artistaModificado){
	BaseDatos.eliminarPersona(nombreViejo);
	BaseDatos.registrarArtista(artistaModificado);
	var cancionesAfectadas = canciones.Where(c => c.performer == nombreViejo).ToList();
    
	foreach(var cancionVieja in cancionesAfectadas){
	    int index = canciones.IndexOf(cancionVieja);
	    var cancionNueva = new CancionFila
		{
		    id = cancionVieja.id,
		    id_performer = artistaModificado.id,
		    id_album = cancionVieja.id_album,
		    path = cancionVieja.path,
		    titulo = cancionVieja.titulo,
		    genero = cancionVieja.genero,
		    album = cancionVieja.album,
		    performer = artistaModificado.seudonimo 
		};
	    canciones[index] = cancionNueva;
	}
    }

    public void agregarDesconocido(string artista, Artista artistaModificado){
	BaseDatos.eliminarBanda(artista);
	BaseDatos.eliminarPersona(artista);
	BaseDatos.registrarArtista(artistaModificado);
	
	var cancionesAfectadas = canciones.Where(c => c.performer == artista).ToList();
	
	foreach(var cancionVieja in cancionesAfectadas){
	    int index = canciones.IndexOf(cancionVieja);
	    var cancionNueva = new CancionFila
		{
		    id = cancionVieja.id,
		    id_performer = artistaModificado.id,
		    id_album = cancionVieja.id_album,
		    path = cancionVieja.path,
		    titulo = cancionVieja.titulo,
		    genero = cancionVieja.genero,
		    album = cancionVieja.album,
		    performer = artistaModificado.seudonimo 
	    };
	    canciones[index] = cancionNueva;
	}
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
		    path = cancionOriginal.path,
		    id = cancionOriginal.id,
		    titulo = tituloEditado.titulo,
		    genero = tituloEditado.genero,
		    // Conservamos los datos que no se editan en esa ventana
		    performer = cancionOriginal.performer, 
		    album = cancionOriginal.album,
		    id_performer = cancionOriginal.id_performer,
		    id_album = cancionOriginal.id_album
		    
		};
	    //Reemplazamos el objeto viejo por el nuevo en esa posición
	    canciones[index] = cancionNueva;
	}
        BaseDatos.ActualizarCancion(tituloEditado);
    }

    public void ActualizarArtista(Artista artistaEditado, int idViejoPerformer){
	BaseDatos.ActualizarPerformer(artistaEditado);
	// Obtenemos una lista de las canciones con ese artista
	var cancionesAfectadas = canciones.Where(c => c.id_performer == idViejoPerformer).ToList();

	// Las actualizamos una por una
	foreach(var cancionVieja in cancionesAfectadas){
	    int index = canciones.IndexOf(cancionVieja);

	    // Creamos una nueva instancia para forzar al DataGrid a redibujar
	    var cancionNueva = new CancionFila
		{
		    path = cancionVieja.path,        
		    id = cancionVieja.id,
		    id_performer = artistaEditado.id,
		    id_album = cancionVieja.id_album,
		    titulo = cancionVieja.titulo,
		    genero = cancionVieja.genero,
		    album = cancionVieja.album,
		    performer = artistaEditado.seudonimo 
		};

	    // Reemplazamos en la colección
	    canciones[index] = cancionNueva;
	}
    }

    public void ActualizarPersona(Artista artistaEditado, string nombreViejo, string realViejo){
	// Actualizamos en la base de datos
	BaseDatos.actualizarPersona(artistaEditado, nombreViejo, realViejo);
	
    }

    public void ActualizarBanda(Artista artistaEditado, string nombreViejo, string inicioViejo){
	// Actualizamos en la base de datos
	BaseDatos.actualizarBanda(artistaEditado, nombreViejo, inicioViejo);
	
    }

    public void ActualizarAlbum(Album albumEditado, int idViejoAlbum){
	//Mandamos a llamar a la base de datos
	BaseDatos.ActualizarAlbum(albumEditado, idViejoAlbum);
    
	//Buscamos las canciones que pertenecían a ese album viejo
	var cancionesAfectadas = canciones.Where(c => c.id_album == idViejoAlbum).ToList();

	//Las actualizamos una por una
	foreach(var cancionVieja in cancionesAfectadas)
	{
	    int index = canciones.IndexOf(cancionVieja);

	    var cancionNueva = new CancionFila
		{
		    path = cancionVieja.path,    
		    id = cancionVieja.id,
		    id_performer = cancionVieja.id_performer,
		    id_album = albumEditado.id_album,
		    titulo = cancionVieja.titulo,
		    genero = cancionVieja.genero,
		    performer = cancionVieja.performer,
		    album = albumEditado.nombre 
		};

	    canciones[index] = cancionNueva;
	}
    }
    //Variables para mostrar/ocultar el panel
    [ObservableProperty]
    private bool _mostrarPanelBusqueda = false;

    //Variables para los filtros
    [ObservableProperty] private string _buscarTitulo = "";
    [ObservableProperty] private string _buscarArtista = "";
    [ObservableProperty] private string _buscarAlbum = "";
    [ObservableProperty] private string _buscarGenero = "";

    //Comando para abrir/cerrar el panel
    [RelayCommand]
    private void ToggleBusqueda(){
	MostrarPanelBusqueda = !MostrarPanelBusqueda;
    }

    //El metodo para la busqueda
    [RelayCommand]
    private void EjecutarBusqueda(){
	var resultados = BaseDatos.BuscarCanciones(BuscarTitulo, BuscarArtista, BuscarAlbum, BuscarGenero);
	// Limpiamos la lista de canciones y metemos las que buscamos
	canciones.Clear();
	foreach(var cancion in resultados){
	    canciones.Add(cancion);
	}
    }

    //metodo para limpiar y mostrar todo de nuevo
    [RelayCommand]
    private void LimpiarBusqueda(){
	BuscarTitulo = "";
	BuscarArtista = "";
	BuscarAlbum = "";
	BuscarGenero = "";
	// Recargar todas las canciones
	var todas = BaseDatos.ObtenerCanciones();
	canciones.Clear();
	foreach(var cancion in todas){
	    canciones.Add(cancion);
	}
    }

// REPRODUCTOR
    private LibVLC _libVlc;
    private MediaPlayer _mediaPlayer;
    private Media? _mediaActual;

    public MainWindowViewModel()
    {
        // Inicializamos el motor
        _libVlc = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVlc);
        
        
    }

    // Comando para dar Play
    [RelayCommand]
    private void Reproducir()
    {
        if (CancionSeleccionada == null || string.IsNullOrWhiteSpace(CancionSeleccionada.path)) 
            return;

        string rutaArchivo = CancionSeleccionada.path;

        // Si es una canción diferente a la que estaba sonando
        if(_mediaActual == null || _mediaActual.Mrl != rutaArchivo){
            _mediaActual = new Media(_libVlc, new Uri(rutaArchivo));
            _mediaPlayer.Play(_mediaActual);
        }
        else{
            _mediaPlayer.Play(); 
        }
    }

    //Comando para Pausar
    [RelayCommand]
    private void Pausar(){
        _mediaPlayer.Pause();
    }
}
