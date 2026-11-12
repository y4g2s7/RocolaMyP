using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Interfaz.Models;
namespace Interfaz.Views.BarraCancionActual.Artistas;

public partial class DesconocidoWindow : Window
{
    public DesconocidoWindow()
    {
        InitializeComponent();
    }

    private void CerrarVentana(object? sender, Avalonia.Interactivity.RoutedEventArgs e){
        Close(); 
    }

    private void GuardarClick(object? sender, RoutedEventArgs e){
	//Obtenemos el objeto Titulo que está en el DataContext
	var artista = DataContext as Artista;
	if (artista == null) return;
        Close(artista);
    }
}
