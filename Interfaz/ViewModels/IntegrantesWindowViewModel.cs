using CommunityToolkit.Mvvm.ComponentModel;
using Interfaz.Models;
using System.Collections.ObjectModel;

namespace Interfaz.ViewModels;

public partial class IntegrantesWindowViewModel : ViewModelBase
{
    // Coleccion para mostrar los integrantes de una banda
    public ObservableCollection<Integrante> Integrantes { get; } = new();

    public string NombreBanda { get; set;}
    
    public IntegrantesWindowViewModel(string nombreBanda){
	NombreBanda = nombreBanda;
	// le pedimos a la base de datos una lista de los integrantes
        var listaBD = BaseDatos.ObtenerIntegrantes(nombreBanda);
        // los agregamos a nuestra coleccion
        foreach (var integrante in listaBD){
            Integrantes.Add(integrante);
        }
    }
}
