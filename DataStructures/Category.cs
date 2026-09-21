using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.DataStructures;

public partial class Category : ObservableObject
{
    public ObservableCollection<Property> Properties { get; private set; } = new ();
    public ObservableCollection<Item>   Items      { get; private set; } = new ();

    public ObservableCollection<Category> SubCategories { get; private set; } = new ();

    [ObservableProperty] private string _name = "New Category";

    public Category(string name)
    {
        Name = name;
    }

    public Category(string name, ObservableCollection<Property> properties)
    {
        Name = name;
        Properties = properties;
    }

    public Category(string name, ObservableCollection<Property> properties, ObservableCollection<Item> items)
    {
        Name = name;
        Properties = properties;
        Items = items;
    }

    public void AddProperty() { Properties.Add(new Property()); UpdateItems(); }
    public void AddItem()
    {
        Items.Add(new Item(Properties));
    }
    public void UpdateItems()
    {
        foreach(var item in Items) item.UpdateProperties(Properties);
    }
}