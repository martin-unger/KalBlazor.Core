using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalDrawerFooter
{
    protected override string ComponentClass => "kal-drawer-footer";

    protected override string DefaultClass => "shrink-0";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
