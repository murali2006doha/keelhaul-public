using UnityEngine;
using System.Collections;

public static class LinearToDBConverter
{
  public static float LinearToDecibal(float linear)
  {
    if (linear == 0.0f)
    {
      return -80f;
    }
    else
    {
      return (50f * Mathf.Log10(linear) + 10f);
      //original: (100f * Mathf.Log10(linear) + 20f)
    }

  }
}
