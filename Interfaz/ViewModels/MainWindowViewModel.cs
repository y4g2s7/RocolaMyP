using System.IO;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
	foreach (var c in CancionesBD.ObtenerCanciones()){
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
    
    
}
