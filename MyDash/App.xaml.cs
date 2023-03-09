using Microsoft.Maui.Controls;
using MyDash.Data.Model;

namespace MyDash;

public partial class App : Application
{
    public static new App Current => (App)Application.Current;
    public AppModel Model { get; }

	public App()
	{
        this.Model = new AppModel();
        this.InitializeComponent();
	}
}
