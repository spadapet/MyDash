using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyDash.Data.Model;

public sealed class AdoModel : PropertyNotifier, IDisposable
{
    public ObservableCollection<AdoAccount> Accounts { get; } = new();

    public void Dispose()
    {
        if (this.connection != null)
        {
            this.connection.Dispose();
            this.connection = null;
        }
    }

    public void EnsureValid()
    {
        // Nothing to ensure yet
    }

    private AdoConnection connection;
    [JsonIgnore]
    public AdoConnection Connection
    {
        get => this.connection;
        set => this.SetProperty(ref this.connection, value);
    }

    private AdoAccount currentAccount;
    [JsonIgnore]
    public AdoAccount CurrentAccount
    {
        get => this.currentAccount;
        set
        {
            if (this.SetProperty(ref this.currentAccount, value))
            {
                this.previousAccountName = this.CurrentAccountName;
                this.OnPropertyChanged(nameof(this.CurrentAccountName));
            }
        }
    }

    private string previousAccountName;
    [JsonProperty(Order = 1)]
    public string CurrentAccountName
    {
        get => this.CurrentAccount?.Name ?? this.previousAccountName;
        set => this.CurrentAccount = this.Accounts.FirstOrDefault(a => a.Name == value);
    }
}
