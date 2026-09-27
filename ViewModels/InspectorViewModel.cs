using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ItemDatabaser.DataStructures;

namespace ItemDatabaser.ViewModels;

public partial class InspectorViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isTextVisible = true;
    [ObservableProperty] private bool _isPropertiesVisible = false;

    [ObservableProperty] private List<string>? _types; // list of types to choose from typeBox

    [ObservableProperty] private Category? _loadedCategory; // selected category of items

    [ObservableProperty] private Grid? _dataGrid; // Avalonia object to bind visuals
    [ObservableProperty] private Item? _drawnItem; // idk?

    private string? _selectedPropertyName;

    public InspectorViewModel()
    {
        LoadTypes();
    }

    private void LoadTypes()
    {
        Types = new List<string>()
        {
            typeof(byte).Name,
            typeof(sbyte).Name,
            typeof(short).Name,
            typeof(ushort).Name,
            typeof(int).Name,
            typeof(uint).Name,
            typeof(long).Name,
            typeof(ulong).Name,
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

        foreach(var property in LoadedCategory.Properties)
        {
            DataGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto)); // properties columns
            
            border = new Border();
            border.Classes.Add("HeaderBorder");
            border[Grid.ColumnProperty] = property.Column;

            StackPanel sp = new StackPanel(){ Orientation = Avalonia.Layout.Orientation.Vertical };
            sp.Classes.Add("HeaderStackPanel");

            StackPanel innerSP = new StackPanel(){ Orientation = Avalonia.Layout.Orientation.Horizontal };
            innerSP.Classes.Add("HeaderStackPanel");

            TextBox HeaderName = new TextBox(){ Text = property.Name };
            HeaderName.AddHandler(TextBox.TextChangedEvent, HeaderBox_TextChanged);
            HeaderName.AddHandler(TextBox.GotFocusEvent, HeaderBox_GotFocus);
            HeaderName.Classes.Add("HeaderBox");

            var modButtonImg = new Image(){ Source = new Bitmap(AssetLoader.Open(new Uri("avares://ItemDatabaser/Assets/Img/ModButtonImg.png"))) };
            modButtonImg.Classes.Add("ButtonImage");
            modButtonImg.Width = 10;
            Button ModButton = new Button(){ Content = modButtonImg };
            ModButton.Width = 20;

            var ModFlyout = new MenuFlyout();

            MenuItem Left = new MenuItem();
            Left.Header = "< Left";
            Left.Command = MovePropLeftCommand;
            Left.CommandParameter = property.Column;
            ModFlyout.Items.Add(Left);

            MenuItem Right = new MenuItem();
            Right.Header = "> Right";
            Right.Command = MovePropRightCommand;
            Right.CommandParameter = property.Column;
            ModFlyout.Items.Add(Right);

            MenuItem Separator = new MenuItem();
            Separator.Header = "-";
            ModFlyout.Items.Add(Separator);

            MenuItem DeleteProp = new MenuItem();
            DeleteProp.Icon = new Image(){ Source = new Bitmap(AssetLoader.Open(new Uri("avares://ItemDatabaser/Assets/Img/Trashcan.png"))) };
            DeleteProp.Header = "Delete";
            DeleteProp.Command = DeletePropCommand;
            DeleteProp.CommandParameter = property.Column;
            ModFlyout.Items.Add(DeleteProp);

            ModButton.Flyout = ModFlyout;

            innerSP.Children.Add(HeaderName);
            innerSP.Children.Add(ModButton);

            sp.Children.Add(innerSP);

            ComboBox TypeBox = new ComboBox() { ItemsSource = Types };
            TypeBox.Bind(ComboBox.SelectedItemProperty, new ReflectionBinding(nameof(property.BindValueType)){ Source = property, Mode=BindingMode.TwoWay });
            TypeBox.AddHandler(ComboBox.SelectionChangedEvent, TypeBox_TypeChanged);
            TypeBox.Classes.Add("TypeBox");
            TypeBox.Classes.Add("Validate");

            sp.Children.Add(TypeBox);

            border.Child = sp;

            DataGrid.Children.Add(border);
        }

        DataGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto)); // delete item button / add property button column

        var addIcon = new Image(){ Source = new Bitmap(AssetLoader.Open(new Uri("avares://ItemDatabaser/Assets/Img/AddIcon.png"))) };
        addIcon.Classes.Add("ButtonImage");

        var AddPropButton = new Button(){ Content = addIcon };
        AddPropButton[Grid.ColumnProperty] = LoadedCategory.Properties.Count+1;
        AddPropButton.AddHandler(Button.ClickEvent, AddProp);
        AddPropButton.Classes.Add("Add");

        DataGrid.Children.Add(AddPropButton);
    }

    private void GenerateData()
    {
        _selectedPropertyName = null;

        if(LoadedCategory is null) return;
        if(DataGrid is null) return;

        DataGrid.Children.Clear();

        GenerateColumnsAndHeaders();

        DataGrid.RowDefinitions = new(){new RowDefinition(GridLength.Star)};

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

            foreach(var property in item.Properties)
            {
                TextBox ValueBox = new TextBox();
                ValueBox.Bind(TextBox.TextProperty, new ReflectionBinding(nameof(property.BindValue))
                {
                    Source = property,
                    Mode = BindingMode.TwoWay
                });
                ValueBox[Grid.ColumnProperty] = property.Column;
                ValueBox[Grid.RowProperty] = rowIndex;
                ValueBox.DataContext = property;
                ValueBox.Classes.Add("ValueBox");
                ValueBox.Classes.Add("Validate");

                DataGrid.Children.Add(ValueBox); 
            }

            Button Delete = new Button();
            Delete.AddHandler(Button.ClickEvent, DeleteItem);
            Delete[Grid.ColumnProperty] = LoadedCategory.Properties.Count+1;
            Delete[Grid.RowProperty] = rowIndex;
            Delete.Bind(Button.NameProperty, new ReflectionBinding(nameof(item.ItemID)){ Source = item });
            var deleteImg = new Image(){ Source = new Bitmap(AssetLoader.Open(new Uri("avares://ItemDatabaser/Assets/Img/Trashcan.png"))) };
            deleteImg.Classes.Add("ButtonImage");
            Delete.Content = deleteImg;
            Delete.Classes.Add("Delete");

            DataGrid.Children.Add(Delete);

            rowIndex++;
        }
    }

    private void ShowProperties()
    {
        IsTextVisible = false;
        IsPropertiesVisible = true;
    }

    public void AddItem()
    {
        LoadedCategory?.AddItem();
        GenerateData();
    }

    public void DeleteItem(object? sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if(button is null) return;
        if(button.Name is null) return;
        DeleteItem(button.Name);
    }
    public void DeleteItem(string id)
    {
        if(LoadedCategory is null) return;

        foreach(var item in LoadedCategory.Items)
        {
            if(item.ItemID != Int32.Parse(id)) continue;

            LoadedCategory.Items.Remove(item);
            GenerateData();
            return;
        }
    }

    public void AddProp(object? sender, RoutedEventArgs e) => AddProp();
    public void AddProp()
    {
        LoadedCategory?.AddProperty();
        GenerateData();
    }

    private void TypeBox_TypeChanged(object? sender, SelectionChangedEventArgs e)
    {
        // Debug.WriteLine("[InspectorViewModel] TypeBox_TypeChanged() => ");
        // Debug.WriteLine("");
        var senderTypeBox = sender as ComboBox;
        if(senderTypeBox is null) return;
        if(senderTypeBox.SelectedItem is null) return;
        string typeName = (string)senderTypeBox.SelectedItem;
        
        var sp = senderTypeBox.Parent as StackPanel;
        if(sp is null) return;

        var UpperSP = sp.Children[0] as StackPanel;
        if(UpperSP is null) return;

        var propHeader = UpperSP.Children[0] as TextBox;
        if(propHeader is null) return;

        // Debug.WriteLine($"[InspectorViewModel] TypeBox_TypeChanged() => Successfully got property name: {propHeader.Text}"); 

        // LoadedCategory?.UpdateItems();
        if(propHeader.Text is null) return;
        if(typeName is null) return;
        LoadedCategory?.UpdateTypes(propHeader.Text, typeName);
    }

    private void HeaderBox_TextChanged(object? sender, TextChangedEventArgs args)
    {
        if(LoadedCategory is null) return;

        var headerBox = sender as TextBox;
        if(headerBox is null) return;
        if(headerBox.Text is null) return;

        foreach(var prop in LoadedCategory.Properties)
        {
            if(!prop.Name.Equals(_selectedPropertyName)) continue;

            LoadedCategory.UpdateItemPropName(prop.Name, headerBox.Text);
            prop.Name = headerBox.Text;
            _selectedPropertyName = headerBox.Text;
            break;
        }

    }

    private void HeaderBox_GotFocus(object? sender, EventArgs args)
    {
        _selectedPropertyName = (sender as TextBox)?.Text;
    }

    [RelayCommand]
    private void MovePropLeft(int index)
    {
        if(LoadedCategory is null) return;
        if(index == 1) return;
        LoadedCategory.MoveProp("Left", index);
        GenerateData();
    }

    [RelayCommand]
    private void MovePropRight(int index)
    {
        if(LoadedCategory is null) return;
        if(index == LoadedCategory.Properties.Count) return;
        LoadedCategory.MoveProp("Right", index);
        GenerateData();
    }

    [RelayCommand]
    private void DeleteProp(int index)
    {
        // Debug.WriteLine($"[InspectorViewModel] DeleteProp() => [index: {index}]");
        if(LoadedCategory is null) return;
        LoadedCategory.DeleteProperty(index);
        GenerateData();
    }
}