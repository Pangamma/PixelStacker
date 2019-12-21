using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PixelStacker.Logic.Great
{
    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {

        public List<TElement> elements { get; private set; }
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

        public override string ToString()
        {
            return $"Count: {this.elements.Count}, Key: {this.Key}";
        }
    }
}
