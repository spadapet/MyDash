using Microsoft.Maui.Controls;

namespace MyDash;

public partial class App : Application
{
	public App()
	{
        this.InitializeComponent();

        this.MainPage = new AppShell();
	}
}
