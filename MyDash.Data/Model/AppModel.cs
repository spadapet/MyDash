namespace MyDash.Data.Model;

public sealed class AppModel : PropertyNotifier
{
    public static string LoadingRouteName = nameof(AppState.Loading);
    public static string LoginRouteName = nameof(AppState.Login);
    public static string DashboardRouteName = nameof(AppState.Dashboard);

    public Settings Settings { get; } = new Settings();

    private AppState state = AppState.Loading;
    public AppState State
    {
        get => this.state;
        set => this.SetProperty(ref this.state, value);
    }
}
