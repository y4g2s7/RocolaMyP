using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity; 
using Interfaz.Models;
using Interfaz.ViewModels;

namespace Interfaz.Views.BarraCancionActual.Artistas;

public partial class IntegrantesWindow : Window
{
    public IntegrantesWindow()
    {
        InitializeComponent();
    }

    private async void agregarClick(object? sender, RoutedEventArgs e){
	// Creamos la ventana para agregar personas
	var ventanaSeleccion = new AgregarIntegranteWindow();
	// Llamamos un controlador, pera que la ventana pueda mostrar a las personas,
	// que el controlador obtiene comunicandose con el modelo y este con la base
	// datos
	ventanaSeleccion.DataContext = new AgregarIntegrantesWindowViewModel();
	// esperamos a que el usuario selccione la persona que quiere agregar
	var personaElegida = await ventanaSeleccion.ShowDialog<Integrante>(this);
	if(personaElegida != null){
	    // Obtenemos el nombre de la banda del ViewModel actual de esta ventana
	    var vm = DataContext as IntegrantesWindowViewModel;

	    if(vm != null){
	    string nombreBanda = vm.NombreBanda; 
	    MainWindowViewModel.agregarPersonaGrupo(personaElegida.id_persona, nombreBanda);
	    
	    //Actualizamos la lista visual para que aparezca de inmediato
	    vm.Integrantes.Add(personaElegida);
	    }
	}
	

    }

    private void eliminarClick(object? sender, RoutedEventArgs e){
	//Obtenemos el ViewModel y el integrante seleccionado en la tabla
	var vm = DataContext as IntegrantesWindowViewModel;
	var seleccionado = ListaIntegrantes.SelectedItem as Integrante;
	
	// Si no hay nadie seleccionado, no hacemos nada
	if (seleccionado == null || vm == null) return;

	// Borramos de la base de datos
        MainWindowViewModel.eliminarPersonaGrupo(seleccionado.id_persona, vm.NombreBanda);
	// Lo quitamos de la lista visual. 
	vm.Integrantes.Remove(seleccionado);
    }

    private void CerrarVentana(object? sender, Avalonia.Interactivity.RoutedEventArgs e){
        Close(); 
    }
}
