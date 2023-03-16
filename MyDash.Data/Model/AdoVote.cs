using System;

namespace MyDash.Data.Model;

public enum AdoVoteType
{
    Approved = 10,
    ApprovedWithSuggestions = 5,
    NoVote = 0,
    WaitingForAuthor = -5,
    Rejected = -10,
};

public sealed class AdoVote : IComparable, IComparable<AdoVote>, IEquatable<AdoVote>
{
    public string Reviewer { get; set; }
    public bool Declined { get; set; }
    public bool Required { get; set; }
    public bool Flagged { get; set; }
    public AdoVoteType VoteType { get; set; }

    public override string ToString()
    {
        return this.Reviewer;
    }

    public override bool Equals(object obj)
    {
        return obj is AdoVote other && this.Equals(other);
    }

    public bool Equals(AdoVote other)
    {
        return this.Reviewer == other.Reviewer;
    }

    public override int GetHashCode()
    {
        return this.Reviewer.GetHashCode();
    }

    public int CompareTo(AdoVote other)
    {
        return this.Reviewer.CompareTo(other.Reviewer);
    }

    public int CompareTo(object obj)
    {
        if (obj is not AdoVote other)
        {
            throw new InvalidOperationException();
        }

        return this.CompareTo(other);
    }
}
