using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Interfaz.Models;

namespace Interfaz.Views.BotonesArriba;

public partial class RegistrarArtistaWindow : Window
{
    public RegistrarArtistaWindow()
    {
        InitializeComponent();
    }
    
    private async void ConvertirAPersonaClick(object? sender, RoutedEventArgs e){
	var artistaActual = DataContext as Artista;
	if (artistaActual == null) return;

	// Abrimos la ventana de persona
	var ventanaPersona = new RegistrarPersonaWindow();
	// Le pasamos los datos actuales
	ventanaPersona.DataContext = artistaActual; 

	// Esperamos a que el usuario guarde (true) o cancele (false)
	// Si guarda, nos devuelve el artista modificado
	var artistaModificado = await ventanaPersona.ShowDialog<Artista>(this);

	// Si es distinto de null, algo cambio
	if (artistaModificado != null){
	   // GuardarEnBaseDeDatos(artistaModificado);
           Close(); 
	}
    }
    
    private void ConvertirABandaClick(object? sender, RoutedEventArgs e) { }
    private void ConvertirADesconocidoClick(object? sender, RoutedEventArgs e) { }
    private void CerrarVentana(object? sender, Avalonia.Interactivity.RoutedEventArgs e){
        Close(); 
    }
}
