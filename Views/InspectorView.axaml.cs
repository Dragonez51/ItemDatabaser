using System.Diagnostics;
using Avalonia.Controls;
using ItemDatabaser.ViewModels;

namespace ItemDatabaser.Views;

public partial class InspectorView : UserControl
{
    public InspectorView()
    {
        InitializeComponent();
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        (DataContext as InspectorViewModel)?.AddProperty();
    }

    private void DeleteProperty(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var senderButton = sender as Button;
        if(senderButton is null) return; 
        if(senderButton.Name is null) return;
        (DataContext as InspectorViewModel)?.DeleteProperty(senderButton.Name);
    }
}