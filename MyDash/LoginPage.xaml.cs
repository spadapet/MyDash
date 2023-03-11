using Microsoft.Maui.Controls;
using MyDash.Data.Model;
using MyDash.Data.Utility;
using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Threading;

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

        TaskUtility.FileAndForget(async () =>
        {
            AuthenticationHeaderValue foo = await AdoAuthenticationUtility.GetAuthenticationHeaderValueAsync(this.cancellationTokenSource.Token);
            Debug.Assert(foo != null);
        });
    }

    private void OnUnloaded(object sender, EventArgs args)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource = null;
    }
}
