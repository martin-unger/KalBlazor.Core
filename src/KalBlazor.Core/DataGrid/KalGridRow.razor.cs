using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalGridRow
{
    protected override string ComponentClass => "kal-grid-row";

    protected override string DefaultClass => "transition-colors hover:bg-slate-50";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
