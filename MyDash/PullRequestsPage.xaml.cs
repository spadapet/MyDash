using Microsoft.Maui.Controls;
using MyDash.Data.Model;

namespace MyDash;

public partial class PullRequestsPage : ContentView
{
    public PullRequestsModel Model { get; }

    public PullRequestsPage()
    {
        this.Model = new PullRequestsModel(App.Current.Model);
        this.InitializeComponent();
    }
}
