using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Interfaz.Models;
using Avalonia.Interactivity;


namespace Interfaz.Views.BarraCancionActual.Albums;

public partial class AlbumWindow : Window
{
    public AlbumWindow()
    {
        InitializeComponent();
    }

    public AlbumWindow(Album album) : this(){
	DataContext = album;
    }
    
    private void CerrarVentana(object? sender, Avalonia.Interactivity.RoutedEventArgs e){
        Close(); 
    }

    private void GuardarClick(object? sender, RoutedEventArgs e)
    {
	//Obtenemos el objeto Titulo que está en el DataContext
	var album = DataContext as Album;
	if (album == null) return;

        Close(album); 
    }
}
