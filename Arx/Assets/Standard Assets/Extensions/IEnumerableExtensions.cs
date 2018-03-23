using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExtensions
{
    public static IEnumerable<Tuple<T, T>> ToPairs<T>(this IEnumerable<T> source)
    {
        if (source != null)
        {
            var previous = default(T);
            
            using (var it = source.GetEnumerator())
            {
                if (it.MoveNext())
                    previous = it.Current;
                
                while (it.MoveNext())
                    yield return new Tuple<T, T>(previous, previous = it.Current);
            }
        }
    }

    public static TData Random<TData>(this IEnumerable<TData> data)
    {
        if(data.Count() == 0)
        {
            return default(TData);
        }
        var idx = UnityEngine.Random.Range(0, data.Count());
        return data.ElementAt(idx);
    }
}

