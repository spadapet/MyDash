using Microsoft.VisualStudio.Services.WebApi;

namespace MyDash.Data.Model;

internal sealed class AdoConnectionInternal : AdoConnection
{
    public string AccountName { get; set; }
    public VssConnection Connection { get; set; }
}
