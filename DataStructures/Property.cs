using System;
using System.Diagnostics;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.DataStructures;

public partial class Property : ObservableObject
{
    [ObservableProperty] private IBrush _validation = Brush.Parse("#0f0");

    public int Column { get; set; } = 1; 

    public string Name { get; set; } = "New Property";

    [ObservableProperty] private Type _valueType = typeof(Int32);
    [ObservableProperty] private object? _value;

    public object? BindValue { get => Value; set => SetValue(value); }
    public string? BindValueType { get => ValueType.Name; set => SetType(value); }

    public Property(){}

    public Property(string name)
    {
        Name = name;
    }

    public Property(string name, Type valueType, object value)
    {
        Name = name;
        ValueType = valueType;
        Value = value;
    }

    public void SetType(string? typeName)
    {
        var type = Type.GetType("System."+typeName);
        if(type is null) return;
        ValueType = type;
        Validate();
        // Debug.WriteLine($"[Property] SetType() => [Type: {ValueType}][typeName: {typeName}][Value: {Value}]");
    }

    public void SetValue(object? value)
    {
        Value = value;
        Validate();
    }

    public void Validate()
    {
        try
        {
            var convert = Convert.ChangeType(Value, ValueType);
            if(convert is null)   
            {
                Invalid();
                return;
            }
            Valid();
            return;
        }
        catch (Exception)
        {
            Invalid();
            return;
        }
    }

    private void Valid() => Validation = Brush.Parse("#0f0");
    private void Invalid() => Validation = Brush.Parse("#f00");

    public override string ToString()
    {
        return $"[Property][name: {Name}][type: {ValueType.Name}][value: {Value}]";
    }
}