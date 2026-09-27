using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.DataStructures;

public partial class PropertyDefinition : ObservableObject
{
    public static int nextID = 0;

    public string Name { get; set; } = "New Property";
    public int PropertyID { get; init; }
    public int Column { get; set; } = 0;

    // NOTE: does it really need to be an ObservableProperty?
    [ObservableProperty] private Type _valueType = typeof(Int32);

    public string? BindValueType { get => ValueType.Name; set => SetType(value);}

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

    public PropertyDefinition(string name, int column, Type type)
    {
        Name = name;
        Column = column;
        ValueType = type;
        PropertyID = nextID++;
    }

    public void SetType(string? typeName)
    {
        var type = Type.GetType("System."+typeName);
        if(type is null) return;
        ValueType = type;
    }
}