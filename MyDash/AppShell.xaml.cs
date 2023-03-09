using Microsoft.Maui.Controls;
using MyDash.Data.Model;
using MyDash.Data.Utility;
using System.ComponentModel;

namespace MyDash;

public partial class AppShell : Shell
{
    public static new AppShell Current => (AppShell)Shell.Current;

    public ShellModel Model { get; }

    public AppShell()
    {
        this.Model = new ShellModel();
        this.Model.PropertyChanged += this.OnModelPropertyChanged;
        this.InitializeComponent();
    }

    private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        bool any = string.IsNullOrEmpty(args.PropertyName);

        if (any || args.PropertyName == nameof(this.Model.State))
        {
            this.GoToState(this.Model.State);
        }
    }

    private void GoToState(ShellState state)
    {
        this.Dispatcher.Dispatch(() =>
        {
            string route = $"/Enum.GetName(state)";
            TaskUtility.FileAndForget(() => this.GoToAsync(route));
        });
    }
}
