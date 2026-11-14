using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace ItemDatabaser.DataStructures;

public class Item
{
    [JsonPropertyName("Values")][JsonPropertyOrder(3)]
    public ObservableCollection<PropertyValue> PropValues { get; private set; } = new ();
    [JsonPropertyOrder(1)]
    public string Name { get; set; } = "New Item";
    [JsonPropertyOrder(2)]
    public Guid ItemID { get; init; }
    
    public Item(ObservableCollection<PropertyDefinition> properties)
    {
        ItemID = Guid.NewGuid();
        SetProperties(properties);
    }

    public Item(string name, ObservableCollection<PropertyDefinition> properties)
    {
        Name = name;  
        ItemID = Guid.NewGuid();
        SetProperties(properties);
    }

    public void AddPropValue(PropertyDefinition propertyDefinition)
    {
        var newProp = new PropertyValue(propertyDefinition);

        PropValues.Add(newProp);
    }

    private void SetProperties(ObservableCollection<PropertyDefinition> props)
    {
        foreach(var prop in props)
        {
            PropValues.Add(new PropertyValue(prop));
        }
    }

    public void ValidateProperty(PropertyDefinition propDef)
    {
        foreach (var prop in PropValues)
        {
            if(prop.PropertyDefinition.PropertyID != propDef.PropertyID) continue;

            prop.Validate();
            break;
        }
    }

    public void DeleteProp(Guid propID)
    {
        if(PropValues is null) return;
        foreach(var prop in PropValues)
        {
            if(prop.PropertyDefinition.PropertyID != propID) continue;
            
            PropValues.Remove(prop);
            break;
        }
    }
}