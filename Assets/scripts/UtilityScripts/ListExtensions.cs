using System;
using System.Collections.Generic;

public static class ListExtensions
{

    public static List<T> Filter<T>(this List<T> list, Func<T, bool> fn)
    {
        var newList = new List<T>();

        foreach (var element in list)
        {
            if (fn(element))
            {
                newList.Add(element);
            }
        }

        return newList;
    }
}