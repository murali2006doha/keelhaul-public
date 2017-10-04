// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.36 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.36;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32716,y:32678,varname:node_4795,prsc:2|diff-4312-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:744,x:31209,y:32273,varname:node_744,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:9580,x:31382,y:32009,prsc:2,pt:False;n:type:ShaderForge.SFN_Append,id:5454,x:31469,y:32211,varname:node_5454,prsc:2|A-744-Y,B-744-Z;n:type:ShaderForge.SFN_Append,id:6287,x:31469,y:32330,varname:node_6287,prsc:2|A-744-Z,B-744-X;n:type:ShaderForge.SFN_Append,id:928,x:31469,y:32452,varname:node_928,prsc:2|A-744-X,B-744-Y;n:type:ShaderForge.SFN_Abs,id:3965,x:31561,y:31996,varname:node_3965,prsc:2|IN-9580-OUT;n:type:ShaderForge.SFN_Multiply,id:1257,x:31735,y:31996,varname:node_1257,prsc:2|A-3965-OUT,B-3965-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:7736,x:31447,y:32610,ptovrint:False,ptlb:node_7736,ptin:_node_7736,varname:node_7736,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b66bceaf0cc0ace4e9bdc92f14bba709,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:7281,x:31695,y:32194,varname:node_7281,prsc:2,tex:b66bceaf0cc0ace4e9bdc92f14bba709,ntxv:0,isnm:False|UVIN-5454-OUT,TEX-7736-TEX;n:type:ShaderForge.SFN_Tex2d,id:4685,x:31695,y:32384,varname:node_4685,prsc:2,tex:b66bceaf0cc0ace4e9bdc92f14bba709,ntxv:0,isnm:False|UVIN-6287-OUT,TEX-7736-TEX;n:type:ShaderForge.SFN_Tex2d,id:8348,x:31695,y:32542,varname:node_8348,prsc:2,tex:b66bceaf0cc0ace4e9bdc92f14bba709,ntxv:0,isnm:False|UVIN-928-OUT,TEX-7736-TEX;n:type:ShaderForge.SFN_ChannelBlend,id:4312,x:31998,y:32284,varname:node_4312,prsc:2,chbt:0|M-1257-OUT,R-7281-RGB,G-4685-RGB,B-8348-RGB;proporder:7736;pass:END;sub:END;*/

Shader "Shader Forge/triplaner" {
    Properties {
        _node_7736 ("node_7736", 2D) = "black" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
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
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _node_7736; uniform float4 _node_7736_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                LIGHTING_COORDS(2,3)
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 node_3965 = abs(i.normalDir);
                float3 node_1257 = (node_3965*node_3965);
                float2 node_5454 = float2(i.posWorld.g,i.posWorld.b);
                float4 node_7281 = tex2D(_node_7736,TRANSFORM_TEX(node_5454, _node_7736));
                float2 node_6287 = float2(i.posWorld.b,i.posWorld.r);
                float4 node_4685 = tex2D(_node_7736,TRANSFORM_TEX(node_6287, _node_7736));
                float2 node_928 = float2(i.posWorld.r,i.posWorld.g);
                float4 node_8348 = tex2D(_node_7736,TRANSFORM_TEX(node_928, _node_7736));
                float3 diffuseColor = (node_1257.r*node_7281.rgb + node_1257.g*node_4685.rgb + node_1257.b*node_8348.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
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
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _node_7736; uniform float4 _node_7736_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                LIGHTING_COORDS(2,3)
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 node_3965 = abs(i.normalDir);
                float3 node_1257 = (node_3965*node_3965);
                float2 node_5454 = float2(i.posWorld.g,i.posWorld.b);
                float4 node_7281 = tex2D(_node_7736,TRANSFORM_TEX(node_5454, _node_7736));
                float2 node_6287 = float2(i.posWorld.b,i.posWorld.r);
                float4 node_4685 = tex2D(_node_7736,TRANSFORM_TEX(node_6287, _node_7736));
                float2 node_928 = float2(i.posWorld.r,i.posWorld.g);
                float4 node_8348 = tex2D(_node_7736,TRANSFORM_TEX(node_928, _node_7736));
                float3 diffuseColor = (node_1257.r*node_7281.rgb + node_1257.g*node_4685.rgb + node_1257.b*node_8348.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
