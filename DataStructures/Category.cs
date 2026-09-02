using System.Collections.ObjectModel;

namespace ItemDatabaser.DataStructures;

public class Category
{
    public ObservableCollection<Property> Properties { get; private set; } = new ();
    public ObservableCollection<Item>   Items      { get; private set; } = new ();

    public string Name { get; private set; } = "New Category";

    public Category(string name)
    {
        Name = name;
    }

    public void AddProperty() => Properties.Add(new Property());
    public void AddItem(string name) => Items.Add(new Item(name, Properties));
    public void UpdateItems()
    {
        foreach(var item in Items) item.SetProperties(Properties);
    } 
}