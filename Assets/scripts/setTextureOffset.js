#pragma strict
 
public var scrollSpeedX : float = 0.1;
public var scrollSpeedY : float = 0.1;
private var mesh : Mesh;
 
function Start() 
{
    mesh = this.transform.GetComponent(MeshFilter).mesh;
}
 
function Update() 
{
    SwapUVs();
}
 
function SwapUVs()
{
    var uvSwap : Vector2[] = mesh.uv;
 
    for (var b:int = 0; b < uvSwap.length; b ++)
    {
       uvSwap[b] += Vector2( scrollSpeedX * (Time.deltaTime), scrollSpeedY * (Time.deltaTime));
    }
 
mesh.uv = uvSwap;
}