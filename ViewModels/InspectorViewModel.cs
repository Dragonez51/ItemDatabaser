using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using ItemDatabaser.DataStructures;

namespace ItemDatabaser.ViewModels;

public partial class InspectorViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isTextVisible = true;
    [ObservableProperty] private bool _isPropertiesVisible = false;

    [ObservableProperty] private List<string>? _types;

    [ObservableProperty] private TreeNode? _loadedTreeNode;
    [ObservableProperty] private Category? _loadedCategory;

    [ObservableProperty] private Grid? _dataGrid;
    [ObservableProperty] private Item? _drawnItem;

    public InspectorViewModel()
    {
        LoadTypes();
    }

    private void LoadTypes()
    {
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

    public void Load(Category category)
    {
        DataGrid = new Grid();
        DataGrid.Classes.Add("DataGrid");
        LoadedCategory = category;
        GenerateData();
        ShowProperties();
    }

    private void GenerateColumnsAndHeaders()
    {
        if(LoadedCategory is null) return;
        if(DataGrid is null) return;
        
        DataGrid.ColumnDefinitions = new(){ new ColumnDefinition(GridLength.Auto) /* item name column*/ };

        Border border = new Border();
        border.Classes.Add("HeaderBorder");
        border[Grid.ColumnProperty] = 0;

        TextBlock ItemNameHeader = new TextBlock() { Text = "Name"};
        ItemNameHeader.Classes.Add("Header");
        
        border.Child = ItemNameHeader;

        DataGrid.Children.Add(border);

        int columnIndexer = 1;
        foreach(var property in LoadedCategory.Properties)
        {
            DataGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto)); // properties columns
            
            border = new Border();
            border.Classes.Add("HeaderBorder");
            border[Grid.ColumnProperty] = columnIndexer;

            StackPanel sp = new StackPanel(){ Orientation = Avalonia.Layout.Orientation.Horizontal };
            sp.Classes.Add("HeaderStackPanel");

            TextBox HeaderName = new TextBox(){ Text = property.Name };
            HeaderName.Classes.Add("HeaderBox");

            sp.Children.Add(HeaderName);

            ComboBox TypeBox = new ComboBox() { ItemsSource = Types };
            TypeBox.Bind(ComboBox.SelectedItemProperty, new ReflectionBinding(nameof(property.BindValueType)){ Source = property, Mode=BindingMode.TwoWay });
            TypeBox.AddHandler(ComboBox.SelectionChangedEvent, TypeBox_TypeChanged);
            TypeBox.Classes.Add("TypeBox");
            TypeBox.Classes.Add("Validate");

            sp.Children.Add(TypeBox);

            border.Child = sp;

            DataGrid.Children.Add(border);
            
            columnIndexer++;
        }

        DataGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto)); // delete item button / add property button column

        var addIcon = new Image(){ Source = new Bitmap(AssetLoader.Open(new Uri("avares://ItemDatabaser/Assets/Img/AddIcon.png"))) };
        addIcon.Classes.Add("ButtonImage");

        var AddPropButton = new Button(){ Content = addIcon };
        AddPropButton[Grid.ColumnProperty] = columnIndexer;
        AddPropButton.AddHandler(Button.ClickEvent, AddProp);
        AddPropButton.Classes.Add("Add");

        DataGrid.Children.Add(AddPropButton);
    }

    private void GenerateData()
    {
        if(LoadedCategory is null) return;
        if(DataGrid is null) return;

        DataGrid.Children.Clear();

        GenerateColumnsAndHeaders();

        DataGrid.RowDefinitions = new();
        DataGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));

        int rowIndex = 1;
        foreach(var item in LoadedCategory.Items)
        {
            DataGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));

            DrawnItem = item;

            TextBox NameBox = new TextBox();
            NameBox.Bind(TextBox.TextProperty, new ReflectionBinding(nameof(item.Name))
            {
                Source = item,
                Mode = BindingMode.TwoWay 
            });
            NameBox[Grid.ColumnProperty] = 0;
            NameBox[Grid.RowProperty] = rowIndex;
            NameBox.Classes.Add("NameBox");

            DataGrid.Children.Add(NameBox);

            if(item.Properties is null) break;

            int propColumnIndex = 1;
            foreach(var property in item.Properties)
            {
                TextBox ValueBox = new TextBox();
                ValueBox.Bind(TextBox.TextProperty, new ReflectionBinding(nameof(property.BindValue))
                {
                    Source = property,
                    Mode = BindingMode.TwoWay
                });
                ValueBox[Grid.ColumnProperty] = propColumnIndex;
                ValueBox[Grid.RowProperty] = rowIndex;
                ValueBox.DataContext = property;
                ValueBox.Classes.Add("ValueBox");
                ValueBox.Classes.Add("Validate");

                DataGrid.Children.Add(ValueBox); 

                propColumnIndex++;
            }

            Button Delete = new Button();
            Delete.AddHandler(Button.ClickEvent, DeleteItem);
            Delete[Grid.ColumnProperty] = propColumnIndex;
            Delete[Grid.RowProperty] = rowIndex;
            var deleteImg = new Image(){ Source = new Bitmap(AssetLoader.Open(new Uri("avares://ItemDatabaser/Assets/Img/Trashcan.png"))) };
            deleteImg.Classes.Add("ButtonImage");
            Delete.Content = deleteImg;
            Delete.Classes.Add("Delete");

            DataGrid.Children.Add(Delete);

            rowIndex++;
        }
    }

    public void Load(TreeNode treeNode)
    {
        LoadedTreeNode = treeNode;
        ShowProperties();
    }

    private void ShowProperties()
    {
        IsTextVisible = false;
        IsPropertiesVisible = true;
    }

    public void AddProperty()
    {
        LoadedTreeNode?.Properties.Add(new Property());
    }

    public void AddItem()
    {
        LoadedCategory?.AddItem();
        GenerateData();
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

    public void DeleteItem(object? sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if(button is null) return;
        if(button.Name is null) return;
        DeleteItem(button.Name);
    }
    public void DeleteItem(string name)
    {
        if(LoadedCategory is null) return;

        foreach(var item in LoadedCategory.Items)
        {
            if(!item.Name.Equals(name)) continue;

            LoadedCategory.Items.Remove(item);
            return;
        }
    }

    public void AddProp(object? sender, RoutedEventArgs e) => AddCategoryProperty();
    public void AddCategoryProperty()
    {
        LoadedCategory?.AddProperty();
        GenerateData();
    }

    private void TypeBox_TypeChanged(object? sender, SelectionChangedEventArgs e)
    {
        LoadedCategory?.UpdateItems();
    }
}