using System.Collections.ObjectModel;
namespace Interfaz.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<CancionFila> canciones { get; } = new();  
    
    public MainWindowViewModel(){
	canciones.Add(new CancionFila {
		titulo = "Mi sangre prisionera",
		genero = "Norteño",
		performer = "Los Tigres del Norte",
		album = "Las Grandes Norteñas"
	    });
	
	canciones.Add(new CancionFila {
		titulo = "Que tal si eres tu",
		genero = "Norteño",
		performer = "Los Tigres del Norte",
		album = "Realidades"
	    });
    }
    

   public string Greeting { get; } = "Welcome to Avalonia!";
}
