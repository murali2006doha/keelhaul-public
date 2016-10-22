using UnityEngine;
using System.Collections;

public class LayerHelper {

    public static void setLayerRecursively(GameObject obj,int  newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            setLayerRecursively(child.gameObject, newLayer);
        }
    }
}
