namespace MyDash.Data.Model;

public sealed class AppModel : PropertyNotifier
{
    private AppState state = AppState.Loading;
    public AppState State
    {
        get => this.state;
        set => this.SetProperty(ref this.state, value);
    }
}
