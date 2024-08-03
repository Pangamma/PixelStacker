using SkiaSharp;
using System;
using System.ComponentModel;

namespace PixelStacker.Resources.Themes
{
    public enum AppTheme
    {
        Light,
        Dark,
        Smooth
    }

    public class ThemeChangeEventArgs : EventArgs
    {
        public AppTheme Theme { get; private set; }
        public ThemeChangeEventArgs(AppTheme theme)
        {
            this.Theme = theme;
        }

        public static implicit operator ThemeChangeEventArgs(AppTheme t) => new ThemeChangeEventArgs(t);
    }

    public class ThemeManager
    {
        private static AppTheme theme = AppTheme.Light;
        public static AppTheme Theme
        {
            get => ThemeManager.theme; set
            {
                ThemeManager.theme = value;
                if (ThemeManager.OnThemeChange != null)
                {
                    OnThemeChange(null, new ThemeChangeEventArgs(value));
                }

            }
        }


        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public static event EventHandler<ThemeChangeEventArgs> OnThemeChange;
    }
}
