namespace MyDash.Data.Model;

public sealed class AdoProject
{
    public string Name { get; set; }

    public override string ToString()
    {
        return this.Name;
    }
}
