using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ItemDatabaser.DataStructures;

namespace ItemDatabaser.ViewModels;

public partial class StructureViewModel : ViewModelBase
{
    // TODO:
    //  1. Make a Database class from this file.

    [JsonPropertyOrder(0)] public string FormatVersion { get; init; } = "0.3.1";
    [ObservableProperty][property: JsonPropertyOrder(1)] private string _databaseName = "New Database";
    [JsonPropertyOrder(2)][JsonPropertyName("Categories")] public ObservableCollection<Category> CategoryNodes { get; init; } = new ();

    public StructureViewModel(){}

    public StructureViewModel(bool initTestDB)
    {
        if(initTestDB) CreateTestStructure();
    }

    public StructureViewModel(string formatVersion, string databaseName, ObservableCollection<Category> categories)
    {
        Debug.WriteLine("[StructureViewModel] (formatVersion, databaseName, categories)");
        FormatVersion = formatVersion;
        DatabaseName = databaseName;
        CategoryNodes = categories;
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

    public void LinkProperties()
    {
        foreach(var category in CategoryNodes)
        {
            category.LinkProperties();
        }
    }

    public bool ValidateDatabase()
    {
        foreach(var category in CategoryNodes)
        {
            if(!category.Validate()) return false;
        }

        return true;
    }

    public override string ToString()
    {
        string categoriesString = "";
        foreach(var category in CategoryNodes)
        {
            categoriesString+="\n"+category;
        }
        return $"[StructureViewModel [FormatVersion: {FormatVersion}][DatabaseName: {DatabaseName}]\n[Categories: {categoriesString}]]";
    }
}