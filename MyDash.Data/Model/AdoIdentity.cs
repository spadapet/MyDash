using System;

namespace MyDash.Data.Model;

public sealed class AdoIdentity : IComparable, IComparable<AdoIdentity>, IEquatable<AdoIdentity>
{
    public string DisplayName { get; set; }
    public string Url { get; set; }
    public Uri Avatar { get; set; }
    public string Id { get; set; }

    public override string ToString()
    {
        return this.Id;
    }

    public override bool Equals(object obj)
    {
        return obj is AdoIdentity other && this.Equals(other);
    }

    public bool Equals(AdoIdentity other)
    {
        return this.Id == other.Id;
    }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }

    public int CompareTo(AdoIdentity other)
    {
        return this.Id.CompareTo(other.Id);
    }

    public int CompareTo(object obj)
    {
        if (obj is not AdoIdentity other)
        {
            throw new InvalidOperationException();
        }

        return this.CompareTo(other);
    }
}
