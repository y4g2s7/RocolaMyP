using CommunityToolkit.Mvvm.ComponentModel;
using Interfaz.Models;
using System.Collections.ObjectModel;

namespace Interfaz.ViewModels;

public partial class AgregarIntegrantesWindowViewModel : ViewModelBase
{
    // Coleccion para mostrar todas las personas
    public ObservableCollection<Integrante> Personas { get; } = new();

    public AgregarIntegrantesWindowViewModel(){
	// le pedimos a la base de datos una lista de los integrantes
        var listaBD = BaseDatos.ObtenerPersonas();
        // los agregamos a nuestra coleccion
        foreach (var persona in listaBD){
            Personas.Add(persona);
        }
    }
}
