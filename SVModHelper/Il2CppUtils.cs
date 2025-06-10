using Il2CppInterop.Runtime.InteropTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVModHelper
{
    public static class Il2CppUtils
    {
        public static Il2CppCollections.List<T> Convert<T>(this List<T> list)
        {
            Il2CppCollections.List<T> newList = new(list.Count);
            foreach(T item in list)
            {
                newList.Add(item);
            }
            return newList;
        }

        public static Il2CppCollections.HashSet<T> Convert<T>(this HashSet<T> list)
        {
            Il2CppCollections.HashSet<T> newList = new();
            foreach (T item in list)
            {
                newList.Add(item);
            }
            return newList;
        }

        public static Il2CppCollections.Dictionary<TKey,TValue> ToIl2Cpp<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            Il2CppCollections.Dictionary<TKey, TValue> newDict = new(dict.Count);
            foreach(KeyValuePair<TKey, TValue> kvp in dict)
            {
                newDict.Add(kvp.Key, kvp.Value);
            }
            return newDict;
        }
    }
}
