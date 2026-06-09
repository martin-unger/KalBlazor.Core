using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core.DataGrid;

public partial class KalGridBody
{
    protected override string ComponentClass => "kal-grid-body";

    protected override string DefaultClass => "divide-y divide-slate-200 bg-white";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
