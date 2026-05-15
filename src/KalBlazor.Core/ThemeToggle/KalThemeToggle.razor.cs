using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace SoftwareThingies.KalBlazor.Core;

public partial class KalThemeToggle : IAsyncDisposable
{
    protected override string ComponentClass => "kal-theme-toggle";

    protected override string DefaultClass => "inline-flex items-center gap-3";

    [Inject]
    private IJSRuntime JsRuntime { get; set; } = default!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string AriaLabel { get; set; } = "Dark Mode umschalten";

    private IJSObjectReference? Module { get; set; }

    private bool IsDarkMode { get; set; }

    private string IsDarkModeText => IsDarkMode ? "true" : "false";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        Module = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/SoftwareThingies.KalBlazor.Core/kalTheme.js");

        IsDarkMode = await Module.InvokeAsync<bool>("isDarkMode");
        StateHasChanged();
    }

    private async Task ToggleAsync()
    {
        if (Module is null)
        {
            return;
        }

        IsDarkMode = !IsDarkMode;
        await Module.InvokeVoidAsync("setDarkMode", IsDarkMode);
    }

    public async ValueTask DisposeAsync()
    {
        if (Module is not null)
        {
            await Module.DisposeAsync();
        }
    }
}
