Shader "Unlit/barrel"
{
	SubShader
	{

		Pass
	{
		ZTest GEqual
		ZWrite Off
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

	struct v2f
	{
		float4 pos : SV_POSITION;
		float3 viewDir : TEXCOORD;
		float3 normal : NORMAL;
	};

	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.normal = UnityObjectToWorldNormal(v.normal);
		o.viewDir = normalize(UnityWorldSpaceViewDir(o.pos));
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		//Note you have to normalize these again since they are being interpolated between vertices
		float rim = 1 - dot(normalize(i.normal), normalize(i.viewDir));
	return lerp(half4(1, 0, 0, 1), half4(0, 1, 0, 1), rim);
	}
		ENDCG
	}

		Pass
	{
		CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag

#include "UnityCG.cginc"

		fixed4 frag(v2f_img i) : SV_Target
	{
		return 1;
	}
		ENDCG
	}
	}
}