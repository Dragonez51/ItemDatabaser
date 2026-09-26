using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.DataStructures;

public partial class Item : ObservableObject
{
    private static int _itemId = 1;

    // [ObservableProperty] private ObservableCollection<Property>? _properties;
    public ObservableCollection<Property>? Properties  { get; private set; }
    // public string   Name    { get; private set; } = "New Item";
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
            var newProp = new Property(property.Name);
            newProp.SetType(property.ValueType.Name);

            Properties.Add(newProp);
        }
        return;
    }

    // TODO: this function binds every item properties to the same one (prob last)
    public void UpdateProperties(ObservableCollection<Property> updateProperties)
    {
        if(Properties is null) return;

        var lastProperties = new ObservableCollection<Property>(Properties);

        Properties = new ();
        foreach(var updateProp in updateProperties)
        {
            Property newProp = new Property();
            newProp.Name = updateProp.Name;
            newProp.SetType(updateProp.ValueType.Name);
            newProp.SetValue(updateProp.Value);

            Properties.Add(newProp);

            foreach(var lastProp in lastProperties)
            {
                if (!lastProp.Name.Equals(newProp.Name)) continue;
                
                newProp.SetValue(lastProp.Value);
                break;
            }
        }
    }

    public void AddProperty()
    {
        if(Properties is null) return;
        Property newProperty = new Property();
        
        Properties.Add(newProperty);
    }

    public void DeleteProperty(string propertyName)
    {
        if(Properties is null) return;
        foreach(var prop in Properties)
        {
            if(!prop.Name.Equals(propertyName)) continue;

            Properties.Remove(prop);
        }
    }
}