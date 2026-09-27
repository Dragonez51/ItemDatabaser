using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.DataStructures;

public partial class Category : ObservableObject
{
    public ObservableCollection<Property> Properties    { get; private set; } = new ();
    public ObservableCollection<Item>     Items         { get; private set; } = new ();

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
        int column = Properties.Count+1;

        Properties.Add(new Property(column));
        
        foreach(var item in Items)
        {
            item.AddProperty(column);
        } 
    }

    public void AddItem()
    {
        Items.Add(new Item(Properties));
    }

    public void UpdateTypes(string propName, string typeName)
    {
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

    public void DeleteProperty(int column)
    {
        Property? delProp = null;
        foreach(var prop in Properties)
        {
            if(prop.Column > column){ prop.Column--; continue; }
            if(prop.Column != column) continue;

            delProp = prop;
        }
        if(delProp is null) return;
        Properties.Remove(delProp);

        foreach(var item in Items)
        {
            item.DeleteProperty(column);
        }
    }

    public void MoveProp(string side, int column)
    {
        int secondColumn = 0;
        if(side.Equals("Left")) secondColumn = -1;
        if(side.Equals("Right")) secondColumn = 1;

        Property? first = null;
        bool firstFound = false;
        bool secondFound = false;
        Property? second = null;
        foreach(var prop in Properties)
        {
            if(firstFound && secondFound) break;

            if(prop.Column == column) { first = prop; firstFound = true; continue; }
            if(prop.Column == column+secondColumn) { second = prop; secondFound = true; continue; }
        }
        if(first is null || second is null) return;

        first.Column+=secondColumn;
        second.Column-=secondColumn;

        foreach(var item in Items)
        {
            item.SetPropColumn(first.Name, first.Column);
            item.SetPropColumn(second.Name, second.Column);
        }
    }
}