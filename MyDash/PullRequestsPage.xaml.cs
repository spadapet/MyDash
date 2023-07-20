using Microsoft.Maui.Controls;
using MyDash.Data.Model;
using MyDash.Data.Utility;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace MyDash;

internal sealed class PullRequestsAllPage : PullRequestsPage
{
    public PullRequestsAllPage()
        : base(PullRequestsType.All)
    { }
}

internal sealed class PullRequestsMinePage : PullRequestsPage
{
    public PullRequestsMinePage()
        : base(PullRequestsType.Mine)
    { }
}

internal abstract partial class PullRequestsPage : ContentPage, IUpdatable
{
    public PullRequestsModel Model { get; }
    private CancellationTokenSource cancellationTokenSource;
    private AdoAccount currentAccount;
    private AdoProject currentProject;

    public PullRequestsPage(PullRequestsType pullRequestsType)
    {
        this.Model = new PullRequestsModel(App.Current.Model, pullRequestsType);
        this.InitializeComponent();
    }

    protected void OnLoaded(object sender, EventArgs args)
    {
        this.StartUpdate();
    }

    protected void OnUnloaded(object sender, EventArgs e)
    {
        this.ClearCurrent();
    }

    private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        bool all = string.IsNullOrEmpty(args.PropertyName);

        if (this.currentAccount == sender)
        {
            if (all || args.PropertyName == nameof(this.currentAccount.CurrentProject))
            {
                if (this.currentProject != this.currentAccount.CurrentProject)
                {
                    this.StartUpdate();
                }
            }
        }
    }

    public void StartUpdate()
    {
        if (this.cancellationTokenSource != null)
        {
            // Already updating
            return;
        }

        TaskUtility.FileAndForget(async () =>
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            WorkData work = new("Getting pull requests", this.cancellationTokenSource)
            {
                Progress = 1
            };

            using (this.Model.AppModel.ProgressBar.Begin(work))
            {
                try
                {
                    await this.UpdateAsync(this.cancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    this.Model.AppModel.InfoBar.SetError(ex);
                    throw;
                }
                finally
                {
                    this.cancellationTokenSource = null;
                }
            }
        });
    }

    private async Task UpdateAsync(CancellationToken cancellationToken)
    {
        this.ClearCurrent();

        AdoModel ado = this.Model.AppModel.AdoModel;
        if (ado.CurrentAccount is AdoAccount account)
        {
            this.currentAccount = account;
            this.currentAccount.PropertyChanged += this.OnModelPropertyChanged;

            if (this.currentAccount.CurrentProject is AdoProject project)
            {
                this.currentProject = project;
                this.Model.PullRequests = await AdoUtility.UpdatePullRequestsAsync(this.Model.PullRequestsType, ado.Connection, ado.CurrentAccount, project, cancellationToken);
            }
        }
    }

    private void ClearCurrent()
    {
        if (this.currentAccount != null)
        {
            this.currentAccount.PropertyChanged -= this.OnModelPropertyChanged;
            this.currentAccount = null;
            this.currentProject = null;
        }
    }
}
