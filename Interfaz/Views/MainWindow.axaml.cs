using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Interfaz.ViewModels;
using Interfaz.Models;
using Interfaz.Views.BotonesArriba;
using Interfaz.Views.BarraCancionActual.Title;
using Interfaz.Views.BarraCancionActual.Albums;
using Interfaz.Views.BarraCancionActual.Artistas;

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
    private async void TituloClick(object? sender, RoutedEventArgs e) {
	var vm = DataContext as MainWindowViewModel;
	// Obtenemos la cancion seleccionada que vive en MainWindowViewModel
	var cancion = vm?.CancionSeleccionada;
	if (cancion == null || vm == null) return;

	// Obtenemos el titulo donde viene la informacion para mostrar
	Titulo? informacion = vm.datosTitulo(cancion);
	if(informacion == null) return;
	
	//Creamos la ventana
	var ventana = new TituloWindow(informacion);
    
	//Esperamos a que se cierre y guardamos la modificacion
	var tituloModificado = await ventana.ShowDialog<Titulo>(this); 
	
	// Si el usuario guardo actualizamos
	if (tituloModificado != null){
	    // Le mandamos al modelo para modificar
	    vm.ActualizarCancionEnLista(tituloModificado);
	}
    
    }
    
    private async void PerformerClick(object? sender, RoutedEventArgs e) {
	var vm = DataContext as MainWindowViewModel;
	// Obtenemos la cancion seleccionada que vive en MainWindowViewModel
	var cancion = vm?.CancionSeleccionada;
	if (cancion == null || vm == null) return;
	
	Artista? artista = vm.datosArtista(cancion);
	if(artista == null) return;
	// Dependiendo del tipo abrimos que ventana
	if(artista.tipo==0){
	    var ventana = new PersonaWindow();
            // Le pasamos los datos del artista a la ventana
            ventana.DataContext = artista; 
            ventana.ShowDialog(this);
	    
	} else if(artista.tipo==1){
	    var ventana = new BandaWindow();
            ventana.DataContext = artista;
            ventana.ShowDialog(this);
	    
	} else if(artista.tipo==2){
	    var ventana = new DesconocidoWindow();
            ventana.DataContext = artista;
            var artistaModificado = await ventana.ShowDialog<Artista>(this);
	    // Si el usuario guardo actualizamos
	    if (artistaModificado != null){
		// Le mandamos al modelo para modificar
		vm.ActualizarArtista(artistaModificado);
	    }
	}
	
    }
    
    private void AlbumClick(object? sender, RoutedEventArgs e) {
	var vm = DataContext as MainWindowViewModel;
	// Obtenemos la cancion seleccionada que vive en MainWindowViewModel
	var cancion = vm?.CancionSeleccionada;
	if (cancion == null || vm == null) return;
	
	Album? album = vm.datosAlbum(cancion);
	if(album == null) return;
	
	new AlbumWindow(album).ShowDialog(this); 
    
    }
    
    private void RegistrarArtistaClick(object? sender, RoutedEventArgs e){
	var vm = DataContext as MainWindowViewModel;
	// Obtenemos la cancion seleccionada que vive en MainWindowViewModel
	var cancion = vm?.CancionSeleccionada;
	if (cancion == null || vm == null) return;
	Artista? artista = vm.datosArtista(cancion);
	if(artista == null) return;
	var ventana = new RegistrarArtistaWindow();
	ventana.DataContext = artista; 
	ventana.ShowDialog(this);
    } 

    private async void AgregarPersonaClick(object? sender, RoutedEventArgs e){
	var ventanaNuevaPersona = new AgregarPersonaWindow();
	ventanaNuevaPersona.DataContext = new Artista();
	var artistaNuevo = await ventanaNuevaPersona.ShowDialog<Artista>(this);
	if(artistaNuevo != null){
	    MainWindowViewModel.agregarNuevaPersona(artistaNuevo); 
	}
    }
}
