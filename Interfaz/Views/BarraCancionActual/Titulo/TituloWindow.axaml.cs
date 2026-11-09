using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Interfaz.Models;
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
}
