using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalDrawerContent
{
    protected override string DefaultClass => "min-h-0 flex-1 overflow-y-auto px-4 py-3";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
