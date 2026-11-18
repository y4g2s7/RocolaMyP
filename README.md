# Rocola MyP

Reproductor de música de escritorio para Linux, desarrollado en **C#** con **Avalonia UI**. Lee archivos MP3 con etiquetas ID3v2.4, guarda la información en una base de datos **SQLite** y reproduce el audio mediante **LibVLCSharp**.

## Estructura del proyecto

| Carpeta | Descripción |
|---|---|
| `Interfaz/` | Aplicación de escritorio (Avalonia UI) |
| `Minero.Lib/` | Librería independiente que lee los archivos MP3 y sus etiquetas |
| `Minero/` | Ejecutable de consola que usa `Minero.Lib` para poblar la base de datos sin abrir la interfaz |

## Requisitos previos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download) o superior.
- **LibVLCSharp, TagLibSharp y Microsoft.Data.Sqlite:** se instalan automáticamente mediante los paquetes NuGet al ejecutar `dotnet restore`.
- **VLC (sistema operativo):** la interfaz está diseñada para Linux, por lo que es indispensable tener instalado el motor de VLC para que la librería de reproducción de audio funcione.

En sistemas basados en Debian/Ubuntu:

```bash
sudo apt update
sudo apt install libvlc-dev vlc
```

## Ejecución

### Interfaz gráfica (Avalonia)

Desde la terminal:

```bash
cd RocolaMyP/Interfaz
dotnet restore
dotnet run
```

Se abrirá la ventana principal de la Rocola. La primera vez que presiones el botón **Minar**, la aplicación:

1. Crea automáticamente la base de datos SQLite en `~/.local/share/rocola/musica.db` (Linux).
2. Solicita la ruta de la carpeta de música.
3. Comienza a leer los archivos MP3 con etiquetas ID3v2.4.

### Módulo de minado (librería `Minero.Lib`)

Si deseas compilar o probar la librería de forma aislada:

```bash
cd RocolaMyP/Minero.Lib
dotnet build
```

### Minero (consola)

Ejecutable de consola que usa `Minero.Lib` para poblar la base de datos sin abrir la interfaz:

```bash
cd RocolaMyP/Minero
dotnet restore
dotnet run -- /ruta/a/la/carpeta/de/musica
```

Reemplaza `/ruta/a/la/carpeta/de/musica` por la ruta real de tu colección de MP3.

## Tecnologías

- C# / .NET 8
- Avalonia UI
- LibVLCSharp
- SQLite