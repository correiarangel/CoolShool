using MudBlazor;

namespace CoolShool.WebUI.Theme;

public static class AppTheme
{
    public static MudTheme DefaultTheme => new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#3B82F6", // Blue 500
            Secondary = "#6366F1", // Indigo 500
            Tertiary = "#10B981", // Emerald 500
            AppbarBackground = "#FFFFFF",
            Background = "#F8FAFC", // Slate 50
            Surface = "#FFFFFF",
            DrawerBackground = "#FFFFFF",
            DrawerText = "#1E293B", // Slate 800
            Success = "#10B981",
            Warning = "#F59E0B",
            Error = "#EF4444",
            Info = "#3B82F6",
            Divider = "#E2E8F0",
            ActionDefault = "#64748B",
            TextPrimary = "#0F172A", // Slate 900
            TextSecondary = "#475569", // Slate 600
        },
        PaletteDark = new PaletteDark()
        {
            Primary = "#60A5FA", // Blue 400
            Secondary = "#818CF8", // Indigo 400
            Tertiary = "#34D399", // Emerald 400
            Background = "#0B0F1A", // Um Slate ainda mais profundo
            Surface = "#1E293B", // Slate 800
            AppbarBackground = "#1E293B",
            DrawerBackground = "#1E293B",
            DrawerText = "#F8FAFC",
            Success = "#34D399",
            Warning = "#FBBF24",
            Error = "#F87171",
            Info = "#60A5FA",
            Divider = "#334155", // Slate 700
            ActionDefault = "#94A3B8",
            TextPrimary = "#F8FAFC",
            TextSecondary = "#94A3B8",
        },
        Typography = new Typography()
        {
            Default = new DefaultTypography()
            {
                FontFamily = new[] { "Plus Jakarta Sans", "Inter", "Helvetica", "Arial", "sans-serif" },
                FontSize = ".875rem",
                LineHeight = "1.5"
            },
            H1 = new H1Typography() { FontFamily = new[] { "Outfit", "sans-serif" }, FontWeight = "700" },
            H2 = new H2Typography() { FontFamily = new[] { "Outfit", "sans-serif" }, FontWeight = "700" },
            H3 = new H3Typography() { FontFamily = new[] { "Outfit", "sans-serif" }, FontWeight = "700" },
            H4 = new H4Typography() { FontFamily = new[] { "Outfit", "sans-serif" }, FontWeight = "700" },
            H5 = new H5Typography() { FontFamily = new[] { "Outfit", "sans-serif" }, FontWeight = "600" },
            H6 = new H6Typography() { FontFamily = new[] { "Outfit", "sans-serif" }, FontWeight = "600" },
            Button = new ButtonTypography() { FontFamily = new[] { "Outfit", "sans-serif" }, FontWeight = "600", TextTransform = "none" },
            Body1 = new Body1Typography() { FontFamily = new[] { "Plus Jakarta Sans", "sans-serif" } },
            Body2 = new Body2Typography() { FontFamily = new[] { "Plus Jakarta Sans", "sans-serif" } }
        },
        LayoutProperties = new LayoutProperties()
        {
            DefaultBorderRadius = "12px"
        }
    };
}
