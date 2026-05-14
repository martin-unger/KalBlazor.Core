using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalDrawerHeader
{
    protected override string DefaultClass => "shrink-0 border-b border-slate-200 px-4 py-3";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [CascadingParameter]
    internal KalDrawerContext? DrawerContext { get; set; }

    protected override string AdditionalClass => DrawerContext?.HasAppBar == true ? "min-h-14" : string.Empty;
}
