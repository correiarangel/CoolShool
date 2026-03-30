using Microsoft.AspNetCore.Components;
using MudBlazor;
using CoolShool.WebUI.Theme;

namespace CoolShool.WebUI.Layout;

public partial class MainLayout : LayoutComponentBase
{
    private bool _drawerOpen = true;
    private bool _isDarkMode;
    private MudTheme _currentTheme = AppTheme.DefaultTheme;

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }
}
