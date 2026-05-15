using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalDrawerTrigger
{
    protected override string ComponentClass => "kal-drawer-trigger";

    protected override string DefaultClass => "inline-flex items-center justify-center";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? TargetId { get; set; }

    [Parameter]
    public string? TargetName { get; set; }

    [Parameter]
    public IReadOnlyList<string>? TargetIds { get; set; }

    [Parameter]
    public IReadOnlyList<string>? TargetNames { get; set; }

    [Parameter]
    public KalDrawerTriggerAction Action { get; set; } = KalDrawerTriggerAction.Toggle;

    [CascadingParameter]
    internal KalDrawerContext? DrawerContext { get; set; }

    private async Task TriggerAsync()
    {
        if (DrawerContext is null)
        {
            return;
        }

        foreach (var key in GetTargetKeys())
        {
            switch (Action)
            {
                case KalDrawerTriggerAction.Open:
                    DrawerContext.Open(key);
                    break;
                case KalDrawerTriggerAction.Close:
                    DrawerContext.Close(key);
                    break;
                default:
                    DrawerContext.Toggle(key);
                    break;
            }
        }

        await Task.CompletedTask;
    }

    private IEnumerable<string> GetTargetKeys()
    {
        if (!string.IsNullOrWhiteSpace(TargetId))
        {
            yield return $"id:{TargetId}";
        }

        if (!string.IsNullOrWhiteSpace(TargetName))
        {
            yield return $"name:{TargetName}";
        }

        if (TargetIds is not null)
        {
            foreach (var targetId in TargetIds.Where(targetId => !string.IsNullOrWhiteSpace(targetId)))
            {
                yield return $"id:{targetId}";
            }
        }

        if (TargetNames is not null)
        {
            foreach (var targetName in TargetNames.Where(targetName => !string.IsNullOrWhiteSpace(targetName)))
            {
                yield return $"name:{targetName}";
            }
        }
    }
}
