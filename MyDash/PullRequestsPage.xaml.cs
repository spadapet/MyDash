using Microsoft.Maui.Controls;
using MyDash.Data.Model;
using MyDash.Data.Utility;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace MyDash;

public partial class PullRequestsPage : ContentView, IUpdatable
{
    public PullRequestsModel Model { get; }
    private CancellationTokenSource cancellationTokenSource;

    public PullRequestsPage()
    {
        this.Model = new PullRequestsModel(App.Current.Model);
        this.InitializeComponent();
    }

    private void OnLoaded(object sender, EventArgs args)
    {
        this.StartUpdate();
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

            try
            {
                await this.UpdateAsync(this.cancellationTokenSource.Token);
            }
            catch
            {
                // this.Model.SetError(ex);
                throw;
            }
            finally
            {
                this.cancellationTokenSource = null;
            }
        });
    }

    private async Task UpdateAsync(CancellationToken cancellationToken)
    {
        //AdoModel ado = this.Model.AppModel.AdoModel;
        //AdoProject project = ado.CurrentAccount?.CurrentProject;

        await Task.Delay(0, cancellationToken);
    }
}
