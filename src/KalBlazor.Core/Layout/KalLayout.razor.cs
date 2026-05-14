using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalLayout : IDisposable
{
    protected override string DefaultClass => "min-h-screen w-full flex flex-col bg-white text-slate-950";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private KalDrawerContext DrawerContext { get; } = new();

    private string DrawerOverlayClass =>
        DrawerContext.HasOpenOverlayDrawers
            ? "fixed inset-0 z-40 bg-black/40 opacity-100 transition-opacity duration-200"
            : DrawerContext.HasOpenDrawers
                ? "fixed inset-0 z-30 bg-black/40 opacity-100 transition-opacity duration-200"
                : "pointer-events-none fixed inset-0 z-30 bg-black/40 opacity-0 transition-opacity duration-200";

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
