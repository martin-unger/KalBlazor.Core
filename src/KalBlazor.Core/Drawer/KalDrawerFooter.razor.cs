using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalDrawerFooter
{
    protected override string ComponentClass => "kal-drawer-footer";

    protected override string DefaultClass => "shrink-0 border-t border-slate-200 px-4 py-3";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
