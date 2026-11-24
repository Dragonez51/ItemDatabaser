using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using ItemDatabaser.DataStructures;

namespace ItemDatabaser.ViewModels;

public partial class InspectorViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isTextVisible = true; // should "Select category" text be visible
    [ObservableProperty] private bool _isPropertiesVisible = false; // should data table be visible

    [ObservableProperty] private DataGrid? _data; // table structure (mb rename)
    [ObservableProperty] private Grid? _dataGrid; // Avalonia object to link the visuals

    public void Load(Category category)
    {
        Data = new DataGrid(category);
        DataGrid = Data.MainGrid;
        ShowProperties();
    }

    public void Unload()
    {
        HideProperties();
    }

    public void AddItem()
    {
        Data?.AddItem();
    }

    private void ShowProperties()
    {
        IsTextVisible = false;
        IsPropertiesVisible = true;
    }

    private void HideProperties()
    {
        IsTextVisible = true;
        IsPropertiesVisible = false;
    }
}