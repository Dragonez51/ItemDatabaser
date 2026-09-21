using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia.Controls;
using ItemDatabaser.DataStructures;

namespace ItemDatabaser.ViewModels;

public partial class StructureViewModel : ViewModelBase
{
    public ObservableCollection<Category> CategoryNodes { get; private set; } = new ();
    public ObservableCollection<TreeNode> TreeNodes { get; private set; } = new ();

    public TreeNode? SelectedTreeNode { get; private set; }

    public StructureViewModel()
    {
        CreateCategoryItemStructure();
    }

    private void CreateCategoryItemStructure()
    {
        var weaponsProperties = new ObservableCollection<Property>()
        {
            new Property("Damage", typeof(Int32), 1),
            new Property("Durability", typeof(Int32), 10),
        };

        CategoryNodes.Add(new Category("Weapons", weaponsProperties));

        var armorProperties = new ObservableCollection<Property>()
        {
            new Property("Defense", typeof(Int32), 1),
            new Property("Durability", typeof(Int32), 10),
        };

        CategoryNodes.Add(new Category("Armor", armorProperties));
    }

    private void CreateTreeNodeStructure()
    {
        var weaponsProperties = new ObservableCollection<Property>()
        {
            new Property("Damage", typeof(Int32), 1),
            new Property("Durability", typeof(Int32), 10),
        };

        TreeNode weapons = new TreeNode("Weapon", weaponsProperties);
        weapons.AddSubNode(new TreeNode("Scythe", weaponsProperties));
        weapons.AddSubNode(new TreeNode("Sword", weaponsProperties));
        TreeNodes.Add(weapons);

        var armorProperties = new ObservableCollection<Property>()
        {
            new Property("Defense", typeof(Int32), 1),
            new Property("Durability", typeof(Int32), 10),
        };

        TreeNode armor = new TreeNode("Armor", armorProperties);
        armor.AddSubNode(new TreeNode("ChestPlate", armorProperties));
        armor.AddSubNode(new TreeNode("Leggings", armorProperties));
        TreeNodes.Add(armor);
    }

    public void AddNode()
    {
        if(SelectedTreeNode is null) return;
        SelectedTreeNode.AddSubNode(new TreeNode(SelectedTreeNode.Properties));
    }

    public void TreeView_SelectionChanged(object? sender, SelectionChangedEventArgs args)
    {
        var tree = sender as TreeView;
        if(tree is null) return;
        var item = tree.SelectedItem;

        var treeNode = item as TreeNode;
        if(!(treeNode is null))
        {
            SelectedTreeNode = treeNode;
            MainWindowViewModel.LoadInspector(treeNode);
        }

        var category = item as Category;
        if(!(category is null))
        {
            MainWindowViewModel.LoadInspector(category);
        }
    }
}