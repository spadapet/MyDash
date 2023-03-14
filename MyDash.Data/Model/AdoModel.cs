using System.Collections.ObjectModel;
using System.Linq;

namespace MyDash.Data.Model;

public sealed class AdoModel : PropertyNotifier
{
    public ObservableCollection<AdoAccount> Accounts { get; } = new();

    private AdoConnection authentication;
    public AdoConnection Authentication
    {
        get => this.authentication;
        set => this.SetProperty(ref this.authentication, value);
    }

    private AdoAccount currentAccount;
    public AdoAccount CurrentAccount
    {
        get => (this.currentAccount != null && this.Accounts.Contains(this.currentAccount))
            ? this.currentAccount
            : this.Accounts.FirstOrDefault();

        set => this.SetProperty(ref this.currentAccount, value);
    }
}
