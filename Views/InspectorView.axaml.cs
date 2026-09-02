using System;
using System.Diagnostics;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using ItemDatabaser.ViewModels;

namespace ItemDatabaser.Views;

public partial class InspectorView : UserControl
{

    private static InspectorView _self;

    public InspectorView()
    {
        _self = this;
        InitializeComponent();
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        (DataContext as InspectorViewModel)?.AddProperty();
    }

    private void DeleteProperty(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        (DataContext as InspectorViewModel)?.DeleteProperty((sender as Button).Name);
    }
}