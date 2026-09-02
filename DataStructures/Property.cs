using System;
using System.Diagnostics;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.DataStructures;

// public abstract partial class Property : ObservableObject
public partial class Property : ObservableObject
{
    public string Name { get; set; } = "New Property";

    // public abstract object? UntypedValue { get; set; }
    // public abstract Type? UntypedValueType { get; set; }
    [ObservableProperty] private Type _valueType = typeof(Int32);

    public object? Value { get; set; }
    public Type? BindValueType { get => ValueType; set => SetType(value); }

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

    public void SetType(Type type)
    {
        if(type is null) return;
        ValueType = type;
    }

    public override string ToString()
    {
        return $"[Property][name: {Name}]";
    }
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