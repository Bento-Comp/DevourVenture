// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Mobile/Particles/Multiply (Double)" {
Properties {
	_MainTex ("Particle Texture", 2D) = "white" {}
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend DstColor SrcColor
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off Fog { Color (0.5,0.5,0.5,0.5) }
	
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColor;
			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};
			
			float4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}

			float _InvFade;
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;
				fixed4 tex = tex2D(_MainTex, i.texcoord);
				col.rgb = tex.rgb * i.color.rgb * 2;
				col.a = i.color.a * tex.a;
				return lerp(fixed4(0.5f,0.5f,0.5f,0.5f), col, col.a);
			}
			ENDCG 
		}
	}
}
}
