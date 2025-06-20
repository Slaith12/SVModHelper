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
        public static ID ToID(this CardID id)
        {
            return id.BoxIl2CppObject().Cast<ID>();
        }

        public static Il2CppSystem.Object ToObject(this Il2CppObjectBase obj)
        {
            return obj.Cast<Il2CppSystem.Object>();
        }

        public static Il2CppSystem.Collections.IEnumerator ToILCPP(this System.Collections.IEnumerator enumerator)
        {
            return new EnumeratorLink(enumerator).Cast<Il2CppSystem.Collections.IEnumerator>();
        }

        public static Il2CppCollections.IEnumerable<T> ToILCPPEnumerable<T>(this List<T> list)
        {
            return list.ToILCPP().Cast<Il2CppCollections.IEnumerable<T>>();
        }

        public static Il2CppCollections.List<T> ToILCPP<T>(this IList<T> list)
        {
            Il2CppCollections.List<T> newList = new(list.Count);
            foreach(T item in list)
            {
                newList.Add(item);
            }
            return newList;
        }

        public static List<T> ToMono<T>(this Il2CppCollections.List<T> list)
        {
            List<T> newList = new(list.Count);
            for(int i = 0; i < list.Count; i++)
            {
                newList.Add(list[i]);
            }
            return newList;
        }

        public static Il2CppCollections.HashSet<T> ToILCPP<T>(this ISet<T> list)
        {
            Il2CppCollections.HashSet<T> newList = new();
            foreach (T item in list)
            {
                newList.Add(item);
            }
            return newList;
        }

        public static Il2CppCollections.Dictionary<TKey,TValue> ToILCPP<TKey, TValue>(this IDictionary<TKey, TValue> dict)
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
