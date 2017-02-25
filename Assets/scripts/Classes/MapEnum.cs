using System;
using System.Collections;

public enum MapEnum {
	AtlantisMap, ChineseMap, VikingMap, BlackbeardMap
}


public class MapTypeHelper
{
    public static MapEnum GetRandomMap()
    {
        Array values = Enum.GetValues(typeof(MapEnum));
        Random random = new Random();
        return (MapEnum)values.GetValue(random.Next(values.Length));
    }
}