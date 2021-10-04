using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Config
{
    public interface IStorage<T>
    {
        T Load();
        void Save(T t);
    }

    public class MemoryStorage<T> : IStorage<T> where T: new()
    {
        private T Value;
        public T Load()
        {
            return Value;
        }

        public void Save(T t)
        {
            this.Value = t;
        }
    }
}
