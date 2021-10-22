using System.Globalization;

namespace PixelStacker.Resources.Localization
{
    public interface ILocalized
    {
        void ApplyLocalization(CultureInfo locale);
    }
}
