namespace SoftwareThingies.KalBlazor.Core;

internal static class KalDrawerOffsetClass
{
    public static string GetMarginLeft(DrawerWidth? width)
    {
        return width switch
        {
            DrawerWidth.Xs => "ml-[var(--container-xs)]",
            DrawerWidth.Sm => "ml-[var(--container-sm)]",
            DrawerWidth.Md => "ml-[var(--container-md)]",
            DrawerWidth.Lg => "ml-[var(--container-lg)]",
            DrawerWidth.Xl => "ml-[var(--container-xl)]",
            DrawerWidth.TwoXl => "ml-[var(--container-2xl)]",
            DrawerWidth.ThreeXl => "ml-[var(--container-3xl)]",
            DrawerWidth.FourXl => "ml-[var(--container-4xl)]",
            DrawerWidth.FiveXl => "ml-[var(--container-5xl)]",
            DrawerWidth.SixXl => "ml-[var(--container-6xl)]",
            DrawerWidth.SevenXl => "ml-[var(--container-7xl)]",
            DrawerWidth.Full => "ml-[100%]",
            _ => string.Empty
        };
    }

    public static string GetMarginRight(DrawerWidth? width)
    {
        return width switch
        {
            DrawerWidth.Xs => "mr-[var(--container-xs)]",
            DrawerWidth.Sm => "mr-[var(--container-sm)]",
            DrawerWidth.Md => "mr-[var(--container-md)]",
            DrawerWidth.Lg => "mr-[var(--container-lg)]",
            DrawerWidth.Xl => "mr-[var(--container-xl)]",
            DrawerWidth.TwoXl => "mr-[var(--container-2xl)]",
            DrawerWidth.ThreeXl => "mr-[var(--container-3xl)]",
            DrawerWidth.FourXl => "mr-[var(--container-4xl)]",
            DrawerWidth.FiveXl => "mr-[var(--container-5xl)]",
            DrawerWidth.SixXl => "mr-[var(--container-6xl)]",
            DrawerWidth.SevenXl => "mr-[var(--container-7xl)]",
            DrawerWidth.Full => "mr-[100%]",
            _ => string.Empty
        };
    }

    public static string GetInsetLeft(DrawerWidth? width)
    {
        return width switch
        {
            DrawerWidth.Xs => "left-[var(--container-xs)]",
            DrawerWidth.Sm => "left-[var(--container-sm)]",
            DrawerWidth.Md => "left-[var(--container-md)]",
            DrawerWidth.Lg => "left-[var(--container-lg)]",
            DrawerWidth.Xl => "left-[var(--container-xl)]",
            DrawerWidth.TwoXl => "left-[var(--container-2xl)]",
            DrawerWidth.ThreeXl => "left-[var(--container-3xl)]",
            DrawerWidth.FourXl => "left-[var(--container-4xl)]",
            DrawerWidth.FiveXl => "left-[var(--container-5xl)]",
            DrawerWidth.SixXl => "left-[var(--container-6xl)]",
            DrawerWidth.SevenXl => "left-[var(--container-7xl)]",
            DrawerWidth.Full => "left-[100%]",
            _ => "left-0"
        };
    }

    public static string GetInsetRight(DrawerWidth? width)
    {
        return width switch
        {
            DrawerWidth.Xs => "right-[var(--container-xs)]",
            DrawerWidth.Sm => "right-[var(--container-sm)]",
            DrawerWidth.Md => "right-[var(--container-md)]",
            DrawerWidth.Lg => "right-[var(--container-lg)]",
            DrawerWidth.Xl => "right-[var(--container-xl)]",
            DrawerWidth.TwoXl => "right-[var(--container-2xl)]",
            DrawerWidth.ThreeXl => "right-[var(--container-3xl)]",
            DrawerWidth.FourXl => "right-[var(--container-4xl)]",
            DrawerWidth.FiveXl => "right-[var(--container-5xl)]",
            DrawerWidth.SixXl => "right-[var(--container-6xl)]",
            DrawerWidth.SevenXl => "right-[var(--container-7xl)]",
            DrawerWidth.Full => "right-[100%]",
            _ => "right-0"
        };
    }
}
