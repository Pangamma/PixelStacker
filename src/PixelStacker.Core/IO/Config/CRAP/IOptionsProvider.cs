using PixelStacker.Core.IO.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Core.Model.Config
{
    public interface IOptionsProvider
    {
        Options Load();
        void Save(Options t);
    }

    public class MemoryOptionsProvider : IOptionsProvider
    {
        public Options Value;
        public Options Load()
        {
            Value ??= new Options(this);
            return Value;
        }

        public void Save(Options t)
        {
            Value = t;
        }
    }
}
