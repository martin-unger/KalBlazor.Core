using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalDrawerMaximizerTrigger
{
    protected override string ComponentClass => "kal-drawer-maximizer-trigger";

    protected override string DefaultClass => "inline-flex items-center justify-center";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string AriaLabel { get; set; } = "Drawer maximieren";

    [CascadingParameter]
    internal KalDrawer? Drawer { get; set; }
}
