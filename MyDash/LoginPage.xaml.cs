using Microsoft.Maui.Controls;
using MyDash.Data.Model;
using MyDash.Data.Utility;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyDash;

public partial class LoginPage : ContentPage
{
    public LoginModel Model { get; }
    private CancellationTokenSource cancellationTokenSource;

    public LoginPage()
    {
        this.Model = new LoginModel(App.Current.Model);
        this.InitializeComponent();
    }

    private void OnLoaded(object sender, EventArgs args)
    {
        this.StartLogin();
    }

    private void OnUnloaded(object sender, EventArgs args)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource = null;
    }

    private void StartLogin()
    {
        TaskUtility.FileAndForget(async () =>
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            this.Model.SetError(null);

            try
            {
                await this.Login(this.cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                this.Model.SetError(ex);
                throw;
            }
            finally
            {
                this.cancellationTokenSource = null;
            }
        });
    }

    private async Task Login(CancellationToken cancellationToken)
    {
        AdoModel adoModel = this.Model.AppModel.AdoModel;
        adoModel.Authentication = await AdoConnectionUtility.GetConnectionAsync(cancellationToken);
        IEnumerable<AdoAccount> accounts = await AdoUtility.GetAccounts(adoModel.Authentication, cancellationToken);

        adoModel.Accounts.Clear();
        foreach (AdoAccount account in accounts)
        {
            adoModel.Accounts.Add(account);
        }

        this.Model.AppModel.State = AppState.PullRequests;
    }

    private void OnCancelClicked(object sender, EventArgs args)
    {
        this.cancellationTokenSource?.Cancel();
    }

    private void OnRetryClicked(object sender, EventArgs args)
    {
        this.StartLogin();
    }
}
