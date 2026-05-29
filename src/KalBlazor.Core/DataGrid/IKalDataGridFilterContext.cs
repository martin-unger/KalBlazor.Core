namespace SoftwareThingies.KalBlazor.Core;

internal interface IKalDataGridFilterContext
{
    string? FilterText { get; }

    Task SetFilterTextAsync(string? filterText);

    Task ClearFilterTextAsync();
}
