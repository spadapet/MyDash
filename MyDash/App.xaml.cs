using Microsoft.Maui;
using Microsoft.Maui.Controls;
using MyDash.Data;
using MyDash.Data.Model;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace MyDash;

public partial class App : Application
{
    public static new App Current => (App)Application.Current;
    public AppModel Model { get; } = new AppModel();

    public App()
    {
        this.InitializeComponent();
        this.UpdateMainPage();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        Debug.Assert(this.Windows.Count == 0);

        Window window = base.CreateWindow(activationState);
        window.Title = DataResource.AppName;
        window.Created += this.OnWindowCreated;
        window.Stopped += this.OnWindowStopped;

        return window;
    }

    private void OnWindowCreated(object sender, EventArgs e)
    {
        this.Model.PropertyChanged += this.OnModelPropertyChanged;
    }

    private void OnWindowStopped(object sender, EventArgs args)
    {
        Window window = (Window)sender;
        window.Created -= this.OnWindowCreated;
        window.Stopped -= this.OnWindowStopped;

        this.Model.PropertyChanged -= this.OnModelPropertyChanged;
    }

    private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        this.UpdateMainPage();
    }

    private void UpdateMainPage()
    {
        void EnsureMainPage<T>() where T : Page, new()
        {
            if (this.MainPage is not T)
            {
                this.MainPage = new T();
            }
        }

        switch (this.Model.State)
        {
            case AppState.Loading:
                EnsureMainPage<LoadingPage>();
                break;

            case AppState.Login:
                EnsureMainPage<LoginPage>();
                break;

            case AppState.Dashboard:
                EnsureMainPage<AppShell>();
                break;

            default:
                throw new InvalidOperationException();
        }
    }
}
