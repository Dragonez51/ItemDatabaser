using System.Collections.ObjectModel;

namespace ItemDatabaser.DataStructures;

public class Item
{
    private static int _itemId = 0;

    public ObservableCollection<PropertyValue> PropValues { get; private set; } = new ();

    public string Name { get; set; } = "New Item";

    public int ItemID { get; private set; }
    
    public Item(ObservableCollection<PropertyDefinition> properties)
    {
        ItemID = _itemId++;
        SetProperties(properties);
    }

    public Item(string name, ObservableCollection<PropertyDefinition> properties)
    {
        Name = name;  
        ItemID = _itemId++;
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

    public void DeleteProp(int propID)
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