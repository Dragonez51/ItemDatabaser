using System;
using System.Diagnostics;
using System.Xml.Linq;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.DataStructures;

// public abstract partial class Property : ObservableObject
public partial class Property : ObservableObject
{
    [ObservableProperty] private IBrush _typeValidation = Brush.Parse("#0f0");
    [ObservableProperty] private IBrush _valueValidation = Brush.Parse("#0f0");

    // public IBrush TypeValidation { get; set; } = Brush.Parse("#0f0");
    // public IBrush ValueValidation { get; set; } = Brush.Parse("#0f0");

    public string Name { get; set; } = "New Property";

    // public abstract object? UntypedValue { get; set; }
    // public abstract Type? UntypedValueType { get; set; }
    [ObservableProperty] private Type _valueType = typeof(Int32);
    [ObservableProperty] private object _value;

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

    // public abstract void SetValue(object value);
    // public abstract void SetType(Type type);

    public void SetType(string typeName)
    {
        var type = Type.GetType("System."+typeName);
        if(type is null) { InValidateType(); return; }
        ValidateType();
        ValueType = type;

        // if(Value as ValueType is null){ InValidateValue(); return;}
    }

    public void SetValue(object value)
    {
        var convert = Convert.ChangeType(value, ValueType);
        // Debug.WriteLine($"[Property] SetValue(value: {value}) => [valueType: {value.GetType()}][ValueType: {ValueType}][value as ValueType: {value as ValueType}]");
        if(convert is null){ InValidateValue(); return; }
        ValidateValue();
        Value = value;
    }

    public override string ToString()
    {
        return $"[Property][name: {Name}]";
    }

    private void ValidateType() => TypeValidation = Brush.Parse("#0f0");
    private void InValidateType() => TypeValidation = Brush.Parse("#f00");
    private void ValidateValue() => ValueValidation = Brush.Parse("#0f0");
    private void InValidateValue() => ValueValidation = Brush.Parse("#f00");
}

// public partial class Property<T> : Property
// {

//     [ObservableProperty] public T?    _value;
//     [ObservableProperty] public Type? _valueType = typeof(System.Int32);

//     public override object? UntypedValue { get => Value; set => SetValue(value); }
//     public override Type? UntypedValueType { get => ValueType; set => SetType(value); }

//     public Property()
//     {
//         Name = "New Property";
//     }

//     public Property(string name, T value) : base(name)
//     {
//         Value = value;
//     }

//     public override void SetValue(object value)
//     {
//         try
//         {
//             Debug.WriteLine($"[Property] SetValue(value: {value}) => Trying to convert value to type: {UntypedValueType}");
//             var convertValue = Convert.ChangeType(value, UntypedValueType);
//             if(convertValue is T input)
//             {
//                 Value = input;
//                 Debug.WriteLine("[Property] SetValue() => [Success] Input correct!");
//                 return;
//             }
//         }
//         catch (Exception)
//         {
//             Debug.WriteLine("[Property] SetValue() => [Error] Input incorrect...");
//             return;
//         }

//         // throw new System.ArgumentException($"Unable to set value of type: {value.GetType().Name} to value of type: {typeof(T).Name}!");
//     }

//     public override void SetType(Type type)
//     {
//         Debug.Write($"[Property] SetType(type: {type}) => before: [ValueType: {ValueType}] ");
//         ValueType = type;
//         Debug.WriteLine($"after: [ValueType: {ValueType}]");
//     }

//     public override string ToString()
//     {
//         return $"[Property][name: {Name}][T: {typeof(T)}][Value: {Value}]";
//     }
// }