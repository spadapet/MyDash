using Microsoft.Maui.Controls;
using MyDash.Data.Model;
using MyDash.Data.Utility;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyDash;

internal partial class AppShell : Shell, IUpdatable
{
    public static new AppShell Current => (AppShell)Shell.Current;
    public ShellModel Model { get; }
    private CancellationTokenSource cancellationTokenSource;

    public AppShell()
    {
        this.Model = new ShellModel(App.Current.Model);
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
        AdoModel ado = this.Model.AppModel.AdoModel;

        await AdoUtility.UpdateAccountsAsync(ado, cancellationToken);

        if (ado.CurrentAccount is AdoAccount account)
        {
            await AdoUtility.UpdateProjectsAsync(ado.Connection, account, cancellationToken);
        }

        if (this.CurrentPage is IUpdatable updatableChild)
        {
            updatableChild.StartUpdate();
        }
    }

    private void OnNavigated(object sender, ShellNavigatedEventArgs args)
    {
        this.pageTitleLabel.Text = this.CurrentPage?.Title;
    }

    private void OnClickCancel(object sender, EventArgs args)
    {
        this.cancellationTokenSource?.Cancel();
    }
}
