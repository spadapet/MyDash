using Microsoft.Maui.Controls;
using MyDash.Data.Model;

namespace MyDash;

public partial class MainPage : ContentPage
{
    public MainModel Model { get; }

    public MainPage()
    {
        this.Model = new MainModel(App.Current.Model);
        this.InitializeComponent();
    }
}
