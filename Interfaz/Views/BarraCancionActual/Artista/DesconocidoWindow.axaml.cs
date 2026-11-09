using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Interfaz.Views.BarraCancionActual.Artista;

public partial class DesconocidoWindow : Window
{
    public DesconocidoWindow()
    {
        InitializeComponent();
    }

    private void CerrarVentana(object? sender, Avalonia.Interactivity.RoutedEventArgs e){
        Close(); 
    }
}
