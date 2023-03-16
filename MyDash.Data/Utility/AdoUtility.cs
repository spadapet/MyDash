using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Account;
using Microsoft.VisualStudio.Services.Account.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.OAuth;
using Microsoft.VisualStudio.Services.WebApi;
using MyDash.Data.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        model.Accounts.SortedMerge(accounts, replaceEqualItems: false);
        model.CurrentAccountName = currentAccountName ?? defaultAccountName;
    }

    public static async Task UpdateProjectsAsync(AdoConnection connection, AdoAccount account, CancellationToken cancellationToken)
    {
        string currentProjectName = account.CurrentProjectName;
        var (projects, defaultProjectName) = await AdoUtility.GetProjectsAsync(connection, account, cancellationToken);
        account.Projects.SortedMerge(projects, replaceEqualItems: false);
        account.CurrentProjectName = currentProjectName ?? defaultProjectName;
    }

    public static async Task<ObservableCollection<AdoPullRequest>> UpdatePullRequestsAsync(PullRequestsType type, AdoConnection connection, AdoAccount account, AdoProject project, CancellationToken cancellationToken)
    {
        GitPullRequestSearchCriteria search = new()
        {
            Status = PullRequestStatus.Active,
        };

        List<AdoPullRequest> newPrs = await AdoUtility.GetPullRequestsAsync(connection, account, project, search, cancellationToken);
        ObservableCollection<AdoPullRequest> prs = project.EnsurePullRequests(type);
        prs.SortedMerge(newPrs, replaceEqualItems: true);
        return prs;
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

    private static async Task<List<AdoPullRequest>> GetPullRequestsAsync(AdoConnection connection, AdoAccount account, AdoProject project, GitPullRequestSearchCriteria search, CancellationToken cancellationToken)
    {
        List<AdoPullRequest> results = new();

        GitHttpClient gitClient = await connection.Connect(account).GetClientAsync<GitHttpClient>(cancellationToken);
        foreach (GitPullRequest pr in await gitClient.GetPullRequestsByProjectAsync(project.Id, search, cancellationToken: cancellationToken))
        {
            AdoPullRequest adoPr = new()
            {
                Id = pr.PullRequestId,
                RepoId = pr.Repository.Id,
                Status = pr.Status,
                CreationDate = pr.CreationDate,
                ClosedDate = pr.ClosedDate,
                CreatedBy = pr.CreatedBy.Id,
                ClosedBy = pr.ClosedBy?.Id,
                AutoCompleteSetBy = pr.AutoCompleteSetBy?.Id,
                Title = pr.Title,
                SourceRefName = pr.SourceRefName,
                TargetRefName = pr.TargetRefName,
                IsDraft = pr.IsDraft.HasValue && pr.IsDraft.Value,
            };

            foreach (IdentityRefWithVote vote in pr.Reviewers)
            {
                adoPr.Votes.Add(new()
                {
                    Reviewer = vote.Id,
                    VoteType = (AdoVoteType)vote.Vote,
                    Declined = vote.HasDeclined.HasValue && vote.HasDeclined.Value,
                    Flagged = vote.IsFlagged.HasValue && vote.IsFlagged.Value,
                    Required = vote.IsRequired,
                });
            }

            results.Add(adoPr);
        }

        return results;
    }
}
