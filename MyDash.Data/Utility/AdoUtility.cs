using Microsoft.TeamFoundation.Core.WebApi;
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
    public static async Task UpdateAccountsAsync(this AdoModel model, CancellationToken cancellationToken)
    {
        string currentAccountName = model.CurrentAccountName;
        var (accounts, defaultAccountName) = await AdoUtility.GetAccountsAsync(model.Connection, cancellationToken);
        model.Accounts.SortedMerge(accounts);
        model.CurrentAccountName = currentAccountName ?? defaultAccountName;
    }

    public static async Task UpdateProjectsAsync(AdoConnection connection, AdoAccount account, CancellationToken cancellationToken)
    {
        string currentProjectName = account.CurrentProjectName;
        var (projects, defaultProjectName) = await AdoUtility.GetProjectsAsync(connection, account, cancellationToken);
        account.Projects.SortedMerge(projects);
        account.CurrentProjectName = currentProjectName ?? defaultProjectName;
    }

    private static async Task<(List<AdoAccount>, string defaultAccountName)> GetAccountsAsync(AdoConnection connection, CancellationToken cancellationToken)
    {
        List<AdoAccount> results = new();

        using (VssConnection vsspsConnection = new(new Uri("https://app.vssps.visualstudio.com"), new VssOAuthAccessTokenCredential(connection.AccessToken)))
        {
            AccountHttpClient accountsClient = await vsspsConnection.GetClientAsync<AccountHttpClient>(cancellationToken);
            IEnumerable<Account> accounts = await accountsClient.GetAccountsByMemberAsync(vsspsConnection.AuthorizedIdentity.Id, cancellationToken: cancellationToken);
            foreach (Account account in accounts.Where(a => a.AccountStatus == AccountStatus.None || a.AccountStatus == AccountStatus.Enabled))
            {
                results.Add(new()
                {
                    Id = account.AccountId,
                    Name = account.AccountName,
                    Uri = account.AccountUri,
                });
            }
        }

        string defaultAccountName = results.FirstOrDefault()?.Name;
        results.Sort();
        return (results, defaultAccountName);
    }

    private static async Task<(List<AdoProject>, string defaultProjectName)> GetProjectsAsync(AdoConnection connection, AdoAccount account, CancellationToken cancellationToken)
    {
        List<AdoProject> results = new();
        ProjectHttpClient projectClient = await connection.Connect(account).GetClientAsync<ProjectHttpClient>(cancellationToken);

        IPagedList<TeamProjectReference> page = null;
        do
        {
            page = await projectClient.GetProjects(continuationToken: page?.ContinuationToken);

            foreach (TeamProjectReference project in page.Where(p => p.State == ProjectState.WellFormed))
            {
                results.Add(new()
                {
                    Id = project.Id,
                    Abbreviation = project.Abbreviation,
                    Name = project.Name,
                    Description = project.Description,
                    Url = project.Url,
                    DefaultTeamImageUrl = project.DefaultTeamImageUrl,
                });
            }
        }
        while (page.ContinuationToken != null);

        string defaultProjectName = results.FirstOrDefault()?.Name;
        results.Sort();
        return (results, defaultProjectName);
    }
}
