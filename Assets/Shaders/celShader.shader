// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9361,x:33209,y:32712,varname:node_9361,prsc:2|normal-9560-RGB,emission-2460-OUT,custl-2189-OUT,olwid-4217-OUT,olcol-9426-RGB;n:type:ShaderForge.SFN_LightVector,id:6869,x:32042,y:32834,varname:node_6869,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:9684,x:32042,y:32962,prsc:2,pt:True;n:type:ShaderForge.SFN_Dot,id:7782,x:32213,y:32881,cmnt:Lambert,varname:node_7782,prsc:2,dt:1|A-6869-OUT,B-9684-OUT;n:type:ShaderForge.SFN_Tex2d,id:851,x:32070,y:32349,ptovrint:False,ptlb:Diffuse,ptin:_Diffuse,varname:node_851,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:5927,x:32070,y:32534,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_5927,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_AmbientLight,id:7528,x:32734,y:32646,varname:node_7528,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2460,x:32927,y:32598,cmnt:Ambient Light,varname:node_2460,prsc:2|A-544-OUT,B-7528-RGB;n:type:ShaderForge.SFN_Multiply,id:544,x:32268,y:32448,cmnt:Diffuse Color,varname:node_544,prsc:2|A-851-RGB,B-5927-RGB;n:type:ShaderForge.SFN_Set,id:9764,x:32472,y:32337,varname:BCol,prsc:2|IN-544-OUT;n:type:ShaderForge.SFN_Set,id:2232,x:32386,y:32881,varname:lOut,prsc:2|IN-7782-OUT;n:type:ShaderForge.SFN_Get,id:5516,x:31340,y:33318,varname:node_5516,prsc:2|IN-2232-OUT;n:type:ShaderForge.SFN_Get,id:1515,x:32638,y:33119,varname:node_1515,prsc:2|IN-9764-OUT;n:type:ShaderForge.SFN_Multiply,id:3566,x:32659,y:33249,cmnt:Modify how dark you want the shadows,varname:node_3566,prsc:2|A-1515-OUT,B-6152-OUT;n:type:ShaderForge.SFN_Slider,id:6152,x:32280,y:33276,ptovrint:False,ptlb:ShadowDarkness,ptin:_ShadowDarkness,varname:node_6152,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_LightColor,id:829,x:31721,y:33644,varname:node_829,prsc:2;n:type:ShaderForge.SFN_LightAttenuation,id:1442,x:31721,y:33784,varname:node_1442,prsc:2;n:type:ShaderForge.SFN_Multiply,id:780,x:31981,y:33643,varname:node_780,prsc:2|A-5724-OUT,B-829-RGB,C-1442-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2501,x:31458,y:32887,ptovrint:False,ptlb:Tones,ptin:_Tones,varname:node_2501,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_Subtract,id:1389,x:31647,y:32857,cmnt:Number of lighting tones,varname:node_1389,prsc:2|A-2501-OUT,B-3959-OUT;n:type:ShaderForge.SFN_Vector1,id:3959,x:31473,y:33078,varname:node_3959,prsc:2,v1:1;n:type:ShaderForge.SFN_Set,id:7240,x:31830,y:32857,varname:Tones,prsc:2|IN-1389-OUT;n:type:ShaderForge.SFN_Multiply,id:9291,x:31648,y:33315,varname:node_9291,prsc:2|A-5516-OUT,B-3440-OUT;n:type:ShaderForge.SFN_Get,id:3440,x:31700,y:33185,varname:node_3440,prsc:2|IN-7240-OUT;n:type:ShaderForge.SFN_Round,id:2840,x:31832,y:33327,cmnt:Clamp the lighting,varname:node_2840,prsc:2|IN-9291-OUT;n:type:ShaderForge.SFN_Divide,id:5724,x:32019,y:33327,varname:node_5724,prsc:2|A-2840-OUT,B-3440-OUT;n:type:ShaderForge.SFN_Lerp,id:2189,x:32908,y:33301,varname:node_2189,prsc:2|A-3566-OUT,B-1515-OUT,T-780-OUT;n:type:ShaderForge.SFN_Tex2d,id:9560,x:32617,y:32850,ptovrint:False,ptlb:bump,ptin:_bump,varname:node_9560,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Slider,id:4217,x:32821,y:33008,ptovrint:False,ptlb:width,ptin:_width,varname:node_4217,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Color,id:9426,x:32924,y:32894,ptovrint:False,ptlb:node_9426,ptin:_node_9426,varname:node_9426,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;proporder:851-5927-6152-2501-9560-4217-9426;pass:END;sub:END;*/

Shader "celShading/cel" {
    Properties {
        _Diffuse ("Diffuse", 2D) = "white" {}
        _Color ("Color", Color) = (1,0.5,0.5,1)
        _ShadowDarkness ("ShadowDarkness", Range(0, 1)) = 0
        _Tones ("Tones", Float ) = 3
        _bump ("bump", 2D) = "bump" {}
        _width ("width", Range(0, 1)) = 0
        _node_9426 ("node_9426", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float _width;
            uniform float4 _node_9426;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_FOG_COORDS(0)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = mul(UNITY_MATRIX_MVP, float4(v.vertex.xyz + v.normal*_width,1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                return fixed4(_node_9426.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float4 _Color;
            uniform float _ShadowDarkness;
            uniform float _Tones;
            uniform sampler2D _bump; uniform float4 _bump_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 _bump_var = UnpackNormal(tex2D(_bump,TRANSFORM_TEX(i.uv0, _bump)));
                float3 normalLocal = _bump_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 node_544 = (_Diffuse_var.rgb*_Color.rgb); // Diffuse Color
                float3 emissive = (node_544*UNITY_LIGHTMODEL_AMBIENT.rgb);
                float3 BCol = node_544;
                float3 node_1515 = BCol;
                float lOut = max(0,dot(lightDirection,normalDirection));
                float Tones = (_Tones-1.0);
                float node_3440 = Tones;
                float3 finalColor = emissive + lerp((node_1515*_ShadowDarkness),node_1515,((round((lOut*node_3440))/node_3440)*_LightColor0.rgb*attenuation));
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _Diffuse; uniform float4 _Diffuse_ST;
            uniform float4 _Color;
            uniform float _ShadowDarkness;
            uniform float _Tones;
            uniform sampler2D _bump; uniform float4 _bump_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 _bump_var = UnpackNormal(tex2D(_bump,TRANSFORM_TEX(i.uv0, _bump)));
                float3 normalLocal = _bump_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float4 _Diffuse_var = tex2D(_Diffuse,TRANSFORM_TEX(i.uv0, _Diffuse));
                float3 node_544 = (_Diffuse_var.rgb*_Color.rgb); // Diffuse Color
                float3 BCol = node_544;
                float3 node_1515 = BCol;
                float lOut = max(0,dot(lightDirection,normalDirection));
                float Tones = (_Tones-1.0);
                float node_3440 = Tones;
                float3 finalColor = lerp((node_1515*_ShadowDarkness),node_1515,((round((lOut*node_3440))/node_3440)*_LightColor0.rgb*attenuation));
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}