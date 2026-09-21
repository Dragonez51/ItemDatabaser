using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.DataStructures;

public partial class Item : ObservableObject
{
    // [ObservableProperty] private ObservableCollection<Property>? _properties;
    public ObservableCollection<Property>? Properties  { get; private set; }
    // public string   Name    { get; private set; } = "New Item";
    [ObservableProperty] private string _name = "New Item";
    
    public Item(ObservableCollection<Property> properties)
    {
        SetProperties(properties);
    }

    public Item(string name, ObservableCollection<Property> properties)
    {
        Name = name;  
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
            Properties.Add(new Property(property.Name));
        }
        return;
    }

    // TODO: this function binds every item properties to the same one (prob last)
    public void UpdateProperties(ObservableCollection<Property> newProperties)
    {
        if(Properties is null) return;

        var lastProperties = new ObservableCollection<Property>(Properties);

        Properties = new ();
        foreach(var newProp in newProperties)
        {
            Properties.Add(newProp);

            foreach(var lastProp in lastProperties)
            {
                if (!lastProp.Name.Equals(newProp.Name)) continue;
                
                newProp.SetValue(lastProp.Value);
                break;
            }
        }
    }
}