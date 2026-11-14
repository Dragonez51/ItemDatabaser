using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using ItemDatabaser.DataStructures;

namespace ItemDatabaser.ViewModels;

public partial class StructureViewModel : ViewModelBase
{
    [JsonPropertyOrder(0)] public string FormatVersion { get => "0.2.0"; }
    [JsonPropertyOrder(1)] public string DatabaseName { get; private set; } = "TestDB";
    [JsonPropertyOrder(2)][JsonPropertyName("Categories")] public ObservableCollection<Category> CategoryNodes { get; private set; } = new ();

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
            new PropertyDefinition("IsToxic", 3, typeof(bool).Name),
        };
        CategoryNodes.Add(new Category("Weapons", weaponsProps));

        var armorProps = new ObservableCollection<PropertyDefinition>()
        {
            new PropertyDefinition("Defense", 1),
            new PropertyDefinition("Durability", 2),
            new PropertyDefinition("IsHeavy", 3, typeof(bool).Name),
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

    [RelayCommand][property: JsonIgnore] public void AddCategory()
    {
        CategoryNodes.Add(new Category());
    }
}