using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace Interfaz.Views;

public partial class MinarWindow : Window
{
    public MinarWindow()
    {
        InitializeComponent();
    }
    // Metodo que regresa la ruta escrita por el usuario al presionar el boton
    private void Minar_Click(object? sender, RoutedEventArgs e){
        Close(RutaBox.Text);
    }
}
