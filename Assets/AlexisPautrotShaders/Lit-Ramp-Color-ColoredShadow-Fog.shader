Shader "Lit Shaders/Ramp-Color-ColoredShadow-Fog"
{
    Properties
    {
		_Color("Color", Color) = (1, 1, 1, 1)
		_ShadowColor("Shadow Color", Color) = (0.75,0.75,0.75,1.0)
		[Space][NoScaleOffset]
		_RampTex("Ramp", 2D) = "white" {}
    }
    SubShader
    {
        Tags
		{
			"RenderType"="Opaque"
			"Queue" = "Geometry"
            "LightMode" = "ForwardBase"
		}
        LOD 100

        Pass
        {
			Name "Normal"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
            };

			struct v2f
			{
				float4 pos : SV_POSITION;
				UNITY_SHADOW_COORDS(1)
				UNITY_FOG_COORDS(2)
				float4 light : COLOR0;
				float3 ambient : COLOR1;
				float3 worldPos : TEXCOORD3;
			};

			sampler2D _MainTex;
			sampler2D _RampTex;
			float3 _Color;
			float3 _ShadowColor;

			v2f vert(appdata v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);

				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				UNITY_TRANSFER_SHADOW(o, o.uv1);
				UNITY_TRANSFER_FOG(o, o.pos);

				float3 worldNormal = UnityObjectToWorldNormal(v.normal);
				float3 ambient = ShadeSH9(half4 (worldNormal, 1));

				o.ambient = (ambient + (_LightColor0.rgb * _ShadowColor)) * _Color;
				o.light.rgb = (ambient + _LightColor0.rgb ) * _Color;

				float NdotL = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				o.light.a = NdotL;

				return o;
			}

			float4 frag(v2f input) : COLOR
			{
				float rampValue = tex2D(_RampTex, float2 (input.light.a, 0.5)).r;
				UNITY_LIGHT_ATTENUATION(attenuationValue, input, input.worldPos)
				float lightingValue = rampValue * attenuationValue;

				float3 lightingColor = lerp(input.ambient, input.light, lightingValue);
				UNITY_APPLY_FOG(input.fogCoord, lightingColor);
				return float4 ( lightingColor, 1 );
			}
			ENDCG
        }

		// shadow casting support
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
