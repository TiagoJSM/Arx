using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Extensions
{
    [Serializable]
    public struct Tuple<T1, T2>
    {
        public T1 Item1;
        public T2 Item2;

        public Tuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }

    public static class IEnumerableExtensions
    {
        public static IEnumerable<Tuple<TSource, TSource>> ToPairs<TSource>(this IEnumerable<TSource> source)
        {
            TSource previous = default(TSource);

            using (var it = source.GetEnumerator())
            {
                if (it.MoveNext())
                    previous = it.Current;

                while (it.MoveNext())
                    yield return new Tuple<TSource, TSource>(previous, previous = it.Current);
            }
        }

        public static IEnumerable<Tuple<TSource, TSource>> ToSequencePairs<TSource>(this IEnumerable<TSource> source)
        {
            return
                source
                    .Select((value, index) => new { Index = index, Value = value })
                    .GroupBy(x => x.Index / 2)
                    .Select(g => new Tuple<TSource, TSource>(g.ElementAt(0).Value, g.ElementAt(1).Value));
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence was empty");
                }
                TSource min = sourceIterator.Current;
                TKey minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    TSource candidate = sourceIterator.Current;
                    TKey candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }

        public static int? IndexOf<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }
            return null;
        }

        public static int? IndexOfMin<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            if(source.Count() == 0)
            {
                return null;
            }
            var min = source.MinBy(selector);
            return source.IndexOf(s => s.Equals(min));
        }

        public static TData Random<TData>(this IEnumerable<TData> data)
        {
            var idx = UnityEngine.Random.Range(0, data.Count());
            return data.ElementAt(idx);
        }
    }
}
