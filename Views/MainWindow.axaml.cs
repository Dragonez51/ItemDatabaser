using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ItemDatabaser.ViewModels;

namespace ItemDatabaser.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Export_Click(object? sender, RoutedEventArgs e)
    {
        var topLevel = GetTopLevel(this);

        if(topLevel is null) return;

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save file",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("JSON") { Patterns = new[] { "*.json" } }
            }
        });

        if(file is not null)
        {
            (DataContext as MainWindowViewModel)?.Export(file.Path);
        }
    }

    private async void Import_Click(object? sender, RoutedEventArgs args)
    {
        var topLevel = GetTopLevel(this);

        if(topLevel is null) return;

        var file = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Save file",
            FileTypeFilter = new[]
            {
                new FilePickerFileType("JSON") { Patterns = new[] { "*.json" } }
            },
            AllowMultiple = false
        });

        if(file is not null)
        {
            (DataContext as MainWindowViewModel)?.Import(file[0].Path);
        }
    }
}