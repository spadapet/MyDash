using Microsoft.Identity.Client;
using Microsoft.Maui.Controls;
using Microsoft.VisualStudio.Services.Account;
using MyDash.Data.Model;
using MyDash.Data.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
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
        this.cancellationTokenSource = new CancellationTokenSource();
        TaskUtility.FileAndForget(() => this.Login(this.cancellationTokenSource.Token));
    }

    private void OnUnloaded(object sender, EventArgs args)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource = null;
    }

    private async Task Login(CancellationToken cancellationToken)
    {
        AppModel appModel = this.Model.AppModel;
        appModel.AdoAuthentication = await AdoAuthenticationUtility.GetAuthenticationAsync(cancellationToken);
        List<Account> accounts = await AdoUtility.GetAccounts(appModel.AdoAuthentication, cancellationToken);

        appModel.AdoAccounts.Clear();
        foreach (Account account in accounts
            .Where(a => a.AccountStatus == AccountStatus.None || a.AccountStatus == AccountStatus.Enabled)
            .OrderBy(a => a.AccountName))
        {
            appModel.AdoAccounts.Add(account);
        }
    }
}
