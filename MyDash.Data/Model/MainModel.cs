namespace MyDash.Data.Model;

public sealed class MainModel : PropertyNotifier
{
    public AppModel AppModel { get; }

    public MainModel(AppModel appModel)
    {
        this.AppModel = appModel;
    }
}
