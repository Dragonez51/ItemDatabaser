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
        var isValid = (DataContext as MainWindowViewModel)?.Structure.ValidateDatabase();
        if(isValid is null) return;
        if(!(bool)isValid)
        { 
            MainWindowViewModel.NotificationService.Show("Cannot export, database is not valid!", "error");
            return;
        }

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

        if(file is not null && file.Count == 1)
        {
            (DataContext as MainWindowViewModel)?.Import(file[0].Path);
        }
    }

    public void Database_Validate(object? sender, RoutedEventArgs args)
    {
        bool? isValid_null = (DataContext as MainWindowViewModel)?.Structure.ValidateDatabase();
        if(isValid_null is null) throw new System.Exception("[MainWindow] Database_Validate()=> isValid is null!");
        bool isValid = (bool)isValid_null;
        string message = "Database is " + (isValid ? "valid!" : "invalid!");
        string type = isValid ? "ok" : "error";
        MainWindowViewModel.NotificationService.Show(message, type);
    }
}