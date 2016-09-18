// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "Toon/Lightmap" {

	Properties{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	_BumpMap("Normal Map", 2D) = "bump" {}
	_Dummy("Dummy", 2D) = "white" {}
	//_SpecularTex ("Specular Map", 2D) = "gray" {}
	_SpecularPower("Specular Intensity (0.0 - 5.0)", float) = 0.5 //Range(0.0, 5.0)
		_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
	_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width (.002 - .03)", float) = .005 //Range(0.002, 0.03)
	}

		SubShader{

		Tags{ "RenderType" = "Opaque" }

		LOD 200

		//UsePass "Toon/Basic Outline/OUTLINE"

		Cull Back

		CGPROGRAM

#pragma surface surf ToonRamp nolightmap

		sampler2D _Ramp;
	sampler2D _MainTex, _BumpMap;// , _SpecularTex;
	half4 _Color;
	half _SpecularPower;
	// sampler2D unity_Lightmap;
	// float4 unity_LightmapST;
	sampler2D _Dummy;

	// custom lighting function that uses a texture ramp based

	// on angle between light direction and normal

#pragma lighting ToonRamp exclude_path:prepass

	inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
	{
#ifndef USING_DIRECTIONAL_LIGHT
		lightDir = normalize(lightDir);
#endif

		half d = dot(s.Normal, lightDir)*0.5 + 0.5;
		half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

		half nh = max(0, dot(s.Normal, lightDir));
		half spec = pow(nh, s.Gloss * 128) * s.Specular * _SpecularPower;

		half4 c;
		c.rgb = (s.Albedo * _LightColor0.rgb * ramp + _LightColor0.rgb * spec) * (atten * 2);
		c.a = 0;

		return c;
	}

	struct Input {
		half2 uv_MainTex : TEXCOORD0;
		half2 uv2_Dummy;
		half3 worldNormal;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		float2 lmuv = IN.uv2_Dummy.xy * unity_LightmapST.xy + unity_LightmapST.zw;

		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));

		half3 specGloss = tex2D(_BumpMap, IN.uv_MainTex).rgb;

		o.Specular = specGloss.r;
		o.Gloss = specGloss.g;

		o.Albedo = c.rgb * DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, lmuv));
		o.Alpha = c.a;

	}

	ENDCG
	}
		Fallback "Diffuse"
}