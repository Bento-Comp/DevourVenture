Shader "UniShader/Unlit/Transparent Texture x Color"
{
	Properties 
	{
			_Color ("Main Color", Color) = (1,1,1,1)
			_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 
	
		Pass 
		{
			SetTexture [_MainTex] 
			{
				constantColor [_Color] 
				combine texture * constant
			}
		}
	}
}