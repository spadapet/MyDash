using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyDash.Data.Model;

public sealed class AdoProject : IComparable, IComparable<AdoProject>, IEquatable<AdoProject>
{
    public Guid Id { get; set; }
    public string Abbreviation { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public string DefaultTeamImageUrl { get; set; }

    public Dictionary<PullRequestsType, ObservableCollection<AdoPullRequest>> PullRequests { get; } = new();

    public ObservableCollection<AdoPullRequest> EnsurePullRequests(PullRequestsType type)
    {
        if (!this.PullRequests.TryGetValue(type, out var prs))
        {
            this.PullRequests[type] = prs = new();
        }

        return prs;
    }

    public override string ToString()
    {
        return this.Name;
    }

    public override bool Equals(object obj)
    {
        return obj is AdoProject other && this.Equals(other);
    }

    public bool Equals(AdoProject other)
    {
        return string.Equals(this.Name, other.Name);
    }

    public override int GetHashCode()
    {
        return this.Name?.GetHashCode() ?? 0;
    }

    public int CompareTo(AdoProject other)
    {
        return this.Name.CompareTo(other.Name);
    }

    public int CompareTo(object obj)
    {
        if (obj is not AdoProject other)
        {
            throw new InvalidOperationException();
        }

        return this.CompareTo(other);
    }
}
