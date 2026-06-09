namespace SoftwareThingies.KalBlazor.Core.DataGrid;

internal interface IKalDataGridFilterContext
{
    string? FilterText { get; }

    Task SetFilterTextAsync(string? filterText);

    Task ClearFilterTextAsync();
}
