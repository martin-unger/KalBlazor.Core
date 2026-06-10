using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalAppBar : IDisposable
{
    private readonly Guid _appBarKey = Guid.NewGuid();

    protected override string ComponentClass => $"kal-app-bar {AppBarStateClass}";

    protected override string DefaultClass => "w-full min-h-14 px-4 flex flex-wrap items-center justify-between gap-x-3 bg-[var(--kal-app-bar-background,var(--color-amber-300))] text-[var(--kal-app-bar-foreground,var(--color-slate-950))] border-[var(--kal-app-bar-border,var(--color-slate-200))] shadow-sm transition-[margin-left,margin-right,left,right] duration-200 ease-out";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool Bottom { get; set; }

    [Parameter]
    public bool Fixed { get; set; }

    [CascadingParameter]
    internal KalDrawerContext? DrawerContext { get; set; }

    private KalAppBarContext AppBarContext { get; } = new();

    protected override string DynamicClass => $"{PositionClass} {WrapClass} {DrawerMarginClass} {FixedClass}".Trim();

    private bool IsFixedToViewport => Fixed || Bottom;

    private string AppBarStateClass =>
        $"{(IsFixedToViewport ? "kal-app-bar-fixed" : string.Empty)} {(Bottom ? "kal-app-bar-bottom" : "kal-app-bar-top")}".Trim();

    private string PositionClass
    {
        get
        {
            return Bottom
                ? "mt-auto bottom-0 border-t"
                : "top-0 border-b";
        }
    }

    private string WrapClass => Bottom ? "content-end pb-2 pt-0 md:content-center md:py-0" : "content-start pb-0 pt-2 md:content-center md:py-0";

    private string FixedClass => IsFixedToViewport ? $"fixed {AppBarZIndexClass} {DrawerInsetClass}" : string.Empty;

    private string AppBarZIndexClass => DrawerContext?.HasOpenOverlayDrawers == true ? "z-30" : "z-50";

    private string DrawerMarginClass =>
        Fixed || Bottom || DrawerContext is null
            ? string.Empty
            : $"{KalDrawerOffsetClass.GetMarginLeft(DrawerContext.LeftDockedOffset)} {KalDrawerOffsetClass.GetMarginRight(DrawerContext.RightDockedOffset)}".Trim();

    private string DrawerInsetClass =>
        DrawerContext is null
            ? "left-0 right-0"
            : $"{KalDrawerOffsetClass.GetInsetLeft(DrawerContext.LeftDockedOffset)} {KalDrawerOffsetClass.GetInsetRight(DrawerContext.RightDockedOffset)}".Trim();

    protected override void OnInitialized()
    {
        AppBarContext.StateChanged = EventCallback.Factory.Create(this, StateHasChanged);

        if (DrawerContext is not null)
        {
            DrawerContext.StateChanged += StateHasChanged;
        }
    }

    protected override void OnParametersSet()
    {
        AppBarContext.Bottom = Bottom;
        DrawerContext?.RegisterAppBar(_appBarKey, Bottom, IsFixedToViewport);
    }

    public void Dispose()
    {
        if (DrawerContext is not null)
        {
            DrawerContext.StateChanged -= StateHasChanged;
            DrawerContext.UnregisterAppBar(_appBarKey);
        }
    }
}
