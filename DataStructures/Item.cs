using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.DataStructures;

public partial class Item : ObservableObject
{
    private static int _itemId = 1;

    public ObservableCollection<Property>? Properties  { get; private set; }
    [ObservableProperty] private string _name = "New Item";

    public int ItemID { get; private set; }
    
    public Item(ObservableCollection<Property> properties)
    {
        ItemID = _itemId++;
        SetProperties(properties);
    }

    public Item(string name, ObservableCollection<Property> properties)
    {
        Name = name;  
        ItemID = _itemId++;
        SetProperties(properties);
    }

    public void SetValue(string name, object value)
    {
        if(Properties is null) return;

        foreach(Property property in Properties)
        {
            if(property is null) throw new System.Exception("[Item] SetValues() => conversion is null!");
            if (property.Name.Equals(name))
            {
                property.Value = value;
                return;
            }
        }
    }

    public void SetProperties(ObservableCollection<Property> newProperties)
    {
        if(!(Properties is null)) return;
        
        Properties = new ();
        foreach(var property in newProperties)
        {
            var newProp = new Property(property.Name, property.Column);
            newProp.SetType(property.ValueType.Name);

            Properties.Add(newProp);
        }
        return;
    }

    public void AddProperty(int column)
    {
        if(Properties is null) return;
        Property newProperty = new Property(column);
        
        Properties.Add(newProperty);
    }

    public void DeleteProperty(int column)
    {
        if(Properties is null) return;

        Property? delProp = null;
        foreach(var prop in Properties)
        {
            if(prop.Column > column) { prop.Column--; continue; }
            if(prop.Column != column) continue;

            delProp = prop;
        }
        if(delProp is null) return;
        Properties.Remove(delProp);
    }

    public void SetPropColumn(string propName, int column)
    {
        if(Properties is null) return;
        foreach(var prop in Properties)
        {
            if(prop.Name.Equals(propName))
            { 
                prop.Column = column;
                break;
            }
        }
    }
}