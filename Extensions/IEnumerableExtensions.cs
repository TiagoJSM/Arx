using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions
{
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
    }
}
