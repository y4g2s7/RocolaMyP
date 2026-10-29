using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Interfaz.ViewModels;

namespace Interfaz.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    
    //Metodo que se activa cuando le dan click al boton de minar
    private async void AbrirVentanaMinar(object? sender, RoutedEventArgs e){
	// Creamos la ventana para escribir la ruta
	var ventana = new MinarWindow();
	//Obetenemos lo que el usuario escribio
	string? ruta = await ventana.ShowDialog<string?>(this);
	//Verificamos que no es vacia 
	if (string.IsNullOrWhiteSpace(ruta))
        return;
	// le pasamor la ruta a ViewModel
	if(DataContext is MainWindowViewModel vm){
	    await vm.MinarAsync(ruta);
	}
    }

    private void TituloClick(object? sender, RoutedEventArgs e) { }
    private void PerformerClick(object? sender, RoutedEventArgs e) { }
    private void AlbumClick(object? sender, RoutedEventArgs e) { }
    
}
