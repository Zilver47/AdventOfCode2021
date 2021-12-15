using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Extensions
{
    public static class ListExtensions
    {
        public static IList<T> Clone<T>(this IList<T> source) where T: ICloneable
        {
            return source.Select(item => (T)item.Clone()).ToList();
        }
        
        public static HashSet<T> Clone<T>(this HashSet<T> source) 
        {
            return source.Select(item => item).ToHashSet();
        }
    }
}
