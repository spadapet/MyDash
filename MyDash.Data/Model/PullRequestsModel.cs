using System.Collections.ObjectModel;

namespace MyDash.Data.Model;

public sealed class PullRequestsModel : PropertyNotifier
{
    public AppModel AppModel { get; }
    public PullRequestsType PullRequestsType { get; }

    public PullRequestsModel(AppModel appModel, PullRequestsType pullRequestsType)
    {
        this.AppModel = appModel;
        this.PullRequestsType = pullRequestsType;
    }

    private ObservableCollection<AdoPullRequest> pullRequests;
    public ObservableCollection<AdoPullRequest> PullRequests
    {
        get => this.pullRequests;
        set => this.SetProperty(ref this.pullRequests, value);
    }
}
