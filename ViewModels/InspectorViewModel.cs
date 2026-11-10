using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using ItemDatabaser.DataStructures;

namespace ItemDatabaser.ViewModels;

public partial class InspectorViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isTextVisible = true; // should "Select category" text be visible
    [ObservableProperty] private bool _isPropertiesVisible = false; // should data table be visible

    [ObservableProperty] private DataGrid? _dataTable; // table structure (mb rename)
    [ObservableProperty] private Grid? _dataGrid; // Avalonia object to link the visuals
    [ObservableProperty] private string? _categoryName; // category name that is presented at the top of InspectorView

    public void Load(Category category)
    {
        DataTable = new DataGrid(category);
        DataGrid = DataTable.MainGrid;
        CategoryName = category.Name;
        ShowProperties();
    }

    public void AddItem()
    {
        DataTable?.AddItem();
    }

    private void ShowProperties()
    {
        IsTextVisible = false;
        IsPropertiesVisible = true;
    }
}