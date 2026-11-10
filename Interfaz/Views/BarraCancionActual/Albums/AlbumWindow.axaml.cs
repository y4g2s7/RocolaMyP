using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Interfaz.Models;
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
}
