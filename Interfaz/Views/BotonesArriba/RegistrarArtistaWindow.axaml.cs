using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Interfaz.Models;
using Interfaz.ViewModels;
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
	if(artistaModificado != null){
	    MainWindowViewModel.agregarPersona(artistaActual, artistaModificado);
	    Close(); 
	}
    }
    
    private async void ConvertirABandaClick(object? sender, RoutedEventArgs e) {
	var artistaActual = DataContext as Artista;
	if (artistaActual == null) return;

	var ventanaBanda = new RegistrarBandaWindow();
	ventanaBanda.DataContext = artistaActual; 

	// Esperamos a que el usuario guarde (true) o cancele (false)
	// Si guarda, nos devuelve el artista modificado
	var artistaModificado = await ventanaBanda.ShowDialog<Artista>(this);

	// Si es distinto de null, algo cambio
	if(artistaModificado != null){
	    MainWindowViewModel.agregarBanda(artistaActual, artistaModificado);
	    // BaseDatos.eliminarPersona(artistaActual);
	    // BaseDatos.registrarArtista(artistaModificado);
           Close(); 
	}
	
    }
    
    private async void ConvertirADesconocidoClick(object? sender, RoutedEventArgs e) {
	var artistaActual = DataContext as Artista;
	if (artistaActual == null) return;

	var ventanaD = new RegistrarDesconocidoWindow();
	ventanaD.DataContext = artistaActual; 

	// Esperamos a que el usuario guarde (true) o cancele (false)
	// Si guarda, nos devuelve el artista modificado
	var artistaModificado = await ventanaD.ShowDialog<Artista>(this);

	// Si es distinto de null, algo cambio
	if (artistaModificado != null){
	    MainWindowViewModel.agregarDesconocido(artistaActual, artistaModificado);
	    // BaseDatos.eliminarBanda(artistaActual);
	    // BaseDatos.eliminarPersona(artistaActual);
	    // BaseDatos.registrarArtista(artistaModificado);
           Close(); 
	}
    }
    
    private void CerrarVentana(object? sender, Avalonia.Interactivity.RoutedEventArgs e){
        Close(); 
    }
}
