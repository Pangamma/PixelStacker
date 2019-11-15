using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PixelStacker.Logic.Great
{
    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {

        readonly List<TElement> elements;
        public TKey Key { get; private set; }

        public Grouping(TKey key, List<TElement> elements)
        {
            this.Key = key;
            this.elements = elements;
        }


        public IEnumerator<TElement> GetEnumerator()
        {
            return this.elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.elements.GetEnumerator();
        }
    }
}
