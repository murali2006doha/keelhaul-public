Shader "Custom/ShipShader" {
	Properties{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_HitColor("Hit Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_HitTex("Base2 (RGB)", 2D) = "white" {}
		_Block("Block", Range(0,1)) = 0
		_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf ToonRamp

	sampler2D _Ramp;
	float _Block;
	sampler2D _HitTex;
	// custom lighting function that uses a texture ramp based
	// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
	inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
	{
#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
#endif

		half d = dot(s.Normal, lightDir)*0.5 + 0.5;
		float4 rampTex = tex2D(_Ramp, float2(d, d));
		float4 hitTex = tex2D(_HitTex, float2(0, 0));
		half4 chosenRamp = lerp(rampTex, hitTex, _Block.x);

		half3 ramp = chosenRamp.rgb;

		half4 c;
		c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
		c.a = 0;
		return c;
	}


	sampler2D _MainTex;
	float4 _Color;
	float4 _HitColor;
	float2 _s = 0;
	struct Input {
		float2 uv_MainTex : TEXCOORD0;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		float4 sample2d1 = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		float4 sample2d2 = tex2D(_HitTex, IN.uv_MainTex) * _HitColor;
		half4 c = lerp(sample2d1, sample2d2, _Block.x);
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG

	}

		Fallback "Diffuse"
}
