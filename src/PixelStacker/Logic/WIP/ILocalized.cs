using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    public interface ILocalized
    {
        void ApplyLocalization(CultureInfo locale);
    }
}
