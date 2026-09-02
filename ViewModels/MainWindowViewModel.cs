using ItemDatabaser.DataStructures;

namespace ItemDatabaser.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public static StructureViewModel Structure { get; private set; } = new StructureViewModel();
    public static InspectorViewModel Inspector { get; private set; } = new InspectorViewModel();

    public static void LoadInspector(TreeNode treeNode)
    {
        Inspector.Load(treeNode);
    }
}