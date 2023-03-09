using Microsoft.Maui.Controls;
using MyDash.Data.Model;
using MyDash.Data.Utility;
using System;
using System.ComponentModel;

namespace MyDash;

public partial class AppShell : Shell
{
    public static new AppShell Current => (AppShell)Shell.Current;

    public ShellModel Model { get; }

    public AppShell()
    {
        this.Model = new ShellModel(App.Current.Model);
        this.InitializeComponent();
    }

    private void OnLoaded(object sender, EventArgs args)
    {
        this.Model.PropertyChanged += this.OnModelPropertyChanged;
        this.Model.AppModel.PropertyChanged += this.OnModelPropertyChanged;
    }

    private void OnUnloaded(object sender, EventArgs args)
    {
        this.Model.PropertyChanged -= this.OnModelPropertyChanged;
        this.Model.AppModel.PropertyChanged -= this.OnModelPropertyChanged;
    }

    private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        bool any = string.IsNullOrEmpty(args.PropertyName);

        if (sender == this.Model)
        {
        }
        else if (sender == this.Model.AppModel)
        {
            if (any || args.PropertyName == nameof(this.Model.AppModel.State))
            {
                this.GoToState(this.Model.AppModel.State);
            }
        }
    }

    private void GoToState(AppState state)
    {
        this.Dispatcher.Dispatch(() =>
        {
            string route = $"///{Enum.GetName(state)}";
            TaskUtility.FileAndForget(() => this.GoToAsync(route));
        });
    }
}
