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

    private void AddItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => (DataContext as InspectorViewModel)?.AddItem();
}