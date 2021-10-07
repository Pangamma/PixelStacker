using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelStacker.Extensions
{
    public static partial class Extend
    {
        /// <summary>
        /// Selects distinct items by a given selector.
        /// </summary>
        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> items, Func<T, TKey> selector)
        {
            HashSet<TKey> keys = new HashSet<TKey>();
            return items.Where(item => keys.Add(selector(item)));
        }

        public static TKey Median<T, TKey>(this IEnumerable<T> items, Func<T, TKey> selector)
        {
            if (!items.Any()) throw new InvalidOperationException("List cannot be empty.");
            return items.OrderBy(selector).Skip(items.Count() / 2).Take(1).Select(selector).FirstOrDefault();
        }
        #region AverageByPercentile
        public static List<double> AverageByPercentile<T,TKey>(this IEnumerable<T> items, float percentChunkSize, Func<T, TKey> selector)
        {
            if (!items.Any())
                throw new InvalidOperationException("List cannot be empty.");
            if (percentChunkSize < 0)
                throw new ArgumentOutOfRangeException(nameof(percentChunkSize), percentChunkSize, "Must be greater than 0.");
            if (!typeof(TKey).IsPrimitive)
                throw new ArgumentException(nameof(selector), "Selector must be a selector for a numeric type.");
            
            var ordered = items.OrderByDescending(selector);
            double max = items.Count();
            int itemsPerChunk = (int)Math.Ceiling(max * (percentChunkSize/ 100));
            double numChunks = Math.Ceiling( max / itemsPerChunk);
            List<double> avgs = new List<double>();


           
            for (int i = 0; i < numChunks; i++)
            {
                double avg; if (selector is Func<T, int> funcInt)
                    avg = ordered.Skip(i * itemsPerChunk).Take(itemsPerChunk).Average(funcInt);
                else if (selector is Func<T, long> funcLong)
                    avg = ordered.Skip(i * itemsPerChunk).Take(itemsPerChunk).Average(funcLong);
                else if (selector is Func<T, decimal> funcDec)
                    avg = (double)ordered.Skip(i * itemsPerChunk).Take(itemsPerChunk).Average(funcDec);
                else if (selector is Func<T, float> funcFloat)
                    avg = ordered.Skip(i * itemsPerChunk).Take(itemsPerChunk).Average(funcFloat);
                else if (selector is Func<T, double> funcDub)
                    avg = ordered.Skip(i * itemsPerChunk).Take(itemsPerChunk).Average(funcDub);
                else throw new ArgumentException(nameof(selector), "Selector must select a numeric format.");

                avgs.Add(Math.Round(avg,2));
            }

            return avgs;
        }
        #endregion AverageByPercentile

        public static List<List<T>> SplitInto<T>(this IEnumerable<T> items, int numSubLists)
        {
            List<List<T>> output = new List<List<T>>();
            int numPerFragment = items.Count() / numSubLists;
            for(int i = 0; i < numSubLists; i++)
            {
                output.Add(new List<T>(items.Skip(i * numPerFragment).Take(numPerFragment).ToList()));
            }

            // Collect any outstanding items and add to first item in list
            output.First().AddRange(items.Skip(numSubLists * numPerFragment).Take(numPerFragment).ToList());

            return output.Where(x => x.Any()).ToList();
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, Comparer<TKey>.Default);
        }

        public static T MaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, IComparer<TKey> comparer)
        {
            using (IEnumerator<T> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext()) throw new InvalidOperationException("Sequence was empty");
                
                T max = sourceIterator.Current;
                TKey maxKey = selector.Invoke(max);

                while (sourceIterator.MoveNext())
                {
                    T candidate = sourceIterator.Current;
                    TKey candidateProjected = selector.Invoke(candidate);

                    if (comparer.Compare(candidateProjected, maxKey) > 0)
                    {
                        max = candidate;
                        maxKey = candidateProjected;
                    }
                }

                return max;
            }
        }


        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }

        public static T MinBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector, IComparer<TKey> comparer)
        {
            using (IEnumerator<T> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext()) throw new InvalidOperationException("Sequence was empty");

                T min = sourceIterator.Current;
                TKey minKey = selector.Invoke(min);

                while (sourceIterator.MoveNext())
                {
                    T candidate = sourceIterator.Current;
                    TKey candidateProjected = selector.Invoke(candidate);

                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }

                return min;
            }
        }
    }
}
