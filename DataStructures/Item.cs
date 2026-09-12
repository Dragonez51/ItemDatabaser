using System.Collections.ObjectModel;

namespace ItemDatabaser.DataStructures;

public class Item
{
    public ObservableCollection<Property> Properties  { get; private set; }
    public string   Name    { get; private set; }

    public Item(string name, ObservableCollection<Property> properties)
    {
        Name = name;  
        Properties = properties;
    }

    public void SetValue(string name, object value)
    {
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
        var lastProperties = Properties;
        Properties = newProperties;

        foreach(Property lastProperty in lastProperties)
        {
            if(lastProperty is null) throw new System.Exception("[Item] SetProperties() => conversion is null!");
            if(lastProperty.Value is null) throw new System.Exception("[Item] SetProperties() => lastProperty.Value is null!");
            SetValue(lastProperty.Name, lastProperty.Value);
        }
    }
}