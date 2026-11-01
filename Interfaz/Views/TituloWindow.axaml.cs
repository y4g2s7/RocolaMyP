using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Interfaz.Models;
namespace Interfaz.Views;

public partial class TituloWindow : Window
{
    public TituloWindow()
    {
        InitializeComponent();
    }
    
    public TituloWindow(Titulo info) : this(){
	DataContext = info;
    }
}
