using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.DataStructures;

public partial class Category : ObservableObject
{
    [ObservableProperty][property: JsonPropertyOrder(1)] private string _name = "New Category";
    [JsonPropertyName("Properties")][JsonPropertyOrder(2)] public ObservableCollection<PropertyDefinition> PropDefs { get; init; } = new ();
    [JsonPropertyOrder(3)] public ObservableCollection<Item> Items { get; init; } = new ();
    [JsonIgnore] public ObservableCollection<Category> SubCategories { get; private set; } = new ();

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

    public void SetType(Guid propID, string typeName)
    {
        foreach(var prop in PropDefs)
        {
            if(prop.PropertyID != propID) continue;

            prop.TrySetType(typeName);
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

    public void SetPropName(Guid propID, string newName)
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

    public void LinkProperties()
    {
        foreach(var item in Items)
        {
            foreach(var value in item.PropValues)
            {
                Boolean linked = false;
                foreach(var definition in PropDefs)
                {
                    if(value.PropertyID == definition.PropertyID)
                    {
                        value.PropertyDefinition = definition;
                        value.Validate();
                        linked = true;
                        break;
                    }
                }
                if (!linked)
                {
                    throw new Exception("[Category] LinkProperties() => import error! PropertyDefinition not found!");
                }
            }
        }
    }

    public bool Validate()
    {
        foreach(var item in Items)
        {
            if(!item.IsValid()) return false;
        }
        foreach(var subCategory in SubCategories)
        {
            if(!subCategory.Validate()) return false;
        }
        return true;
    }

    public override string ToString()
    {
        string propDefsString = "";
        foreach(var propDef in PropDefs)
        {
            propDefsString+="\n"+propDef.ToString();
        }

        string itemsString = "";
        if(Items.Count == 0) itemsString = "!empty!";
        foreach(var item in Items)
        {
            itemsString += "\n"+item.ToString();
        }

        return $"[Category: [Name: {Name}]\n[PropDefs: {propDefsString}]\n[Items: {itemsString}]]";
    }
}