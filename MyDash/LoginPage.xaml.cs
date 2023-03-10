using Microsoft.Maui.Controls;
using MyDash.Data.Model;
using MyDash.Data.Utility;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyDash;

public partial class LoginPage : ContentPage
{
    public LoginModel Model { get; }

    public LoginPage()
    {
        this.Model = new LoginModel(App.Current.Model);
        this.InitializeComponent();
    }

    private void OnLoaded(object sender, EventArgs args)
    {
        TaskUtility.FileAndForget(async () =>
        {
            await Task.Delay(3000);
            this.Model.SetError(new FileNotFoundException("Missing some file", "foo.bar"));
        });
    }

    private void OnUnloaded(object sender, EventArgs args)
    {

    }
}
