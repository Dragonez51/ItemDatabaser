using System;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.DataStructures;

public partial class PropertyValue : ObservableObject
{
    [JsonIgnore] public PropertyDefinition? PropertyDefinition { get; set; }
    public Guid? PropertyID { get; init; }

    private object? _value;
    public object? Value { get => _value; set { _value = value; Validate(); } }

    [ObservableProperty][property: JsonIgnore] private IBrush _validation = Brush.Parse("#f00");

    [ObservableProperty][property: JsonIgnore] private bool _isValid = false;

    public PropertyValue(){}

    public PropertyValue(PropertyDefinition propertyDefinition)
    {
        PropertyDefinition = propertyDefinition;
        PropertyID = PropertyDefinition.PropertyID;
    }

    public PropertyValue(PropertyDefinition propertyDefinition, object value)
    {
        Value = value;
        PropertyDefinition = propertyDefinition;
        PropertyID = PropertyDefinition.PropertyID;
    }

    public bool Validate()
    {
        if(Value is null)
        {
            Invalid();
            return false;
        }

        try
        {
            if(PropertyDefinition?.ValueType is null) throw new Exception("[PropertyValue] Validate() => PropertyDefinition has a null ValueType!");
            var type = Type.GetType(PropertyDefinition.ValueType);
            if(type is null){ type = Type.GetType("System."+PropertyDefinition.ValueType); if(type is null) throw new Exception("[PropertyValue] Validate() => did not find type of '"+PropertyDefinition.ValueType+"'!"); }
            if(Value is JsonElement)
            { 
                Value = ((JsonElement)Value).GetString();
            }
            var convert = Convert.ChangeType(Value, type);
            if(convert is null)
            {
                Invalid();
                return false;
            }
            Valid();
            return true;
        }
        catch (Exception)
        {
            // Debug.WriteLine($"[PropertyValue] Validate() => Value.Type: {Value.GetType()}");
            Invalid();
            return false;
        }
    }

    private void Valid() { Validation = Brush.Parse("#0f0"); IsValid = true; }
    private void Invalid() { Validation = Brush.Parse("#f00"); IsValid = false; }

    public override string ToString()
    {
        return $"[PropertyValue [PropertyID: {PropertyID}][Value: {Value}]]";
    }
}