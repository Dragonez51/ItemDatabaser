using System;
using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using ItemDatabaser.DataStructures;

namespace ItemDatabaser.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private static StructureViewModel _structure = new StructureViewModel();
    public static InspectorViewModel Inspector { get; private set; } = new InspectorViewModel();

    public static void LoadInspector(object data)
    {
        var category = data as Category;
        if(!(category is null)) Inspector.Load(category);
    }

    public void Export(Uri uri)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(Structure, options);
        File.WriteAllText(uri.AbsolutePath, json);
    }

    public void Import(Uri uri)
    {
        var import = File.ReadAllText(uri.AbsolutePath);
        var deserialized = JsonSerializer.Deserialize<StructureViewModel>(import);
        if(deserialized is null) throw new Exception("[MainWindowViewModel] Import() => deserialized is null!");

        Structure = deserialized;
        Structure.LinkProperties();
        Inspector.Unload();
    }
}