namespace MyDash.Data.Model;

public sealed class ShellModel : PropertyNotifier
{
    public AppModel AppModel { get; }

    public ShellModel(AppModel appModel)
    {
        this.AppModel = appModel;
    }
}
