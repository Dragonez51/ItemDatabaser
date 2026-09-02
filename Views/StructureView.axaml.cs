using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Input;
using ItemDatabaser.ViewModels;

namespace ItemDatabaser.Views;

public partial class StructureView : UserControl
{
    public StructureView()
    {
        InitializeComponent();
    }

    private void TreeView_SelectionChanged(object? sender, SelectionChangedEventArgs args)
    {
        // Debug.WriteLine("[StructureView] TreeView_SelectionChanged() =>");
        (DataContext as StructureViewModel)?.TreeView_SelectionChanged(sender, args);
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        (DataContext as StructureViewModel)?.AddNode();
    }
}