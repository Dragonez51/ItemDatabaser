using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.DataStructures;

public partial class TreeNode : ObservableObject
{
    public ObservableCollection<TreeNode> SubNodes { get; private set; } = new();

    public ObservableCollection<Property> Properties { get; private set; } = new();

    [ObservableProperty] private string _name = "NewNode";

    public TreeNode(ObservableCollection<Property> properties)
    {
        CreateProperties(properties);
    }

    public TreeNode(string name, ObservableCollection<Property> properties)
    {
        Name = name;
        CreateProperties(properties);
    }

    public TreeNode(string name, ObservableCollection<TreeNode> nodes, ObservableCollection<Property> properties)
    {
        Name = name;
        SubNodes = nodes;
        CreateProperties(properties);
    }

    private void CreateProperties(ObservableCollection<Property> properties)
    {
        foreach(var property in properties)
        {
            var newProp = new Property()
            {
                Name = property.Name,
                Value = property.Value,
                ValueType = property.ValueType
            };
            Properties.Add(newProp);
            // Type propType = (property as Property).UntypedValueType;
            // Type genericDef = typeof(Property<>);
            // Type constructedType = genericDef.MakeGenericType(propType);
            // object newProp = Activator.CreateInstance(constructedType);
            // (newProp as Property).Name = (property as Property).Name;
            // (newProp as Property).SetValue((property as Property).UntypedValue);
            // Properties.Add(newProp);
        }
    }

    public void AddSubNode(TreeNode SubNode)
    {
        SubNodes.Add(SubNode);
    }
}