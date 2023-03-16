using Microsoft.Maui.Controls;
using MyDash.Data.Model;
using MyDash.Data.Utility;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyDash;

public partial class LoadingPage : ContentPage
{
    public LoadingPage()
    {
        this.InitializeComponent();
    }

    private void OnLoaded(object sender, EventArgs args)
    {
        this.StartLoad();
    }

    private void StartLoad()
    {
        TaskUtility.FileAndForget(this.Load);
    }

    private async Task Load()
    {
        using (CancellationTokenSource cancellationTokenSource = new(TimeSpan.FromSeconds(4)))
        {
            await App.Current.LoadStateAsync(cancellationTokenSource.Token);
        }

        App.Current.Model.State = AppState.Login;
    }
}
