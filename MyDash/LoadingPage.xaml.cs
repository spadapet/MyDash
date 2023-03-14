using Microsoft.Maui.Controls;
using MyDash.Data.Model;
using MyDash.Data.Utility;
using System;
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

    private Task Load()
    {
        // TODO: Load settings from diskS
        //Settings settings = Settings.Deserialize(...);
        //App.Current.Model.Settings.CopyFrom(settings);

        App.Current.Model.State = AppState.Login;

        return Task.CompletedTask;
    }
}
