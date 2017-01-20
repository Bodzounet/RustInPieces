using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class ListExtensionMethod
{
    public static List<T> CircularPermutation<T>(this List<T> list, T newFirstItem)
    {
        List<T> ret = new List<T>();

        ret.AddRange(list.SkipWhile(x => !x.Equals(newFirstItem)).TakeWhile((x, index) => index < list.Count));
        ret.AddRange(list.TakeWhile(x => !x.Equals(newFirstItem)));

        return ret;
    }

    public static List<T> CircularPermutationWithShift<T>(this List<T> list, T newFirstItem)
    {
        List<T> ret = new List<T>();

        ret.AddRange(list.SkipWhile(x => !x.Equals(newFirstItem)).Skip(1).TakeWhile((x, index) => index < list.Count));
        ret.AddRange(list.TakeWhile(x => !x.Equals(newFirstItem)));
        ret.Add(newFirstItem);

        return ret;
    }
}
