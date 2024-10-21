Shader "UniShader/Unlit/3D Text" 
{
	Properties 
	{
		_MainTex ("Font Texture", 2D) = "white" {}
	}
 
	SubShader
	{
		BindChannels 
		{
	        Bind "Color", color
	        Bind "Vertex", vertex
	        Bind "TexCoord", texcoord
	    }
	    
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Lighting Off Cull Off ZWrite Off Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha
		Pass 
		{
			Color [_Color]
			SetTexture [_MainTex]
			{
				combine primary, texture * primary
			}
		}
	}
}