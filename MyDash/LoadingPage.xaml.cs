using Microsoft.Maui.Controls;
using MyDash.Data.Model;
using System;

namespace MyDash;

public partial class LoadingPage : ContentPage
{
    public LoadingPage()
    {
        this.InitializeComponent();
    }

    private void OnLoaded(object sender, EventArgs args)
    {
        // Just go right to the login state since there is nothing to load yet
        AppShell.Current.Model.State = ShellState.Login;
    }
}
