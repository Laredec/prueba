Shader "Hypeful/Toon Shader" {
    Properties {
        _color_map ("color_map", 2D) = "white" {}
        _mask_map ("mask_map", 2D) = "white" {}
        _sss_map ("sss_map", 2D) = "white" {}
        _fresnel_threshold ("fresnel_threshold", Range(-1, 0)) = -0.25
        _fresnel_intensity ("fresnel_intensity", Range(0, 15)) = 5
        _specular_intensity ("specular_intensity", Range(0, 10)) = 5
        _ilumination_steps ("ilumination_steps", Float ) = 1
        _shadow_steps ("shadow_steps", Float ) = 1
        _line_width_multiplier ("line_width_multiplier", Range(0, 1)) = 0.15
        _outline_lightness ("outline lightness", Range(-1, 1)) = -0.5
    }
    SubShader {
        Tags {
            "Queue"="Geometry"
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
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
			#include "UnityCG.cginc"
            
			uniform sampler2D _color_map; 
			uniform fixed4 _color_map_ST;
            uniform sampler2D _sss_map; 
			uniform fixed4 _sss_map_ST;
            uniform fixed4 _alpha_map_ST;
            
			uniform fixed _line_width_multiplier;
            uniform fixed _outline_lightness;
            
			struct VertexInput {
                fixed4 vertex : POSITION;
                fixed3 normal : NORMAL;
                fixed2 texcoord0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                fixed vertex_line_width = o.vertexColor.g;
                fixed outline_width = (vertex_line_width*(_line_width_multiplier/10.0));
                o.pos = UnityObjectToClipPos( fixed4(v.vertex.xyz + v.normal*outline_width,1) );
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                fixed4 _sss_map_var = tex2D(_sss_map,TRANSFORM_TEX(i.uv0, _sss_map));
                fixed3 shadow_tint_map = _sss_map_var.rgb;

                fixed4 _color_map_var = tex2D(_color_map,TRANSFORM_TEX(i.uv0, _color_map));
                fixed3 albedo_map = _color_map_var.rgb;

                fixed3 shaded_color = (shadow_tint_map*albedo_map);
                fixed3 outline_color = ((_outline_lightness+1.0)*shaded_color);

                return fixed4(outline_color,0);
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
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 
            #pragma target 3.0
			#include "UnityCG.cginc"
           
            uniform fixed4 _LightColor0;
            uniform sampler2D _color_map; 
			uniform fixed4 _color_map_ST;
            uniform sampler2D _sss_map; 
			uniform fixed4 _sss_map_ST;
            uniform sampler2D _mask_map; 
			uniform fixed4 _mask_map_ST;
            
			uniform fixed _ilumination_steps;
            uniform fixed _shadow_steps;
            uniform fixed _specular_intensity;
            uniform fixed _fresnel_threshold;
            uniform fixed _fresnel_intensity;

            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed3 normal : NORMAL;
                fixed2 texcoord0 : TEXCOORD0;
                fixed4 vertexColor : COLOR;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;
                fixed2 uv0 : TEXCOORD0;
                fixed4 posWorld : TEXCOORD1;
                fixed3 normalDir : TEXCOORD2;
                fixed4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                fixed3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }

            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                fixed3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                //fixed vertex_line_width = i.vertexColor.g; //not in use (yet)
                fixed3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                fixed3 lightColor = _LightColor0.rgb;
                fixed3 halfDirection = normalize(viewDirection+lightDirection);

				fixed4 mask_map = tex2D(_mask_map,TRANSFORM_TEX(i.uv0, _mask_map)); //get our masks from RGBA channels
                fixed additional_shadows_mask = mask_map.g;
                fixed specular_intensity_mask = mask_map.r;
                fixed specular_glossiness_mask = mask_map.b;
                fixed line_mask = mask_map.a;
                
				fixed3 shadow_tint_map = tex2D(_sss_map,TRANSFORM_TEX(i.uv0, _sss_map)).rgb;
				fixed3 albedo_map = tex2D(_color_map,TRANSFORM_TEX(i.uv0, _color_map)).rgb;
                
				fixed3 shaded_color = (shadow_tint_map*albedo_map);
                
				//HIGHLIGHTS
				fixed base_fresnel = pow(1.0-max(0,dot(i.normalDir, viewDirection)),(specular_intensity_mask*_fresnel_intensity)); //fresnel effect for high specularity elements
				fixed computed_fresnel = saturate(((_fresnel_threshold*specular_intensity_mask) + base_fresnel)); //compute fresnel using threshold
                fixed computed_specular_reflection = max(0,dot(i.normalDir,halfDirection)); //only positive values in order to supress artifacts
                fixed computed_intensity = saturate(pow(computed_specular_reflection,exp2((specular_intensity_mask*_specular_intensity))));
                fixed computed_specular_gloss = dot(saturate((computed_fresnel+computed_intensity)),specular_intensity_mask);
                fixed ramped_specular = (round((computed_specular_gloss*_ilumination_steps))/_ilumination_steps);  //discreet values
				
				//SHADOWS
				fixed base_specular = max(0,dot(i.normalDir,lightDirection)); //only positive values in order to supress shadows artifacts
				fixed computed_additional_shadows = saturate((additional_shadows_mask+(additional_shadows_mask*base_specular)));
                fixed ramped_shadows = (round((computed_additional_shadows*_shadow_steps))/_shadow_steps); //discreet values
                
				fixed3 shaded_albedo = lerp(shaded_color,albedo_map,ramped_shadows); //lerp between shadows and base color deppending on shadow incidence
                
				fixed3 final_color = (line_mask*((_LightColor0.rgb*shaded_albedo)+((_LightColor0.rgb*ramped_specular)*specular_glossiness_mask*0.5))); //add shaded albedo and highlights
                
				return fixed4(final_color,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
