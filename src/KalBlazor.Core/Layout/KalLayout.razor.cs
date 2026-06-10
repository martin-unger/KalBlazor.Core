using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalLayout : IDisposable
{
    protected override string ComponentClass => "kal-layout";

    protected override string DefaultClass => "min-h-screen w-full flex flex-col bg-white text-slate-950";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private KalDrawerContext DrawerContext { get; } = new();

    private string DrawerOverlayClass =>
        DrawerContext.ActiveBackdrop switch
        {
            KalDrawerBackdrop.Darkened => $"fixed inset-0 {DrawerOverlayZIndexClass} bg-black/40 opacity-100 transition-opacity duration-200",
            KalDrawerBackdrop.Transparent => $"fixed inset-0 {DrawerOverlayZIndexClass} bg-transparent opacity-100 transition-opacity duration-200",
            _ => "pointer-events-none fixed inset-0 z-30 bg-transparent opacity-0 transition-opacity duration-200"
        };

    private string DrawerOverlayZIndexClass =>
        DrawerContext.HasOpenOverlayDrawerWithBackdrop ? "z-40" : "z-30";

    protected override void OnInitialized()
    {
        DrawerContext.StateChanged += StateHasChanged;
    }

    private void CloseDrawers()
    {
        DrawerContext.CloseAll();
    }

    public void Dispose()
    {
        DrawerContext.StateChanged -= StateHasChanged;
    }
}
