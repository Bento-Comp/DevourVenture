Shader "UniShader/Unlit/Opaque Texture x Color (New)"
{
    Properties
	{
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    }

    SubShader
	{
		Tags { "RenderType" = "Opaque" }

        Lighting Off
        Fog { Mode Off }

        Pass
		{
            Color [_Color]
            SetTexture [_MainTex] { combine texture * primary } 
        }
    }
}