using Avalonia;
using System;
using LibVLCSharp.Shared;

namespace Interfaz;

sealed class Program
{
    [STAThread]
    public static void Main(string[] args) 
    {
        Core.Initialize(); 

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
