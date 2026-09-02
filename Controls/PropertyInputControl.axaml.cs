using System;
using Avalonia.Controls;
using Avalonia.Input;
using ItemDatabaser.ViewModels;

namespace ItemDatabaser.Controls;

public class PropertyInputControl : TextBox
{
    protected override Type StyleKeyOverride => typeof(TextBox);

    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);
        // var textBox = e.Source as TextBox;
        // if(textBox is null) throw new System.Exception("[PropertyInputControl] OnTextInput() => textBox is null... cringe bro...");
        // InspectorViewModel.ChangeProperty(textBox.Name, textBox.Text);
    }
}