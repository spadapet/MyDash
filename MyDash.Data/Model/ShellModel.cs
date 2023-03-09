namespace MyDash.Data.Model;

public sealed class ShellModel : PropertyNotifier
{
    private ShellState state = ShellState.Loading;
    public ShellState State
    {
        get => this.state;
        set => this.SetProperty(ref this.state, value);
    }
}
