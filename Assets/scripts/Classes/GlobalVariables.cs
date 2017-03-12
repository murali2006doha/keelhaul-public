using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GlobalVariables {

    public static float gameSpeed = 1.25f;
    public static float uiSliderSpeed = 2.5f;
    public static bool firstTimeBoot = false;
    public static Dictionary<string,string> shipToPrefabLocation = new Dictionary<string, string> () {
      {ShipEnum.BlackbeardShip.ToString(),"Ship/Blackbeard Ship"},
      {ShipEnum.AtlanteanShip.ToString(),"Ship/Atlantean Ship"},
      {ShipEnum.ChineseJunkShip.ToString(),"Ship/Chinese Junk Ship"},
      {ShipEnum.VikingShip.ToString(),"Ship/Viking Ship"}
    };
    
    public enum MapEnum
    {
      Tropical, Chinese
    };

    public static Dictionary<ShipEnum, string> ShipToColor = new Dictionary<ShipEnum, string>(){
      { ShipEnum.AtlanteanShip, "blue"},
      { ShipEnum.BlackbeardShip, "black"},
      { ShipEnum.ChineseJunkShip, "yellow"},
      { ShipEnum.VikingShip, "red"}
    };

    public static Dictionary<string, string> mapToScene = new Dictionary<string, string>() {
      {MapEnum.Tropical.ToString(),"free for all_vig"},
      {MapEnum.Chinese.ToString(),"free for all_marketplace"}
    };

    public static string [] mapsToLoad = { MapEnum.Tropical.ToString(), MapEnum.Chinese.ToString() };

    public static string getMapToLoad() {
      string mapToLoad = mapsToLoad[Random.Range(0, mapsToLoad.Length)]; 

      if (!firstTimeBoot) {
        firstTimeBoot = true;
        mapToLoad = MapEnum.Chinese.ToString();
      }
      return mapToScene[mapToLoad];
    }
    
    public static float windFactor = 4f;
    public static float minCcVelocity = 0.2f;

    public static float killFeedDuration = 6f;
}

