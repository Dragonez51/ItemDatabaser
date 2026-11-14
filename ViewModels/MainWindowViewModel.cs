using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.Input;
using ItemDatabaser.DataStructures;

namespace ItemDatabaser.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public static StructureViewModel Structure { get; private set; } = new StructureViewModel();
    public static InspectorViewModel Inspector { get; private set; } = new InspectorViewModel();

    public static void LoadInspector(object data)
    {
        var category = data as Category;
        if(!(category is null)) Inspector.Load(category);
    }

    [RelayCommand][property: JsonIgnore] public void Export()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(Structure, options);
        string filePath = AppContext.BaseDirectory+"../../../"+Structure.DatabaseName+".json";
        File.WriteAllText(filePath, json);
    }
}