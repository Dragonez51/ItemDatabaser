using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.DataStructures;

public partial class PropertyDefinition : ObservableObject
{
    public static int nextID = 0;

    public string Name { get; set; } = "New Property";
    public int PropertyID { get; init; }
    public int Column { get; set; } = 0;

    [ObservableProperty] private string? _valueType = "Int32";

    public PropertyDefinition(int column)
    {
        Column = column;
        PropertyID = nextID++;
    }

    public PropertyDefinition(string name, int column)
    {
        Name = name;
        Column = column;
        PropertyID = nextID++;
    }

    public PropertyDefinition(string name, int column, string type)
    {
        Name = name;
        Column = column;
        ValueType = type;
        PropertyID = nextID++;
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