using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalThemeToggle
{
    protected override string ComponentClass => "kal-theme-toggle";

    protected override string DefaultClass => "inline-flex items-center gap-3";

    [Inject]
    private IJSRuntime JsRuntime { get; set; } = default!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string AriaLabel { get; set; } = "Dark Mode umschalten";

    private bool IsDarkMode { get; set; }

    private string IsDarkModeText => IsDarkMode ? "true" : "false";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        IsDarkMode = await JsRuntime.InvokeAsync<bool>("document.documentElement.classList.contains", "dark");
        StateHasChanged();
    }

    private async Task ToggleAsync()
    {
        IsDarkMode = !IsDarkMode;

        if (IsDarkMode)
        {
            await JsRuntime.InvokeVoidAsync("document.documentElement.classList.add", "dark");
            return;
        }

        await JsRuntime.InvokeVoidAsync("document.documentElement.classList.remove", "dark");
    }
}
