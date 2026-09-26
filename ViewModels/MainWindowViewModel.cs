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
}