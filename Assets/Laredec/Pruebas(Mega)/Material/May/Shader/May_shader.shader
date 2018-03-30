// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:True,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:35109,y:32854,varname:node_9361,prsc:2|normal-9538-OUT,custl-9680-OUT,olwid-1908-OUT,olcol-6035-RGB;n:type:ShaderForge.SFN_AmbientLight,id:7528,x:34310,y:32693,varname:node_7528,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2460,x:34617,y:32670,cmnt:Ambient Light,varname:node_2460,prsc:2|A-1370-OUT,B-7528-RGB,C-9010-OUT;n:type:ShaderForge.SFN_Tex2d,id:5563,x:31043,y:32712,ptovrint:False,ptlb:RMA,ptin:_RMA,varname:_RMA,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:5cd39045115ea6a41b5313fb12600d28,ntxv:0,isnm:False;n:type:ShaderForge.SFN_NormalVector,id:3476,x:31477,y:32491,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:1685,x:32027,y:32559,varname:node_1685,prsc:2,dt:0|A-3476-OUT,B-4321-OUT;n:type:ShaderForge.SFN_Multiply,id:9733,x:32232,y:32543,varname:node_9733,prsc:2|A-1685-OUT,B-9841-OUT;n:type:ShaderForge.SFN_Add,id:3033,x:32406,y:32543,varname:node_3033,prsc:2|A-9733-OUT,B-9841-OUT;n:type:ShaderForge.SFN_Tex2d,id:9967,x:31060,y:33311,ptovrint:False,ptlb:Sss,ptin:_Sss,varname:_Sss,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:1fcd93297305fb8478d88d0eb079e2c1,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Set,id:1819,x:31237,y:32843,varname:sp_size,prsc:2|IN-5563-B;n:type:ShaderForge.SFN_Set,id:3401,x:31237,y:32744,varname:shadow_map,prsc:2|IN-5563-G;n:type:ShaderForge.SFN_Set,id:207,x:31237,y:33074,varname:Difuse,prsc:2|IN-5023-RGB;n:type:ShaderForge.SFN_Set,id:4892,x:31237,y:32641,varname:Sp_gradient,prsc:2|IN-5563-R;n:type:ShaderForge.SFN_Set,id:9764,x:31237,y:33311,varname:Sss,prsc:2|IN-9967-RGB;n:type:ShaderForge.SFN_Get,id:9841,x:32006,y:32452,varname:node_9841,prsc:2|IN-3401-OUT;n:type:ShaderForge.SFN_Round,id:3806,x:32353,y:32788,varname:node_3806,prsc:2|IN-3378-OUT;n:type:ShaderForge.SFN_Vector1,id:4153,x:31054,y:32396,varname:node_4153,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:8586,x:31038,y:32523,varname:node_8586,prsc:2,v1:2;n:type:ShaderForge.SFN_Set,id:5410,x:31215,y:32396,varname:Setp_light,prsc:2|IN-4153-OUT;n:type:ShaderForge.SFN_Set,id:5127,x:31226,y:32523,varname:Step_Shadow,prsc:2|IN-8586-OUT;n:type:ShaderForge.SFN_Multiply,id:3378,x:32167,y:32788,varname:node_3378,prsc:2|A-4789-OUT,B-9337-OUT;n:type:ShaderForge.SFN_Divide,id:6741,x:32528,y:32788,varname:node_6741,prsc:2|A-3806-OUT,B-9337-OUT;n:type:ShaderForge.SFN_Get,id:9337,x:32094,y:32948,varname:node_9337,prsc:2|IN-5127-OUT;n:type:ShaderForge.SFN_Tex2d,id:5023,x:31060,y:33074,ptovrint:False,ptlb:Alvedo,ptin:_Alvedo,varname:_Alvedo,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:2adac77551146d44d81f41dc74d5e634,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Set,id:302,x:32972,y:32788,varname:Base_shadow,prsc:2|IN-7721-OUT;n:type:ShaderForge.SFN_Set,id:2788,x:33078,y:33142,varname:SP_Light,prsc:2|IN-691-OUT;n:type:ShaderForge.SFN_Get,id:1370,x:34289,y:32628,varname:node_1370,prsc:2|IN-9764-OUT;n:type:ShaderForge.SFN_Get,id:9223,x:33415,y:33079,varname:node_9223,prsc:2|IN-207-OUT;n:type:ShaderForge.SFN_Vector3,id:9538,x:34637,y:32531,varname:node_9538,prsc:2,v1:0,v2:0,v3:1;n:type:ShaderForge.SFN_Get,id:5876,x:33229,y:33264,varname:node_5876,prsc:2|IN-302-OUT;n:type:ShaderForge.SFN_Get,id:4725,x:33260,y:33142,varname:node_4725,prsc:2|IN-9764-OUT;n:type:ShaderForge.SFN_Get,id:8433,x:33012,y:33717,varname:node_8433,prsc:2|IN-1819-OUT;n:type:ShaderForge.SFN_Add,id:9474,x:34077,y:33303,varname:node_9474,prsc:2|A-5077-OUT,B-5305-OUT;n:type:ShaderForge.SFN_Blend,id:9680,x:34650,y:33180,varname:node_9680,prsc:2,blmd:1,clmp:True|SRC-8097-OUT,DST-8101-OUT;n:type:ShaderForge.SFN_Set,id:1277,x:31237,y:32945,varname:lines,prsc:2|IN-5563-A;n:type:ShaderForge.SFN_Get,id:3575,x:34033,y:33124,varname:node_3575,prsc:2|IN-1277-OUT;n:type:ShaderForge.SFN_Multiply,id:8101,x:34272,y:33266,varname:node_8101,prsc:2|A-9474-OUT,B-9300-RGB;n:type:ShaderForge.SFN_LightColor,id:9300,x:34077,y:33542,varname:node_9300,prsc:2;n:type:ShaderForge.SFN_Vector1,id:5426,x:32752,y:33027,varname:node_5426,prsc:2,v1:1;n:type:ShaderForge.SFN_Clamp,id:7721,x:32752,y:32788,varname:node_7721,prsc:2|IN-6741-OUT,MIN-6582-OUT,MAX-5426-OUT;n:type:ShaderForge.SFN_Vector1,id:6582,x:32752,y:32970,varname:node_6582,prsc:2,v1:0.2;n:type:ShaderForge.SFN_Vector1,id:9010,x:34396,y:32829,varname:node_9010,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Power,id:3133,x:31821,y:33086,varname:node_3133,prsc:2|VAL-589-OUT,EXP-8945-OUT;n:type:ShaderForge.SFN_Fresnel,id:6052,x:31621,y:33294,varname:node_6052,prsc:2|EXP-9222-OUT;n:type:ShaderForge.SFN_Multiply,id:1003,x:31795,y:33294,varname:node_1003,prsc:2|A-6052-OUT,B-8756-OUT;n:type:ShaderForge.SFN_Get,id:8756,x:31481,y:33476,varname:node_8756,prsc:2|IN-4892-OUT;n:type:ShaderForge.SFN_Vector1,id:9222,x:31421,y:33328,varname:node_9222,prsc:2,v1:1;n:type:ShaderForge.SFN_Add,id:9103,x:31988,y:33143,varname:node_9103,prsc:2|A-3133-OUT,B-1003-OUT;n:type:ShaderForge.SFN_Dot,id:589,x:31619,y:32953,varname:node_589,prsc:2,dt:1|A-3020-OUT,B-8263-OUT;n:type:ShaderForge.SFN_ConstantLerp,id:8945,x:31619,y:33143,varname:node_8945,prsc:2,a:0,b:5|IN-5378-OUT;n:type:ShaderForge.SFN_Vector1,id:5378,x:31421,y:33143,varname:node_5378,prsc:2,v1:1;n:type:ShaderForge.SFN_Lerp,id:8171,x:33401,y:33517,varname:node_8171,prsc:2|A-1862-OUT,B-6405-OUT,T-2704-OUT;n:type:ShaderForge.SFN_Clamp01,id:691,x:32878,y:33142,varname:node_691,prsc:2|IN-9318-OUT;n:type:ShaderForge.SFN_LightVector,id:4321,x:31477,y:32641,varname:node_4321,prsc:2;n:type:ShaderForge.SFN_HalfVector,id:3020,x:31421,y:32864,varname:node_3020,prsc:2;n:type:ShaderForge.SFN_ViewReflectionVector,id:8263,x:31421,y:33019,varname:node_8263,prsc:2;n:type:ShaderForge.SFN_Get,id:288,x:32636,y:33408,varname:node_288,prsc:2|IN-2788-OUT;n:type:ShaderForge.SFN_Multiply,id:5077,x:33648,y:33214,varname:node_5077,prsc:2|A-9223-OUT,B-1474-OUT;n:type:ShaderForge.SFN_Blend,id:1474,x:33451,y:33244,varname:node_1474,prsc:2,blmd:10,clmp:True|SRC-4725-OUT,DST-5876-OUT;n:type:ShaderForge.SFN_Vector1,id:1862,x:33106,y:33422,varname:node_1862,prsc:2,v1:0;n:type:ShaderForge.SFN_Get,id:5450,x:32490,y:33506,varname:node_5450,prsc:2|IN-4892-OUT;n:type:ShaderForge.SFN_Vector1,id:2531,x:32477,y:33622,varname:node_2531,prsc:2,v1:0.4;n:type:ShaderForge.SFN_Add,id:1745,x:32944,y:33554,varname:node_1745,prsc:2|A-288-OUT,B-1917-OUT;n:type:ShaderForge.SFN_If,id:4385,x:32184,y:33153,varname:node_4385,prsc:2|A-8463-OUT,B-483-OUT,GT-8463-OUT,EQ-8463-OUT,LT-9103-OUT;n:type:ShaderForge.SFN_Get,id:483,x:31967,y:33086,varname:node_483,prsc:2|IN-4892-OUT;n:type:ShaderForge.SFN_Vector1,id:8463,x:31988,y:33316,varname:node_8463,prsc:2,v1:0;n:type:ShaderForge.SFN_Multiply,id:9052,x:32355,y:33175,varname:node_9052,prsc:2|A-4385-OUT,B-520-OUT;n:type:ShaderForge.SFN_Round,id:569,x:32541,y:33175,varname:node_569,prsc:2|IN-9052-OUT;n:type:ShaderForge.SFN_Divide,id:9318,x:32716,y:33175,varname:node_9318,prsc:2|A-569-OUT,B-520-OUT;n:type:ShaderForge.SFN_Get,id:520,x:32153,y:33343,varname:node_520,prsc:2|IN-5410-OUT;n:type:ShaderForge.SFN_Multiply,id:1917,x:32754,y:33649,varname:node_1917,prsc:2|A-5450-OUT,B-2531-OUT;n:type:ShaderForge.SFN_Multiply,id:6405,x:33143,y:33534,varname:node_6405,prsc:2|A-1745-OUT,B-1917-OUT;n:type:ShaderForge.SFN_Multiply,id:5305,x:33656,y:33517,varname:node_5305,prsc:2|A-8171-OUT,B-8433-OUT,C-6400-OUT;n:type:ShaderForge.SFN_Vector1,id:6400,x:33559,y:33735,varname:node_6400,prsc:2,v1:1.5;n:type:ShaderForge.SFN_VertexColor,id:437,x:32006,y:32330,varname:node_437,prsc:2;n:type:ShaderForge.SFN_Clamp,id:704,x:32336,y:32348,varname:node_704,prsc:2|IN-437-RGB,MIN-4261-OUT,MAX-4410-OUT;n:type:ShaderForge.SFN_Multiply,id:4789,x:32639,y:32543,varname:node_4789,prsc:2|A-3033-OUT,B-704-OUT;n:type:ShaderForge.SFN_Vector1,id:4410,x:32006,y:32242,varname:node_4410,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:4261,x:32006,y:32183,varname:node_4261,prsc:2,v1:0.7;n:type:ShaderForge.SFN_Blend,id:2704,x:33220,y:33664,varname:node_2704,prsc:2,blmd:5,clmp:True|SRC-5450-OUT,DST-8433-OUT;n:type:ShaderForge.SFN_Slider,id:1908,x:34539,y:33413,ptovrint:False,ptlb:Line,ptin:_Line,varname:_Line,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:0.4;n:type:ShaderForge.SFN_Color,id:6035,x:34617,y:33539,ptovrint:False,ptlb:cc,ptin:_cc,varname:_cc,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:0;n:type:ShaderForge.SFN_Multiply,id:6251,x:34318,y:32949,varname:node_6251,prsc:2|A-4906-OUT,B-3522-OUT;n:type:ShaderForge.SFN_Get,id:3522,x:34079,y:32787,varname:node_3522,prsc:2|IN-9764-OUT;n:type:ShaderForge.SFN_Lerp,id:8097,x:34484,y:33025,varname:node_8097,prsc:2|A-6251-OUT,B-3575-OUT,T-3575-OUT;n:type:ShaderForge.SFN_Slider,id:4906,x:33897,y:32988,ptovrint:False,ptlb:Line_color,ptin:_Line_color,varname:_Line_color,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:2;proporder:5563-9967-5023-1908-6035-4906;pass:END;sub:END;*/

Shader "Shader Forge/May_shader" {
    Properties {
        _RMA ("RMA", 2D) = "white" {}
        _Sss ("Sss", 2D) = "white" {}
        _Alvedo ("Alvedo", 2D) = "black" {}
        _Line ("Line", Range(0, 0.4)) = 0
        _cc ("cc", Color) = (0,0,0,0)
        _Line_color ("Line_color", Range(0, 2)) = 0
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
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float _Line;
            uniform float4 _cc;
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
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*_Line,1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                return fixed4(_cc.rgb,0);
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
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _RMA; uniform float4 _RMA_ST;
            uniform sampler2D _Sss; uniform float4 _Sss_ST;
            uniform sampler2D _Alvedo; uniform float4 _Alvedo_ST;
            uniform float _Line_color;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalLocal = float3(0,0,1);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float4 _Sss_var = tex2D(_Sss,TRANSFORM_TEX(i.uv0, _Sss));
                float3 Sss = _Sss_var.rgb;
                float4 _RMA_var = tex2D(_RMA,TRANSFORM_TEX(i.uv0, _RMA));
                float lines = _RMA_var.a;
                float node_3575 = lines;
                float4 _Alvedo_var = tex2D(_Alvedo,TRANSFORM_TEX(i.uv0, _Alvedo));
                float3 Difuse = _Alvedo_var.rgb;
                float shadow_map = _RMA_var.g;
                float node_9841 = shadow_map;
                float Step_Shadow = 2.0;
                float node_9337 = Step_Shadow;
                float3 Base_shadow = clamp((round(((((dot(i.normalDir,lightDirection)*node_9841)+node_9841)*clamp(i.vertexColor.rgb,0.7,1.0))*node_9337))/node_9337),0.2,1.0);
                float node_8463 = 0.0;
                float Sp_gradient = _RMA_var.r;
                float node_4385_if_leA = step(node_8463,Sp_gradient);
                float node_4385_if_leB = step(Sp_gradient,node_8463);
                float Setp_light = 1.0;
                float node_520 = Setp_light;
                float SP_Light = saturate((round((lerp((node_4385_if_leA*(pow(max(0,dot(halfDirection,viewReflectDirection)),lerp(0,5,1.0))+(pow(1.0-max(0,dot(normalDirection, viewDirection)),1.0)*Sp_gradient)))+(node_4385_if_leB*node_8463),node_8463,node_4385_if_leA*node_4385_if_leB)*node_520))/node_520));
                float node_5450 = Sp_gradient;
                float node_1917 = (node_5450*0.4);
                float sp_size = _RMA_var.b;
                float node_8433 = sp_size;
                float3 finalColor = saturate((lerp((_Line_color*Sss),float3(node_3575,node_3575,node_3575),node_3575)*(((Difuse*saturate(( Base_shadow > 0.5 ? (1.0-(1.0-2.0*(Base_shadow-0.5))*(1.0-Sss)) : (2.0*Base_shadow*Sss) )))+(lerp(0.0,((SP_Light+node_1917)*node_1917),saturate(max(node_5450,node_8433)))*node_8433*1.5))*_LightColor0.rgb)));
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
            uniform sampler2D _RMA; uniform float4 _RMA_ST;
            uniform sampler2D _Sss; uniform float4 _Sss_ST;
            uniform sampler2D _Alvedo; uniform float4 _Alvedo_ST;
            uniform float _Line_color;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 vertexColor : COLOR;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalLocal = float3(0,0,1);
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float4 _Sss_var = tex2D(_Sss,TRANSFORM_TEX(i.uv0, _Sss));
                float3 Sss = _Sss_var.rgb;
                float4 _RMA_var = tex2D(_RMA,TRANSFORM_TEX(i.uv0, _RMA));
                float lines = _RMA_var.a;
                float node_3575 = lines;
                float4 _Alvedo_var = tex2D(_Alvedo,TRANSFORM_TEX(i.uv0, _Alvedo));
                float3 Difuse = _Alvedo_var.rgb;
                float shadow_map = _RMA_var.g;
                float node_9841 = shadow_map;
                float Step_Shadow = 2.0;
                float node_9337 = Step_Shadow;
                float3 Base_shadow = clamp((round(((((dot(i.normalDir,lightDirection)*node_9841)+node_9841)*clamp(i.vertexColor.rgb,0.7,1.0))*node_9337))/node_9337),0.2,1.0);
                float node_8463 = 0.0;
                float Sp_gradient = _RMA_var.r;
                float node_4385_if_leA = step(node_8463,Sp_gradient);
                float node_4385_if_leB = step(Sp_gradient,node_8463);
                float Setp_light = 1.0;
                float node_520 = Setp_light;
                float SP_Light = saturate((round((lerp((node_4385_if_leA*(pow(max(0,dot(halfDirection,viewReflectDirection)),lerp(0,5,1.0))+(pow(1.0-max(0,dot(normalDirection, viewDirection)),1.0)*Sp_gradient)))+(node_4385_if_leB*node_8463),node_8463,node_4385_if_leA*node_4385_if_leB)*node_520))/node_520));
                float node_5450 = Sp_gradient;
                float node_1917 = (node_5450*0.4);
                float sp_size = _RMA_var.b;
                float node_8433 = sp_size;
                float3 finalColor = saturate((lerp((_Line_color*Sss),float3(node_3575,node_3575,node_3575),node_3575)*(((Difuse*saturate(( Base_shadow > 0.5 ? (1.0-(1.0-2.0*(Base_shadow-0.5))*(1.0-Sss)) : (2.0*Base_shadow*Sss) )))+(lerp(0.0,((SP_Light+node_1917)*node_1917),saturate(max(node_5450,node_8433)))*node_8433*1.5))*_LightColor0.rgb)));
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
