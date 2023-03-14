using System.Collections.ObjectModel;
using System.Linq;

namespace MyDash.Data.Model;

public sealed class AdoAccount : PropertyNotifier
{
    public string Name { get; set; }
    public ObservableCollection<AdoProject> Projects { get; } = new();

    private AdoProject currentProject;
    public AdoProject CurrentProject
    {
        get => (this.currentProject != null && this.Projects.Contains(this.currentProject))
            ? this.currentProject
            : this.Projects.FirstOrDefault();

        set => this.SetProperty(ref this.currentProject, value);
    }

    public override string ToString()
    {
        return this.Name;
    }
}
