namespace MyDash.Data.Model;

public sealed class PullRequestsModel : PropertyNotifier
{
    public AppModel AppModel { get; }

    public PullRequestsModel(AppModel appModel)
    {
        this.AppModel = appModel;
    }
}
