using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using ItemDatabaser.DataStructures;

namespace ItemDatabaser.ViewModels;

public partial class StructureViewModel : ViewModelBase
{
    public ObservableCollection<Category> CategoryNodes { get; private set; } = new ();

    public StructureViewModel()
    {
        CreateTestStructure();
    }

// This creates a basic template for testing during development stage
    private void CreateTestStructure()
    {
        var weaponsProps = new ObservableCollection<PropertyDefinition>()
        {
            new PropertyDefinition("Damage", 1),
            new PropertyDefinition("Durability", 2),
            new PropertyDefinition("IsToxic", 3, typeof(bool)),
        };
        CategoryNodes.Add(new Category("Weapons", weaponsProps));

        var armorProps = new ObservableCollection<PropertyDefinition>()
        {
            new PropertyDefinition("Defense", 1),
            new PropertyDefinition("Durability", 2),
            new PropertyDefinition("IsHeavy", 3, typeof(bool)),
        };
        CategoryNodes.Add(new Category("Armor", armorProps));
    }

    public void TreeView_SelectionChanged(object? sender, SelectionChangedEventArgs args)
    {
        var tree = sender as TreeView;
        if(tree is null) return;
        var item = tree.SelectedItem;

        var category = item as Category;
        if(!(category is null)) MainWindowViewModel.LoadInspector(category);
    }

    [RelayCommand] public void AddCategory()
    {
        CategoryNodes.Add(new Category());
    }
}