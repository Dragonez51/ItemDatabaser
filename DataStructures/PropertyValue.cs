using System;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.DataStructures;

public partial class PropertyValue : ObservableObject
{
    public PropertyDefinition PropertyDefinition { get; init; }
    
    private object? _value;
    public object? Value { get => _value; set {_value = value; Validate(); } }

    [ObservableProperty] private IBrush _validation = Brush.Parse("#f00");

    public PropertyValue(PropertyDefinition propertyDefinition)
    {
        PropertyDefinition = propertyDefinition;
    }

    public PropertyValue(PropertyDefinition propertyDefinition, object value)
    {
        PropertyDefinition = propertyDefinition;
        Value = value;
    }

    public void Validate()
    {
        try
        {
            var convert = Convert.ChangeType(Value, PropertyDefinition.ValueType);
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
}