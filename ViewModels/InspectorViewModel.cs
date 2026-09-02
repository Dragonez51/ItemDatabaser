using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ItemDatabaser.DataStructures;
using ItemDatabaser.Views;

namespace ItemDatabaser.ViewModels;

public partial class InspectorViewModel : ViewModelBase
{
    private static InspectorViewModel? _self;

    [ObservableProperty] private bool _isTextVisible = true;
    [ObservableProperty] private bool _isPropertiesVisible = false;

    [ObservableProperty] private List<Type>? _types;

    [ObservableProperty] private TreeNode? _loadedTreeNode;

    public InspectorViewModel()
    {
        _self = this;
        LoadTypes();
    }

    private void LoadTypes()
    {
        var frameworkTypes = typeof(Type).Assembly.GetTypes().Where(x => x.IsPrimitive).ToList();
        frameworkTypes.Add(typeof(String));
        Types = frameworkTypes;
    }

    public void Load(TreeNode treeNode)
    {
        LoadedTreeNode = treeNode;
        IsTextVisible = false;
        IsPropertiesVisible = true;
    }

    public void AddProperty()
    {
        LoadedTreeNode?.Properties.Add(new Property());
    }

    public void DeleteProperty(string name)
    {
        foreach(var property in LoadedTreeNode?.Properties)
        {
            if(property.Name.Equals(name))
            {
                LoadedTreeNode.Properties.Remove(property);
                return;
            }
        }
    }

    public static void ChangeProperty(string name, object value)
    {
        Debug.Write($"[InspectorViewModel] ChangeProperty ([name: {name}][value: {value}]) => ");

        if(_self?.LoadedTreeNode is null) return;

        var properties = _self.LoadedTreeNode.Properties;
        foreach(Property property in properties)
        {
            if(!property.Name.Equals(name)) continue;

            // Debug.Write($"[propName: {property.Name}]");
            // var selectedType = InspectorView.FindControl(name).SelectedItem;
            // Debug.Write($"[SelectedItem: {selectedType}]");

            // Debug.Write($"[Type: {Type.GetType("System."+selectedType.ToString())}]");
            // Debug.Write($"[Type: {Type.GetType()}]");

            // property.SetValue(Convert.ChangeType(value, Type.GetType("System."+selectedType.ToString())));
            break;

            // Type type = Type.GetType();
            // if(type is null) throw new Exception();

            // property.SetValue(Convert.ChangeType(value, type));
        }

        Debug.WriteLine("");
    }
}