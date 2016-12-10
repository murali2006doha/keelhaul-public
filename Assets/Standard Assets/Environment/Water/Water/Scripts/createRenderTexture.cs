using UnityEngine;
using System.Collections;
namespace UnityStandardAssets.Water
{
    public class createRenderTexture : MonoBehaviour
    {
        public Camera refractCamera;
        public RenderTexture refract;
        bool refractSet = false;
        public int textureSize;
        // Use this for initialization
        void Start()
        {
            refract = new RenderTexture(textureSize, textureSize, 16);
            refract.name = "__WaterReflection" + GetInstanceID();
            refract.isPowerOfTwo = true;
            refract.hideFlags = HideFlags.DontSave;
            refractCamera.targetTexture = refract; 
        }



    }
}