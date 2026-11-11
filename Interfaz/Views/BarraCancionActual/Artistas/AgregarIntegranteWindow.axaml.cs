using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Interfaz.Models;
using Avalonia.Interactivity;

namespace Interfaz.Views.BarraCancionActual.Artistas;


public partial class AgregarIntegranteWindow : Window
{
    public AgregarIntegranteWindow()
    {
        InitializeComponent();
    }

    // Metodo para devolver a la persona seleccionada
    private void AgregarClick(object? sender, RoutedEventArgs e){
	var seleccionado = ListaPersonas.SelectedItem as Integrante;
	if(seleccionado != null){
	    Close(seleccionado); 
	}
    }

    private void CancelarClick(object? sender, RoutedEventArgs e){
	Close(null);
    }
}
