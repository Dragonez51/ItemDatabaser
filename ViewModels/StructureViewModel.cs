using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia.Controls;
using ItemDatabaser.DataStructures;

namespace ItemDatabaser.ViewModels;

public partial class StructureViewModel : ViewModelBase
{
    public ObservableCollection<TreeNode> Nodes { get; private set; } = new ();

    public TreeNode? SelectedTreeNode { get; private set; }

    public StructureViewModel()
    {
        var weaponsProperties = new ObservableCollection<Property>()
        {
            new Property("Damage", typeof(Int32), 1),
            new Property("Durability", typeof(Int32), 10),
        };

        TreeNode weapons = new TreeNode("Weapon", weaponsProperties);
        weapons.AddSubNode(new TreeNode("Scythe", weaponsProperties));
        weapons.AddSubNode(new TreeNode("Sword", weaponsProperties));
        Nodes.Add(weapons);

        var armorProperties = new ObservableCollection<Property>()
        {
            new Property("Defense", typeof(Int32), 1),
            new Property("Durability", typeof(Int32), 10),
        };

        TreeNode armor = new TreeNode("Armor", armorProperties);
        armor.AddSubNode(new TreeNode("ChestPlate", armorProperties));
        armor.AddSubNode(new TreeNode("Leggings", armorProperties));
        Nodes.Add(armor);
    }

    public void AddNode()
    {
        if(SelectedTreeNode is null) return;
        SelectedTreeNode.AddSubNode(new TreeNode(SelectedTreeNode.Properties));
        // Debug.WriteLine("[StructureViewModel] AddNode() => idk what to do lol");
    }

    public void TreeView_SelectionChanged(object? sender, SelectionChangedEventArgs args)
    {
        var treeNode = (sender as TreeView)?.SelectedItem as TreeNode;
        // Debug.WriteLine($"[StructureViewModel] TreeView_SelectionChanged() => [sender: {treeNode}]");
        if(treeNode is null) throw new Exception("[StructureViewModel] SelectionChanged() => treeNode is null!");
        SelectedTreeNode = treeNode;
        MainWindowViewModel.LoadInspector(treeNode);
    }
}