using System;
using System.Collections.Generic;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using ItemDatabaser.DataStructures;

namespace ItemDatabaser.ViewModels;

public partial class InspectorViewModel : ViewModelBase
{
    private static InspectorViewModel? _self;

    [ObservableProperty] private bool _isTextVisible = true;
    [ObservableProperty] private bool _isPropertiesVisible = false;

    [ObservableProperty] private List<string>? _types;

    [ObservableProperty] private TreeNode? _loadedTreeNode;

    public InspectorViewModel()
    {
        _self = this;
        LoadTypes();
    }

    private void LoadTypes()
    {
        // Types = new List<Type>()
        // {
        //     typeof(Int16),
        //     typeof(int),
        //     typeof(long),
        //     typeof(char),
        //     typeof(string),
        //     typeof(bool)
        // };
        Types = new List<string>()
        {
            typeof(Int16).Name,
            typeof(int).Name,
            typeof(long).Name,
            typeof(char).Name,
            typeof(string).Name,
            typeof(bool).Name
        };
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
        if(LoadedTreeNode is null) return;
        if(LoadedTreeNode.Properties is null) return;
        foreach(var property in LoadedTreeNode.Properties)
        {
            if(property.Name.Equals(name))
            {
                LoadedTreeNode.Properties.Remove(property);
                return;
            }
        }
    }

}