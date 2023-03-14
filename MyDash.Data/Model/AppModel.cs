namespace MyDash.Data.Model;

public sealed class AppModel : PropertyNotifier
{
    public Settings Settings { get; } = new Settings();

    private AppState state = AppState.Loading;
    public AppState State
    {
        get => this.state;
        set => this.SetProperty(ref this.state, value);
    }

    private AdoModel adoModel = new();
    public AdoModel AdoModel
    {
        get => this.adoModel;
        set => this.SetProperty(ref this.adoModel, value);
    }
}
