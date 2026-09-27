using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using ItemDatabaser.DataStructures;

namespace ItemDatabaser.ViewModels;

public partial class StructureViewModel : ViewModelBase
{
    public ObservableCollection<Category> CategoryNodes { get; private set; } = new ();

    public StructureViewModel()
    {
        CreateCategoryItemStructure();
    }

    // This creates a basic template for testing during development stage
    private void CreateCategoryItemStructure()
    {
        var weaponsProperties = new ObservableCollection<Property>()
        {
            new Property("Damage", typeof(Int32), 1, 1),
            new Property("Durability", typeof(Int32), 10, 2),
        };

        CategoryNodes.Add(new Category("Weapons", weaponsProperties));

        var armorProperties = new ObservableCollection<Property>()
        {
            new Property("Defense", typeof(Int32), 1, 1),
            new Property("Durability", typeof(Int32), 10, 2),
        };

        CategoryNodes.Add(new Category("Armor", armorProperties));
    }

    public void TreeView_SelectionChanged(object? sender, SelectionChangedEventArgs args)
    {
        var tree = sender as TreeView;
        if(tree is null) return;
        var item = tree.SelectedItem;

        var category = item as Category;
        if(!(category is null)) MainWindowViewModel.LoadInspector(category);
    }
}