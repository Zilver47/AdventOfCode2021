using System;
using System.Collections.Generic;

namespace AdventOfCode.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate<T, TValue>(this Dictionary<T, TValue> source, T key, TValue value) 
        {
            if (source.ContainsKey(key))
            {
                source[key] = value;
            }
            else
            {
                source.Add(key, value);
            }
        }
        
        public static void AddOrIncrease<T>(this Dictionary<T, long> source, T key, long value = 1) 
        {
            if (source.ContainsKey(key))
            {
                source[key] += value;
            }
            else
            {
                source.Add(key, value);
            }
        }
    }
}
