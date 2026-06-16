using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalDrawerHeader
{
    protected override string ComponentClass => "kal-drawer-header";

    protected override string DefaultClass => "shrink-0";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [CascadingParameter]
    internal KalDrawerContext? DrawerContext { get; set; }

    protected override string DynamicClass => DrawerContext?.HasAppBar == true ? "min-h-14" : string.Empty;
}
