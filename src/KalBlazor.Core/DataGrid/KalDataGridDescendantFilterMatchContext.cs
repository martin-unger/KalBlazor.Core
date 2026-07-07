namespace SoftwareThingies.KalBlazor.Core.DataGrid;

internal sealed class KalDataGridDescendantFilterMatchContext(Action<bool> reportMatch)
{
    public void ReportMatch(bool hasMatch)
    {
        reportMatch(hasMatch);
    }
}

