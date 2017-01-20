using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class LinqExtensionMethods
{
    public static void ForEach<T>(this IEnumerable<T> enumeration, System.Action<T> action)
    {
        foreach (T item in enumeration)
        {
            action(item);
        }
    }

    public static void Print<T>(this IEnumerable<T> list)
    {
        foreach (T item in list)
        {
            Debug.Log(item.ToString());
        }
    }
}
