using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Interfaz.Models;

namespace Interfaz.Views.BotonesArriba;

public partial class AgregarPersonaWindow : Window
{
    public AgregarPersonaWindow()
    {
        InitializeComponent();
    }

    private void GuardarClick(object? sender, RoutedEventArgs e){
	//Obtenemos el Artista que nos pasaron
	var artista = DataContext as Artista;
	if(artista != null){
	    // Tipo a 0 para ser persona
	    artista.tipo = 0; 
            Close(artista); 
	}
    }
    
    private void CancelarClick(object? sender, RoutedEventArgs e){
	// Si cancela, devolvemos null para que la ventana anterior sepa que no hubo cambios
	Close(null); 
    }
}
