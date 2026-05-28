namespace SoftwareThingies.KalBlazor.Core;

internal static class KalDataGridResponsiveClass
{
    public static string HideBelow(KalDataGridBreakpoint breakpoint)
    {
        return breakpoint switch
        {
            KalDataGridBreakpoint.Sm => "hidden sm:table-cell",
            KalDataGridBreakpoint.Md => "hidden md:table-cell",
            KalDataGridBreakpoint.Lg => "hidden lg:table-cell",
            KalDataGridBreakpoint.Xl => "hidden xl:table-cell",
            KalDataGridBreakpoint.TwoXl => "hidden 2xl:table-cell",
            _ => string.Empty
        };
    }

    public static string ShowBelow(KalDataGridBreakpoint breakpoint)
    {
        return breakpoint switch
        {
            KalDataGridBreakpoint.Sm => "sm:hidden",
            KalDataGridBreakpoint.Md => "md:hidden",
            KalDataGridBreakpoint.Lg => "lg:hidden",
            KalDataGridBreakpoint.Xl => "xl:hidden",
            KalDataGridBreakpoint.TwoXl => "2xl:hidden",
            _ => string.Empty
        };
    }

    public static string TextAlignment(KalDataGridAlignment alignment)
    {
        return alignment switch
        {
            KalDataGridAlignment.Center => "text-center",
            KalDataGridAlignment.End => "text-right",
            _ => "text-left"
        };
    }
}
