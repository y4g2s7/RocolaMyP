using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Interfaz.ViewModels;
using Interfaz.Models;
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

    // Metodo que se activa cuando le dan click al boton de el nombre de la cancion
    // 	en la barra de la cancion actual
    private void TituloClick(object? sender, RoutedEventArgs e) {
	var vm = DataContext as MainWindowViewModel;
	// Obtenemos la cancion seleccionada que vive en MainWindowViewModel
	var cancion = vm?.CancionSeleccionada;
	if (cancion == null || vm == null) return;
	
	Titulo? informacion = vm.datosTitulo(cancion);
	if(informacion == null) return;
	
	new TituloWindow(informacion).ShowDialog(this); 
    }
    private void PerformerClick(object? sender, RoutedEventArgs e) { }
    private void AlbumClick(object? sender, RoutedEventArgs e) { }
    
}
