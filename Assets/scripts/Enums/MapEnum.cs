using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapEnum {
	AtlantisMap, ChineseMap, VikingMap, BlackbeardMap, FrigidWasteland, TropicalMap, LevelUp
}


public class MapTypeHelper
{
    public static MapEnum GetRandomMap()
	{
		return MapEnum.TropicalMap;
	}

    public static List<MapEnum> GetDeathMatchOfflineMaps() {
        List<MapEnum> maps = new List<MapEnum>();
        maps.Add(MapEnum.ChineseMap);
        maps.Add(MapEnum.TropicalMap);

        return maps;
    }

	public static List<MapEnum> GetSabotageOfflineMaps()
	{
		List<MapEnum> maps = new List<MapEnum>();
		maps.Add(MapEnum.ChineseMap);
		maps.Add(MapEnum.TropicalMap);

        return maps;
	}
}
