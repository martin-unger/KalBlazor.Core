using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalDrawerStateView
{
    [Parameter]
    public RenderFragment<KalDrawerStateContext> ChildContent { get; set; } = default!;

    [CascadingParameter]
    public KalDrawerStateContext? DrawerState { get; set; }
}
