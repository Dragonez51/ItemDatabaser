using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace ItemDatabaser.DataStructures;

public class Item
{
    [JsonPropertyOrder(0)] public string Name { get; set; } = "New Item";
    
    [JsonPropertyOrder(1)] public Guid? ItemID { get; init; }
    [JsonPropertyName("Values")] [JsonPropertyOrder(2)] public ObservableCollection<PropertyValue> PropValues { get; init; } = new ();

    public Item(){}

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

    public Item(string name, ObservableCollection<PropertyDefinition> properties, Guid itemID)
    {
        Name = name;  
        ItemID = itemID;
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
    
    //    Validate Property is for validating a whole column.
    public void ValidateProperty(PropertyDefinition propDef)
    {
        foreach (var prop in PropValues)
        {
            if(prop.PropertyDefinition is null) throw new Exception("[Item] ValidateProperty() => PropertyDefinition is null!");
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
            if(prop.PropertyDefinition is null) throw new Exception("[Item] ValidateProperty() => PropertyDefinition is null!");
            if(prop.PropertyDefinition.PropertyID != propID) continue;
            
            PropValues.Remove(prop);
            break;
        }
    }

    public override string ToString()
    {
        string propValuesString = "";
        foreach(var propValue in PropValues)
        {
            propValuesString+=propValue.ToString();
        }
        return $"[Item [Name: {Name}][PropValues: {propValuesString}][ItemID: {ItemID}]]";
    }
}