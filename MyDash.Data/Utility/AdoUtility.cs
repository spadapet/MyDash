using Microsoft.VisualStudio.Services.Account;
using Microsoft.VisualStudio.Services.Account.Client;
using Microsoft.VisualStudio.Services.OAuth;
using Microsoft.VisualStudio.Services.WebApi;
using MyDash.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyDash.Data.Utility;

public static class AdoUtility
{
    public static async Task<IEnumerable<AdoAccount>> GetAccounts(AdoConnection connection, CancellationToken cancellationToken)
    {
        VssOAuthAccessTokenCredential credentials = new VssOAuthAccessTokenCredential(connection.AccessToken);
        List<AdoAccount> results = new();

        using (VssConnection vsspsConnection = new VssConnection(new Uri("https://app.vssps.visualstudio.com"), credentials))
        {
            AccountHttpClient accountsClient = await vsspsConnection.GetClientAsync<AccountHttpClient>(cancellationToken);
            IEnumerable<Account> accounts = await accountsClient.GetAccountsByMemberAsync(vsspsConnection.AuthorizedIdentity.Id, cancellationToken: cancellationToken);
            foreach (Account account in accounts
                .Where(a => a.AccountStatus == AccountStatus.None || a.AccountStatus == AccountStatus.Enabled)
                .OrderBy(a => a.AccountName))
            {
                results.Add(new AdoAccount()
                {
                    Name = account.AccountName,
                });
            }
        }

        return results;
    }

    public static async Task UpdateProjects(AdoConnection connection, AdoAccount account, CancellationToken cancellationToken)
    {
        AdoConnectionInternal connectionInternal = (AdoConnectionInternal)connection;

        if (connectionInternal.Connection == null || connectionInternal.AccountName != account.Name)
        {
            //connectionInternal.Connection = new VssConnection();
        }

        await Task.CompletedTask;
    }
}
