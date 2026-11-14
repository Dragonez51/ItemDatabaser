using System;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.DataStructures;

public partial class PropertyDefinition : ObservableObject
{

    public string Name { get; set; } = "New Property";
    [ObservableProperty] private string? _valueType = "Int32";
    public Guid PropertyID { get; init; }
    [JsonIgnore] public int Column { get; set; } = 0;


    public PropertyDefinition(int column)
    {
        Column = column;
        PropertyID = Guid.NewGuid();
    }

    public PropertyDefinition(string name, int column)
    {
        Name = name;
        Column = column;
        PropertyID = Guid.NewGuid();
    }

    public PropertyDefinition(string name, int column, string type)
    {
        Name = name;
        Column = column;
        ValueType = type;
        PropertyID = Guid.NewGuid();
    }

    public void TrySetType(string? typeName)
    {
        if(typeName is null) return;
        var type = Type.GetType(typeName);
        if(!(type is null)){ ValueType = typeName; return; } // this is support for custom types (other than System.Type)
        else
        {
            type = Type.GetType("System."+typeName);
            if(type is null) throw new Exception($"[PropertyDefinition] SetType() => type is null [typeName: {typeName}]");
            ValueType = typeName;
        }
    }
}