using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalDrawer : IDisposable
{
    protected override string ComponentClass => "kal-drawer";

    protected override string DefaultClass => "fixed flex w-screen flex-col overflow-y-auto bg-white text-slate-950 transition-transform duration-200 ease-out";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public KalDrawerSide Side { get; set; } = KalDrawerSide.Left;

    [Parameter]
    public DrawerWidth Width { get; set; } = DrawerWidth.Xs;

    [Parameter]
    public KalDrawerVariant Variant { get; set; } = KalDrawerVariant.Overlay;

    [CascadingParameter]
    internal KalDrawerContext? DrawerContext { get; set; }

    protected override string AdditionalClass => $"{SideClass} {VerticalClass} {ZIndexClass} {WidthClass} {ShadowClass} {StateClass}".Trim();

    private bool IsOpen => DrawerContext?.IsOpen(Key) == true;

    private bool IsHidden => !IsOpen;

    private string Key => TryGetKey(out var key) ? key : string.Empty;

    private string SideClass => Side == KalDrawerSide.Right ? "right-0" : "left-0";

    private string VerticalClass
    {
        get
        {
            if (Variant != KalDrawerVariant.Clipped)
            {
                return "top-0 bottom-0";
            }

            return DrawerContext?.AppBarBottom == true
                ? "top-0 bottom-14"
                : "top-14 bottom-0";
        }
    }

    private string ZIndexClass => Variant == KalDrawerVariant.Clipped ? "z-40" : "z-50";

    private string ShadowClass => "shadow-xl";

    private string WidthClass => Width switch
    {
        DrawerWidth.Sm => "max-w-sm",
        DrawerWidth.Md => "max-w-md",
        DrawerWidth.Lg => "max-w-lg",
        DrawerWidth.Xl => "max-w-xl",
        DrawerWidth.TwoXl => "max-w-2xl",
        DrawerWidth.ThreeXl => "max-w-3xl",
        DrawerWidth.FourXl => "max-w-4xl",
        DrawerWidth.FiveXl => "max-w-5xl",
        DrawerWidth.SixXl => "max-w-6xl",
        DrawerWidth.SevenXl => "max-w-7xl",
        DrawerWidth.Full => "max-w-full",
        _ => "max-w-xs"
    };

    private string StateClass
    {
        get
        {
            if (IsOpen)
            {
                return "translate-x-0";
            }

            return Side == KalDrawerSide.Right ? "translate-x-full" : "-translate-x-full";
        }
    }

    protected override void OnInitialized()
    {
        if (DrawerContext is not null)
        {
            DrawerContext.StateChanged += StateHasChanged;
        }
    }

    protected override void OnParametersSet()
    {
        if (DrawerContext is not null && TryGetKey(out var key))
        {
            DrawerContext.Register(key, Side, Width, Variant);
        }
    }

    private bool TryGetKey(out string key)
    {
        if (!string.IsNullOrWhiteSpace(Id))
        {
            key = $"id:{Id}";
            return true;
        }

        if (!string.IsNullOrWhiteSpace(Name))
        {
            key = $"name:{Name}";
            return true;
        }

        key = string.Empty;
        return false;
    }

    public void Dispose()
    {
        if (DrawerContext is not null)
        {
            DrawerContext.StateChanged -= StateHasChanged;

            if (TryGetKey(out var key))
            {
                DrawerContext.Unregister(key);
            }
        }
    }
}
