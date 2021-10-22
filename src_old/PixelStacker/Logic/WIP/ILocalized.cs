using System.Globalization;

namespace PixelStacker.Logic
{
    public interface ILocalized
    {
        void ApplyLocalization(CultureInfo locale);
    }
}
