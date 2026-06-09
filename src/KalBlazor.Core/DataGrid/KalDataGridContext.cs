namespace SoftwareThingies.KalBlazor.Core.DataGrid;

internal sealed class KalDataGridContext<TItem>
{
    private readonly List<KalGridColumn<TItem>> _columns = [];

    public event Action? StateChanged;

    public IReadOnlyList<KalGridColumn<TItem>> Columns => _columns;

    public void Register(KalGridColumn<TItem> column)
    {
        if (!_columns.Contains(column))
        {
            _columns.Add(column);
            StateChanged?.Invoke();
        }
    }

    public void Unregister(KalGridColumn<TItem> column)
    {
        if (_columns.Remove(column))
        {
            StateChanged?.Invoke();
        }
    }
}
