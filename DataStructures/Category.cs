using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.DataStructures;

public partial class Category : ObservableObject
{
    public ObservableCollection<PropertyDefinition> PropDefs { get; private set; } = new ();
    public ObservableCollection<Item>     Items         { get; private set; } = new ();

    public ObservableCollection<Category> SubCategories { get; private set; } = new ();

    [ObservableProperty] private string _name = "New Category";

    public Category(){}

    public Category(string name)
    {
        Name = name;
    }

    public Category(string name, ObservableCollection<PropertyDefinition> properties)
    {
        Name = name;
        PropDefs = properties;
    }

    public Category(string name, ObservableCollection<PropertyDefinition> properties, ObservableCollection<Item> items)
    {
        Name = name;
        PropDefs = properties;
        Items = items;
    }

    public void AddPropDef()
    {
        int column = PropDefs.Count + 1;

        var newProp = new PropertyDefinition(column);
        PropDefs.Add(newProp);

        foreach(var item in Items)
        {
            item.AddPropValue(newProp);
        }
    }

    public void SetType(int propID, string typeName)
    {
        foreach(var prop in PropDefs)
        {
            if(prop.PropertyID != propID) continue;

            prop.SetType(typeName);
            ValidateProperty(prop);
            break;
        }
    }

    private void ValidateProperty(PropertyDefinition propDef)
    {
        foreach(var item in Items)
        {
            item.ValidateProperty(propDef);
        }
    }

    public void SetPropName(int propID, string newName)
    {
        foreach(var prop in PropDefs)
        {
            if(prop.PropertyID != propID) continue;

            prop.Name = newName;
            break;
        }
    }

    public void AddItem()
    {
        Items.Add(new Item(PropDefs));
    }

    public void DeleteProperty(int column)
    {
        PropertyDefinition? delProp = null;
        foreach(var prop in PropDefs)
        {
            if(prop.Column > column){ prop.Column--; continue; }
            if(prop.Column != column) continue;

            delProp = prop;
        }
        if(delProp is null) return;
        PropDefs.Remove(delProp);

        foreach(var item in Items)
        {
            item.DeleteProp(delProp.PropertyID);
        }
    }

    public void MoveProp(string side, int column)
    {
        int secondColumn = 0;
        if(side.Equals("Left")) secondColumn = -1;
        if(side.Equals("Right")) secondColumn = 1;

        PropertyDefinition? first = null;
        PropertyDefinition? second = null;
        bool firstFound = false;
        bool secondFound = false;

        foreach(var prop in PropDefs)
        {
            if(firstFound && secondFound) break;

            if(prop.Column == column) { first = prop; firstFound = true; continue; }
            if(prop.Column == column+secondColumn) { second = prop; secondFound = true; continue; }
        }
        if(first is null || second is null) return;

        first.Column+=secondColumn;
        second.Column-=secondColumn;
    }
}