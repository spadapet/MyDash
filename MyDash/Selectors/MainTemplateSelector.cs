using Microsoft.Maui.Controls;
using MyDash.Data.Model;

namespace MyDash.Selectors;

internal sealed class MainTemplateSelector : DataTemplateSelector
{
    public DataTemplate PullRequestsTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        return (AppState)item switch
        {
            AppState.PullRequests => this.PullRequestsTemplate,
            _ => null
        };
    }
}
