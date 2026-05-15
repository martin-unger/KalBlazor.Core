using Microsoft.AspNetCore.Components;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalAppMainContent : IDisposable
{
    protected override string ComponentClass => "kal-app-main-content";

    protected override string DefaultClass => "mx-auto px-4 transition-[margin-left,margin-right] duration-200 ease-out";

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [CascadingParameter]
    internal KalDrawerContext? DrawerContext { get; set; }

    protected override string DynamicClass =>
        DrawerContext is null
            ? string.Empty
            : $"{KalDrawerOffsetClass.GetMarginLeft(DrawerContext.LeftContentOffsetWidth)} {KalDrawerOffsetClass.GetMarginRight(DrawerContext.RightContentOffsetWidth)}".Trim();

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
