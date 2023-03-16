using Microsoft.Maui.Controls;
using MyDash.Data.Model;
using MyDash.Data.Utility;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyDash;

public partial class AppShell : Shell, IUpdatable
{
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
}
