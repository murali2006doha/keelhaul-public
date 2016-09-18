using System;
using UnityEngine;
using System.Collections.Generic;

public class ArrayHelper
{
    public static T[] filterTag<T>(T[] array, string tag) where T: Component
    {
        if(array == null)
        {
            return null;
        }
        List<T> result = new List<T>();
        foreach(T item in array)
        {
            if (item.CompareTag(tag))
            {
                result.Add(item);
            }
        }

        return result.ToArray();
    }
}