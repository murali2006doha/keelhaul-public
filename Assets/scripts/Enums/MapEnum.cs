using System;
using System.Collections;

public enum MapEnum {
	AtlantisMap, ChineseMap, VikingMap, BlackbeardMap, FrigidWasteland, TropicalMap, LevelUp, AbhiAtlantisMap, WillsMap
}


public class MapTypeHelper
{
    public static MapEnum GetRandomMap()
	{
		return MapEnum.AbhiAtlantisMap;
	}
}
