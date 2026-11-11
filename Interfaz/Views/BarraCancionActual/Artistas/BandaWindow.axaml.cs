using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using System;
using Interfaz.ViewModels;
using Interfaz.Models;

namespace Interfaz.Views.BarraCancionActual.Artistas;

public partial class BandaWindow : Window
{
    public BandaWindow()
    {
        InitializeComponent();
    }

    private void CerrarVentana(object? sender, Avalonia.Interactivity.RoutedEventArgs e){
        Close(); 
    }

    private void verIntegrantesClick(object? sender, RoutedEventArgs e){
	var artista = DataContext as Artista;
	if (artista == null) return;
	
	//Creamos el ViewModel pasandole el nombre de la banda
	var vm = new IntegrantesWindowViewModel(artista.seudonimo);

	//Creamos la ventana
	var ventana = new IntegrantesWindow();
    
	//Le inyectamos el ViewModel como DataContext
	ventana.DataContext = vm; 
    
	ventana.ShowDialog(this);
    }
}
