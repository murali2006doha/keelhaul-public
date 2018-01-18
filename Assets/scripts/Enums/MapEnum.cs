using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapEnum {
	AtlantisMap, ChineseMap, VikingMap, BlackbeardMap, FrigidWasteland, TropicalMapFinal, TropicalMap, LevelUp,BlackMarket
}


public class MapTypeHelper
{
    public static MapEnum GetRandomMap()
	{
		return MapEnum.TropicalMap;
	}

    public static List<MapEnum> GetDeathMatchOfflineMaps() {
        List<MapEnum> maps = new List<MapEnum>();
        maps.Add(MapEnum.LevelUp);
        maps.Add(MapEnum.BlackMarket);
		maps.Add(MapEnum.TropicalMapFinal);
        maps.Add(MapEnum.VikingMap);

        return maps;
    }

	public static List<MapEnum> GetSabotageOfflineMaps()
	{
		List<MapEnum> maps = new List<MapEnum>();
		maps.Add(MapEnum.BlackMarket);
		maps.Add(MapEnum.LevelUp);
    maps.Add(MapEnum.VikingMap);

    return maps;
	}
}
