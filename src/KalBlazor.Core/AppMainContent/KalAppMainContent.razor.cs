using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalAppMainContent : IDisposable
{
    protected override string ComponentClass => "kal-app-main-content";

    protected override string DefaultClass => "w-full mx-auto px-4 transition-[margin-left,margin-right] duration-200 ease-out";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [CascadingParameter]
    internal KalDrawerContext? DrawerContext { get; set; }

    protected override string DynamicClass =>
        DrawerContext is null
            ? string.Empty
            : $"{AppBarOffsetClass} {KalDrawerOffsetClass.GetMarginLeft(DrawerContext.LeftContentOffsetWidth)} {KalDrawerOffsetClass.GetMarginRight(DrawerContext.RightContentOffsetWidth)}".Trim();

    private string AppBarOffsetClass
    {
        get
        {
            if (DrawerContext is null)
            {
                return string.Empty;
            }

            return $"{(DrawerContext.HasFixedTopAppBar ? "pt-14" : string.Empty)} {(DrawerContext.HasFixedBottomAppBar ? "pb-14" : string.Empty)}".Trim();
        }
    }

    protected override void OnInitialized()
    {
        if (DrawerContext is not null)
        {
            DrawerContext.StateChanged += StateHasChanged;
        }
    }

    public void Dispose()
    {
        if (DrawerContext is not null)
        {
            DrawerContext.StateChanged -= StateHasChanged;
        }
    }
}
