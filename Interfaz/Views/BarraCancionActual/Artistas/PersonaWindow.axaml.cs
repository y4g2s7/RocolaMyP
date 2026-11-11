using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Interfaz.Views.BarraCancionActual.Artistas;

public partial class PersonaWindow : Window
{
    public PersonaWindow()
    {
        InitializeComponent();
    }

    private void CerrarVentana(object? sender, Avalonia.Interactivity.RoutedEventArgs e){
        Close(); 
    }
}
