using UnityEngine;
using System.Collections;

public class MathHelper {

	public static Vector3 setX(Vector3 source, float x)
    {
        Vector3 result = new Vector3(x, source.y, source.z);
        return result;
    }

    public static Vector3 addX(Vector3 source, float amt)
    {
        Vector3 result = new Vector3(source.x + amt, source.y, source.z);
        return result;
    }

    public static Vector3 setY(Vector3 source, float y)
    {
        Vector3 result = new Vector3(source.x, y, source.z);
        return result;
    }

    public static Vector3 addY(Vector3 source, float amt)
    {
        Vector3 result = new Vector3(source.x , source.y + amt, source.z);
        return result;
    }

    public static Vector3 setZ(Vector3 source, float z)
    {
        Vector3 result = new Vector3(source.x, source.y, z);
        return result;
    }

    public static Vector3 addZ(Vector3 source, float amt)
    {
        Vector3 result = new Vector3(source.x , source.y, source.z + amt);
        return result;
    }



}
