using Microsoft.TeamFoundation.SourceControl.WebApi;
using System;
using System.Collections.ObjectModel;

namespace MyDash.Data.Model;

public sealed class AdoPullRequest : IComparable, IComparable<AdoPullRequest>, IEquatable<AdoPullRequest>
{
    public int Id { get; set; }
    public Guid RepoId { get; set; }
    public PullRequestStatus Status { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ClosedDate { get; set; }
    public string CreatedBy { get; set; }
    public string ClosedBy { get; set; }
    public string AutoCompleteSetBy { get; set; }
    public string Title { get; set; }
    public string SourceRefName { get; set; }
    public string TargetRefName { get; set; }
    public bool IsDraft { get; set; }
    public ObservableCollection<AdoVote> Votes { get; } = new();

    public override string ToString()
    {
        return this.Title;
    }

    public override bool Equals(object obj)
    {
        return obj is AdoPullRequest other && this.Equals(other);
    }

    public bool Equals(AdoPullRequest other)
    {
        return this.Id == other.Id;
    }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }

    public int CompareTo(AdoPullRequest other)
    {
        return this.Id.CompareTo(other.Id);
    }

    public int CompareTo(object obj)
    {
        if (obj is not AdoPullRequest other)
        {
            throw new InvalidOperationException();
        }

        return this.CompareTo(other);
    }
}
