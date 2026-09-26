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

    public void AddProperty() 
    { 
        Properties.Add(new Property());
        
        foreach(var item in Items)
        {
            item.AddProperty();
        } 
    }

    public void AddItem()
    {
        Items.Add(new Item(Properties));
    }

    public void UpdateTypes(string propName, string typeName)
    {
        // Debug.WriteLine($"[Category] UpdateTypes(propName: {propName}, typeName: {typeName}) =>");

        // set type for category properties
        foreach(var prop in Properties)
        {
            if(!prop.Name.Equals(propName)) continue;

            Debug.WriteLine("[Category] UpdateTypes()=> found the correct category prop!");

            prop.SetType(typeName);
            break;
        }

        // set type for each item property in category
        foreach(var item in Items)
        {
            if(item.Properties is null) return;
            foreach(var prop in item.Properties)
            {
                if (!prop.Name.Equals(propName)) continue;
                
                prop.SetType(typeName);
                break;
            }
        }
    }

    public void UpdateItemPropName(string propName, string newName)
    {
        foreach(var item in Items)
        {
            if(item.Properties is null) return;
            foreach(var prop in item.Properties)
            {
                if(!prop.Name.Equals(propName)) continue;

                prop.Name = newName;
                break;
            }
        }
    }
}