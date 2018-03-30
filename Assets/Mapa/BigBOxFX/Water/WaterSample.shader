// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Wao3D/BigBox_water"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_WaterNormal("Water Normal", 2D) = "bump" {}
		_WaterNormal2("Water Normal 2", 2D) = "bump" {}
		_Water_Albedo("Water_Albedo", 2D) = "white" {}
		_NormalScale("Normal Scale", Float) = 0
		_AlbedoEmissionPower("AlbedoEmissionPower", Range( 0 , 1)) = 0.2
		_DeepColor("Deep Color", Color) = (0,0,0,0)
		_ShalowColor("Shalow Color", Color) = (1,1,1,0)
		_WaterDepth("Water Depth", Float) = 0
		_WaterFalloff("Water Falloff", Float) = 0
		_WaterSpecular("Water Specular", Float) = 0
		_WaterSmoothness("Water Smoothness", Float) = 0
		_Distortion("Distortion", Float) = 0.5
		_Foam("Foam", 2D) = "white" {}
		_FoamDepth("Foam Depth", Float) = 0
		_AlbedoDistorsion("AlbedoDistorsion", Range( 0 , 0.5)) = 0
		_TextureTile("TextureTile", Range( 0 , 100)) = 0
		_FoamFalloff("Foam Falloff", Float) = 0
		_FoamSpecular("Foam Specular", Float) = 0
		_FoamSmoothness("Foam Smoothness", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ "WaterGrab" }
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf StandardSpecular keepalpha vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
			float2 texcoord_0;
		};

		uniform half _NormalScale;
		uniform sampler2D _WaterNormal;
		uniform float4 _WaterNormal_ST;
		uniform half4 _DeepColor;
		uniform half4 _ShalowColor;
		uniform sampler2D _CameraDepthTexture;
		uniform half _WaterDepth;
		uniform half _WaterFalloff;
		uniform half _FoamDepth;
		uniform half _FoamFalloff;
		uniform sampler2D _Foam;
		uniform float4 _Foam_ST;
		uniform sampler2D WaterGrab;
		uniform half _Distortion;
		uniform sampler2D _Water_Albedo;
		uniform half _TextureTile;
		uniform sampler2D _WaterNormal2;
		uniform half _AlbedoDistorsion;
		uniform half _AlbedoEmissionPower;
		uniform half _WaterSpecular;
		uniform half _FoamSpecular;
		uniform half _WaterSmoothness;
		uniform half _FoamSmoothness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			half2 temp_cast_0 = (_TextureTile).xx;
			o.texcoord_0.xy = v.texcoord.xy * temp_cast_0 + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 uv_WaterNormal = i.uv_texcoord * _WaterNormal_ST.xy + _WaterNormal_ST.zw;
			float3 Normal = BlendNormals( UnpackScaleNormal( tex2D( _WaterNormal, (abs( uv_WaterNormal+_Time[1] * float2(-0.03,0 ))) ) ,_NormalScale ) , UnpackScaleNormal( tex2D( _WaterNormal, (abs( uv_WaterNormal+_Time[1] * float2(0.04,0.04 ))) ) ,_NormalScale ) );
			o.Normal = Normal;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPos2 = ase_screenPos;
			float eyeDepth1 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos2))));
			float Depth = abs( ( eyeDepth1 - ase_screenPos2.w ) );
			float temp_output_94_0 = saturate( pow( ( Depth + _WaterDepth ) , _WaterFalloff ) );
			float2 uv_Foam = i.uv_texcoord * _Foam_ST.xy + _Foam_ST.zw;
			float Spec = ( saturate( pow( ( Depth + _FoamDepth ) , _FoamFalloff ) ) * tex2D( _Foam, (abs( uv_Foam+_Time[1] * float2(-0.01,0.01 ))) ).r );
			float4 ase_screenPos66 = ase_screenPos;
			float2 appendResult67 = float2( ase_screenPos66.x , ase_screenPos66.y );
			float4 screenColor65 = tex2D( WaterGrab, ( half3( ( appendResult67 / ase_screenPos66.w ) ,  0.0 ) + ( Normal * _Distortion ) ).xy );
			float4 WaterGrab = screenColor65;
			float4 Diffuse = lerp( lerp( lerp( _DeepColor , _ShalowColor , temp_output_94_0 ) , half4(1,1,1,0) , Spec ) , WaterGrab , temp_output_94_0 );
			o.Albedo = Diffuse.rgb;
			half2 temp_cast_3 = (UnpackNormal( tex2D( _WaterNormal2, (abs( i.texcoord_0+_Time[1] * float2(0.01,0.01 ))) ) ).r).xx;
			half4 tex2DNode193 = tex2D( _Water_Albedo, lerp( i.texcoord_0 , temp_cast_3 , _AlbedoDistorsion ) );
			float4 Emmision = ( tex2DNode193 * pow( tex2DNode193 , tex2DNode193 ) * _AlbedoEmissionPower );
			o.Emission = Emmision.xyz;
			half3 temp_cast_5 = (lerp( _WaterSpecular , ( 1.0 - _FoamSpecular ) , Spec )).xxx;
			o.Specular = temp_cast_5;
			o.Smoothness = lerp( _WaterSmoothness , _FoamSmoothness , Spec );
			o.Occlusion = 0.0;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "BigBox"
}
/*ASEBEGIN
Version=11002
-266;518;1906;1014;1563.048;2516.789;1.3;True;False
Node;AmplifyShaderEditor.ScreenPosInputsNode;2;-2569.601,-1270;Float;False;1;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ScreenDepthNode;1;-2364.601,-1323.5;Float;False;0;1;0;FLOAT4;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-1587.106,-1706.385;Float;False;0;17;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;3;-2140.201,-1227.199;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.PannerNode;19;-1312.107,-1621.184;Float;False;0.04;0.04;2;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.AbsOpNode;89;-1955.005,-1229.383;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.PannerNode;22;-1314.407,-1733.684;Float;False;-0.03;0;2;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;48;-1345.608,-1426.384;Float;False;Property;_NormalScale;Normal Scale;3;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;172;-1787.206,-1206.384;Float;False;Depth;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;173;-1394.606,-25.8836;Float;False;172;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;212;-1722.649,-2128.383;Float;False;Property;_TextureTile;TextureTile;15;0;0;0;100;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;23;-949.606,-1727.685;Float;True;Property;_Normal2;Normal2;-1;0;None;True;0;True;bump;Auto;True;Instance;17;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1.0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;17;-948.4053,-1450.483;Float;True;Property;_WaterNormal;Water Normal;2;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1.0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;111;-1389.602,76.01846;Float;False;Property;_FoamDepth;Foam Depth;13;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;115;-1157.402,-5.882073;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.BlendNormalsNode;24;-513.6028,-1659.286;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.TextureCoordinatesNode;206;-1376.9,-2180.581;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ScreenPosInputsNode;66;-1441.604,-1195.884;Float;False;1;False;0;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;112;-1184.803,138.9186;Float;False;Property;_FoamFalloff;Foam Falloff;16;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;6;-1655,-435.8996;Float;False;Property;_WaterDepth;Water Depth;7;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;174;-1673.806,-512.6838;Float;False;172;0;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;106;-1432.303,248.519;Float;False;0;105;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;10;-1477.5,-386.9001;Float;False;Property;_WaterFalloff;Water Falloff;8;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.PowerNode;110;-995.6025,10.01844;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.PannerNode;205;-1093.999,-2093.881;Float;False;0.01;0.01;2;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.PannerNode;116;-1181.601,273.7181;Float;False;-0.01;0.01;2;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleAddOpNode;88;-1473.305,-512.2828;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;167;-237.4064,-1612.482;Float;False;Normal;-1;True;1;0;FLOAT3;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;97;-1207.104,-953.783;Float;False;Property;_Distortion;Distortion;11;0;0.5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.AppendNode;67;-1205.005,-1190.985;Float;False;FLOAT2;0;0;0;0;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.GetLocalVarNode;169;-1214.506,-1042.285;Float;False;167;0;1;FLOAT3
Node;AmplifyShaderEditor.PowerNode;87;-1297.105,-425.8831;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;207;-904.1002,-2081.98;Float;True;Property;_WaterNormal2;Water Normal 2;0;0;Assets/BigBox/Water/Water_Normal.png;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;211;-1134.049,-2373.382;Float;False;Property;_AlbedoDistorsion;AlbedoDistorsion;14;0;0;0;0.5;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;68;-1031.304,-1138.283;Float;False;2;0;FLOAT2;0.0,0;False;1;FLOAT;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;105;-942.8022,223.3185;Float;True;Property;_Foam;Foam;12;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;1.0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;-1029.003,-1030.383;Float;False;2;2;0;FLOAT3;0.0,0,0;False;1;FLOAT;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SaturateNode;113;-774.4012,58.01794;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;210;-563.6997,-2233.38;Float;False;3;0;FLOAT2;0.0,0,0;False;1;FLOAT;0,0;False;2;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleAddOpNode;96;-875.9042,-1097.283;Float;False;2;2;0;FLOAT2;0.0,0;False;1;FLOAT3;0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SaturateNode;94;-1076.704,-401.2838;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;11;-1296.399,-636.0002;Float;False;Property;_ShalowColor;Shalow Color;6;0;1,1,1,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;12;-1538.799,-725.2009;Float;False;Property;_DeepColor;Deep Color;5;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;-577.4011,154.0181;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;108;-876.9034,-571.7819;Float;False;Constant;_Color0;Color 0;-1;0;1,1,1,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;193;-373.3047,-2437.285;Float;True;Property;_Water_Albedo;Water_Albedo;1;0;Assets/BigBox/Water/Tile_Water.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;175;-683.7065,-478.3839;Float;False;163;0;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;163;-325.5073,147.4166;Float;False;Spec;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ScreenColorNode;65;-685.7029,-1101.083;Float;False;Global;WaterGrab;WaterGrab;-1;0;Object;-1;1;0;FLOAT2;0,0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;13;-915.7,-761.3999;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;117;-507.3032,-667.8817;Float;False;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.PowerNode;213;159.753,-2453.69;Float;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0.0;False;1;FLOAT4
Node;AmplifyShaderEditor.RangedFloatNode;186;-344.1072,-2174.086;Float;False;Property;_AlbedoEmissionPower;AlbedoEmissionPower;4;0;0.2;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;131;1212.596,-771.98;Float;False;Property;_FoamSpecular;Foam Specular;17;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;176;-458.0061,-1125.084;Float;False;WaterGrab;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.GetLocalVarNode;177;-459.3061,-292.5836;Float;False;176;0;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;26;1198.396,-582.5855;Float;False;Property;_WaterSmoothness;Water Smoothness;10;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;166;1216.893,-404.0836;Float;False;163;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;203;449.9944,-2331.183;Float;False;3;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.LerpOp;93;-146.6045,-546.7849;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0;False;1;COLOR
Node;AmplifyShaderEditor.OneMinusNode;178;1479.193,-737.584;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;104;1233.597,-891.8812;Float;False;Property;_WaterSpecular;Water Specular;9;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;165;1230.893,-680.0831;Float;False;163;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;132;1199.598,-501.8801;Float;False;Property;_FoamSmoothness;Foam Smoothness;18;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;103;1540.295,-260.7843;Float;False;Constant;_Occlusion;Occlusion;-1;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;170;164.793,-542.3835;Float;False;Diffuse;-1;True;1;0;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.RegisterLocalVarNode;187;618.8929,-2369.184;Float;False;Emmision;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4
Node;AmplifyShaderEditor.GetLocalVarNode;168;1775.194,-1011.183;Float;False;167;0;1;FLOAT3
Node;AmplifyShaderEditor.GetLocalVarNode;188;1765.294,-868.0846;Float;False;187;0;1;FLOAT4
Node;AmplifyShaderEditor.GetLocalVarNode;171;1775.194,-1110.783;Float;False;170;0;1;COLOR
Node;AmplifyShaderEditor.LerpOp;133;1560.797,-503.08;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;130;1734.096,-746.3801;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2408.303,-829.1998;Half;False;True;2;Half;BigBox;0;StandardSpecular;Wao3D/BigBox_water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;3;False;0;0;Translucent;0.5;True;False;0;False;Opaque;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;Relative;0;;-1;-1;-1;-1;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0.0,0,0;False;7;FLOAT3;0.0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0.0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;0;2;0
WireConnection;3;0;1;0
WireConnection;3;1;2;4
WireConnection;19;0;21;0
WireConnection;89;0;3;0
WireConnection;22;0;21;0
WireConnection;172;0;89;0
WireConnection;23;1;22;0
WireConnection;23;5;48;0
WireConnection;17;1;19;0
WireConnection;17;5;48;0
WireConnection;115;0;173;0
WireConnection;115;1;111;0
WireConnection;24;0;23;0
WireConnection;24;1;17;0
WireConnection;206;0;212;0
WireConnection;110;0;115;0
WireConnection;110;1;112;0
WireConnection;205;0;206;0
WireConnection;116;0;106;0
WireConnection;88;0;174;0
WireConnection;88;1;6;0
WireConnection;167;0;24;0
WireConnection;67;0;66;1
WireConnection;67;1;66;2
WireConnection;87;0;88;0
WireConnection;87;1;10;0
WireConnection;207;1;205;0
WireConnection;68;0;67;0
WireConnection;68;1;66;4
WireConnection;105;1;116;0
WireConnection;98;0;169;0
WireConnection;98;1;97;0
WireConnection;113;0;110;0
WireConnection;210;0;206;0
WireConnection;210;1;207;1
WireConnection;210;2;211;0
WireConnection;96;0;68;0
WireConnection;96;1;98;0
WireConnection;94;0;87;0
WireConnection;114;0;113;0
WireConnection;114;1;105;1
WireConnection;193;1;210;0
WireConnection;163;0;114;0
WireConnection;65;0;96;0
WireConnection;13;0;12;0
WireConnection;13;1;11;0
WireConnection;13;2;94;0
WireConnection;117;0;13;0
WireConnection;117;1;108;0
WireConnection;117;2;175;0
WireConnection;213;0;193;0
WireConnection;213;1;193;0
WireConnection;176;0;65;0
WireConnection;203;0;193;0
WireConnection;203;1;213;0
WireConnection;203;2;186;0
WireConnection;93;0;117;0
WireConnection;93;1;177;0
WireConnection;93;2;94;0
WireConnection;178;0;131;0
WireConnection;170;0;93;0
WireConnection;187;0;203;0
WireConnection;133;0;26;0
WireConnection;133;1;132;0
WireConnection;133;2;166;0
WireConnection;130;0;104;0
WireConnection;130;1;178;0
WireConnection;130;2;165;0
WireConnection;0;0;171;0
WireConnection;0;1;168;0
WireConnection;0;2;188;0
WireConnection;0;3;130;0
WireConnection;0;4;133;0
WireConnection;0;5;103;0
ASEEND*/
//CHKSM=B0E38A3C0A73C6148CD8305D99C34310C64E321D