// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'SeperateSpecular' with 'SeparateSpecular'

Shader "UniShader/Unlit/Opaque Sprite" 
{
	Properties 
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader
	{
  	 	Tags { "RenderType" = "Opaque" }
	 	
		Pass 
		{
			CGPROGRAM
			 #pragma vertex vert
			 #pragma fragment frag
			 
			 #include "UnityCG.cginc"
			 
			 sampler2D _MainTex;
			 
			 struct appdata 
			 {
            	float4 vertex : POSITION;
            	float4 texcoord : TEXCOORD0;
            	float4 color : COLOR;
            };
        
			 struct v2f 
			 {
			     float4  pos : SV_POSITION;
			     float2  uv : TEXCOORD0;
			     float4 color : COLOR;
			 };
			 
			 float4 _MainTex_ST;
			 
			 v2f vert (appdata v)
			 {
			     v2f o;
			     o.pos = UnityObjectToClipPos (v.vertex);
			     o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
			     o.color = v.color;
			     return o;
			 }
			 
			 half4 frag (v2f i) : COLOR
			 {
			     half4 texcol = tex2D (_MainTex, i.uv);
			     return texcol * i.color;
			 }
			 ENDCG
		}
	}
}