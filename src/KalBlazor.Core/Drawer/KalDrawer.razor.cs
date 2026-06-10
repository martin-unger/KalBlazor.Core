using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalDrawer : IDisposable
{
    protected override string ComponentClass =>
        $"kal-drawer {(IsMinimized ? "kal-drawer-minimized" : IsOpen ? "kal-drawer-open" : "kal-drawer-closed")}";

    protected override string DefaultClass => "fixed flex w-screen flex-col overflow-x-hidden overflow-y-auto bg-white text-slate-950 transition-[width,max-width,transform,translate] duration-200 ease-out";

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

    [Parameter]
    public KalDrawerBackdrop Backdrop { get; set; } = KalDrawerBackdrop.Darkened;

    [Parameter]
    public bool PreventClose { get; set; }

    [Parameter]
    public bool Minimizable { get; set; }

    [Parameter]
    public bool MinimizeOnClose { get; set; }

    [Parameter]
    public EventCallback<bool> MinimizeOnCloseChanged { get; set; }

    [Parameter]
    public string MinimizedWidthClass { get; set; } = "!w-16 !max-w-16";

    [Parameter]
    public string MinimizedOffsetClass { get; set; } = "ml-16 mr-16 left-16 right-16";

    [Parameter]
    public string MinimizeIcon { get; set; } = Icons.AngleLeft;

    [Parameter]
    public string MinimizeButtonAriaLabel { get; set; } = "Drawer minimieren";

    [Parameter]
    public string RestoreButtonAriaLabel { get; set; } = "Drawer erweitern";

    [Parameter]
    public string MinimizeButtonClass { get; set; } = "m-2 inline-flex size-10 shrink-0 items-center justify-center self-end rounded hover:bg-slate-100";

    [Parameter]
    public string MinimizeIconClass { get; set; } = "text-xl transition-transform duration-200";

    [Parameter]
    public RenderFragment? MinimizeButtonContent { get; set; }

    [CascadingParameter]
    internal KalDrawerContext? DrawerContext { get; set; }

    protected override string DynamicClass => $"{SideClass} {VerticalClass} {ZIndexClass} {WidthClass} {ShadowClass} {StateClass}".Trim();

    private bool IsOpen => DrawerContext?.IsOpen(Key) == true;

    private bool IsMinimized => DrawerContext?.IsMinimized(Key) == true;

    private bool IsVisible => IsOpen || IsMinimized;

    private bool IsHidden => !IsVisible;

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

    private string WidthClass =>
        IsMinimized
            ? MinimizedWidthClass
            : Width switch
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
            if (IsVisible)
            {
                return "translate-x-0";
            }

            return Side == KalDrawerSide.Right ? "translate-x-full" : "-translate-x-full";
        }
    }

    private string MinimizeButtonCssClass =>
        $"{MinimizeButtonClass} {(IsMinimized ? "self-center" : string.Empty)}".Trim();

    private string EffectiveMinimizeButtonAriaLabel =>
        IsMinimized ? RestoreButtonAriaLabel : MinimizeButtonAriaLabel;

    private string EffectiveMinimizeIconClass =>
        $"{MinimizeIconClass} {(IsMinimized ? "rotate-180" : string.Empty)}".Trim();

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
            DrawerContext.Register(
                key,
                Side,
                Width,
                Variant,
                Backdrop,
                PreventClose,
                Minimizable || MinimizeOnClose,
                MinimizeOnClose,
                MinimizedOffsetClass);
        }
    }

    private void ToggleMinimized()
    {
        if (DrawerContext is null || !TryGetKey(out var key))
        {
            return;
        }

        DrawerContext.ToggleMinimized(key);
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
