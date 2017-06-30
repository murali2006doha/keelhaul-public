// Shader created with Shader Forge v1.36 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.36;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:33118,y:32674,varname:node_4795,prsc:2|emission-2393-OUT,alpha-798-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32147,y:32718,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a0f2881ebf4394f579f05399401887fe,ntxv:0,isnm:False|UVIN-8711-UVOUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:32875,y:32769,varname:node_2393,prsc:2|A-6074-RGB,B-797-RGB;n:type:ShaderForge.SFN_Color,id:797,x:32522,y:32594,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:798,x:32535,y:32932,varname:node_798,prsc:2|A-6074-A,B-797-A,C-797-A,D-4209-OUT;n:type:ShaderForge.SFN_TexCoord,id:2548,x:31960,y:33100,varname:node_2548,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Slider,id:3981,x:32043,y:33486,ptovrint:False,ptlb:node_3981,ptin:_node_3981,varname:node_3981,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Time,id:1897,x:32159,y:33282,varname:node_1897,prsc:2;n:type:ShaderForge.SFN_RemapRange,id:6046,x:32476,y:33475,varname:node_6046,prsc:2,frmn:0,frmx:1,tomn:1,tomx:10|IN-3981-OUT;n:type:ShaderForge.SFN_Divide,id:926,x:32670,y:33460,varname:node_926,prsc:2|A-1897-T,B-6046-OUT;n:type:ShaderForge.SFN_Panner,id:8711,x:32709,y:33193,varname:node_8711,prsc:2,spu:0,spv:-1|UVIN-935-OUT,DIST-926-OUT;n:type:ShaderForge.SFN_Slider,id:4209,x:32457,y:33088,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_4209,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Sin,id:4558,x:32523,y:33314,varname:node_4558,prsc:2|IN-1897-T;n:type:ShaderForge.SFN_Abs,id:7896,x:33006,y:33639,varname:node_7896,prsc:2|IN-448-OUT;n:type:ShaderForge.SFN_TexCoord,id:4310,x:33129,y:33720,varname:node_4310,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Vector1,id:7482,x:33407,y:33284,varname:node_7482,prsc:2,v1:0;n:type:ShaderForge.SFN_Append,id:9332,x:33672,y:33466,varname:node_9332,prsc:2|A-9223-OUT,B-7482-OUT,C-9223-OUT;n:type:ShaderForge.SFN_OneMinus,id:3834,x:33338,y:33720,varname:node_3834,prsc:2|IN-4310-V;n:type:ShaderForge.SFN_Multiply,id:9223,x:33274,y:33501,varname:node_9223,prsc:2|A-9509-OUT,B-3834-OUT;n:type:ShaderForge.SFN_Multiply,id:8287,x:33152,y:33308,varname:node_8287,prsc:2|A-9332-OUT,B-1589-OUT;n:type:ShaderForge.SFN_Vector1,id:5867,x:32889,y:33538,varname:node_5867,prsc:2,v1:1;n:type:ShaderForge.SFN_Bitangent,id:1589,x:32961,y:33145,varname:node_1589,prsc:2;n:type:ShaderForge.SFN_Tan,id:448,x:32866,y:33719,varname:node_448,prsc:2|IN-1897-T;n:type:ShaderForge.SFN_Clamp,id:9509,x:33118,y:33489,varname:node_9509,prsc:2|IN-7896-OUT,MIN-8725-OUT,MAX-5867-OUT;n:type:ShaderForge.SFN_Vector1,id:8725,x:32954,y:33462,varname:node_8725,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:4217,x:33349,y:32854,varname:node_4217,prsc:2,v1:0.7;n:type:ShaderForge.SFN_Vector1,id:3095,x:33472,y:33003,varname:node_3095,prsc:2,v1:10;n:type:ShaderForge.SFN_TexCoord,id:7573,x:33476,y:32461,varname:node_7573,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:4297,x:33387,y:32728,varname:node_4297,prsc:2;n:type:ShaderForge.SFN_Multiply,id:6612,x:34017,y:32867,varname:node_6612,prsc:2|A-4453-OUT,B-8450-OUT;n:type:ShaderForge.SFN_OneMinus,id:4453,x:33848,y:32518,varname:node_4453,prsc:2|IN-7573-V;n:type:ShaderForge.SFN_Clamp,id:8450,x:33778,y:32932,varname:node_8450,prsc:2|IN-6010-OUT,MIN-4217-OUT,MAX-3095-OUT;n:type:ShaderForge.SFN_Abs,id:6010,x:33794,y:32673,varname:node_6010,prsc:2|IN-7625-OUT;n:type:ShaderForge.SFN_Sin,id:7625,x:33560,y:32728,varname:node_7625,prsc:2|IN-4297-TTR;n:type:ShaderForge.SFN_Multiply,id:935,x:32235,y:33107,varname:node_935,prsc:2|A-2548-UVOUT,B-2720-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2720,x:32050,y:32941,ptovrint:False,ptlb:node_2720,ptin:_node_2720,varname:node_2720,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;proporder:6074-797-3981-4209-2720;pass:END;sub:END;*/

Shader "Shader Forge/IslandFoam" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _node_3981 ("node_3981", Range(0, 1)) = 0
        _Opacity ("Opacity", Range(0, 1)) = 1
        _node_2720 ("node_2720", Float ) = 0.5
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _TintColor;
            uniform float _node_3981;
            uniform float _Opacity;
            uniform float _node_2720;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_1897 = _Time + _TimeEditor;
                float2 node_8711 = ((i.uv0*_node_2720)+(node_1897.g/(_node_3981*9.0+1.0))*float2(0,-1));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_8711, _MainTex));
                float3 emissive = (_MainTex_var.rgb*_TintColor.rgb);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,(_MainTex_var.a*_TintColor.a*_TintColor.a*_Opacity));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
