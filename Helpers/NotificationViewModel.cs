using CommunityToolkit.Mvvm.ComponentModel;

namespace ItemDatabaser.Helpers;

public partial class NotificationViewModel : ObservableObject
{
    public string Message   { get; private set; } = "";

    public bool   IsInfo    { get; private set; } = false;
    public bool   IsOk      { get; private set; } = false;
    public bool   IsError   { get; private set; } = false;

    public NotificationViewModel(string message, string type = "info")
    {
        Message = message;
        LinkType(type);
    }

    private void LinkType(string type)
    {
        type = type.ToLower();
        switch (type)
        {
            case "info":
                IsInfo = true;
                break;
            case "ok":
                IsOk = true;
                break;
            case "error":
                IsError = true;
                break;
            default:
                throw new System.Exception("[NotificationViewModel] LinkType() => invalid notification type!");
        }
    }
}