using Avalonia.Controls;
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
        (DataContext as StructureViewModel)?.TreeView_SelectionChanged(sender, args);
    }

    private void AddCategory_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // (DataContext as StructureViewModel)?.AddCategory();
    }
}