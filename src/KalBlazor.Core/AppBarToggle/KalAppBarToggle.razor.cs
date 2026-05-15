using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalAppBarToggle
{
    protected override string ComponentClass => "kal-app-bar-toggle";

    protected override string DefaultClass => "relative order-10 inline-flex h-10 w-10 shrink-0 items-center justify-center self-start rounded-md text-slate-950 md:hidden ";

    [Parameter]
    public string Label { get; set; } = "Navigation umschalten";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public KalHorizontalAlignment HorizontalAlignment { get; set; } = KalHorizontalAlignment.Left;

    [CascadingParameter]
    internal KalAppBarContext? AppBarContext { get; set; }

    protected override string AdditionalClass => HorizontalAlignment == KalHorizontalAlignment.Right ? "ml-auto justify-self-end" : "mr-auto justify-self-start";

    protected override void OnParametersSet()
    {
        if (AppBarContext is not null)
        {
            AppBarContext.ToggleHorizontalAlignment = HorizontalAlignment;
        }
    }

    private string IsExpandedText => AppBarContext?.IsOpen == true ? "true" : "false";

    private async Task ToggleAsync()
    {
        if (AppBarContext is null)
        {
            return;
        }

        AppBarContext.IsOpen = !AppBarContext.IsOpen;

        if (AppBarContext.StateChanged.HasDelegate)
        {
            await AppBarContext.StateChanged.InvokeAsync();
        }
    }
}
