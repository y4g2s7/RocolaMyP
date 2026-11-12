using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Interfaz.Models;
using Avalonia.Interactivity;

namespace Interfaz.Views.BarraCancionActual.Title;

public partial class TituloWindow : Window
{
    public TituloWindow()
    {
        InitializeComponent();
    }
    
    public TituloWindow(Titulo info) : this(){
	DataContext = info;
    }
    
    private void CerrarVentana(object? sender, Avalonia.Interactivity.RoutedEventArgs e){
        Close(); 
    }

    private void GuardarClick(object? sender, RoutedEventArgs e)
    {
	//Obtenemos el objeto Titulo que está en el DataContext
	var cancion = DataContext as Titulo;
	if (cancion == null) return;

        Close(cancion); 
    }
}
