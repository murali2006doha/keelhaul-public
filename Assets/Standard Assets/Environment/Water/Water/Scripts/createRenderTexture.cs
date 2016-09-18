using UnityEngine;
using System.Collections;
namespace UnityStandardAssets.Water
{
    public class createRenderTexture : MonoBehaviour
    {
        public Camera refractCamera;
        public RenderTexture refract;
        bool refractSet = false;
        // Use this for initialization
        void Start()
        {
            refract = refractCamera.targetTexture;
        }



    }
}