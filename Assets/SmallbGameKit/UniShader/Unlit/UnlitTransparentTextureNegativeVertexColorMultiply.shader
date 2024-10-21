// Upgrade NOTE: replaced 'SeperateSpecular' with 'SeparateSpecular'

Shader "UniShader/Unlit/Transparent Negative Texture x Vertex Color" 
{
	Properties 
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}

	SubShader
	{
		BindChannels 
		{
	        Bind "Color", color
	        Bind "Vertex", vertex
	        Bind "TexCoord", texcoord
	    }
    
		ZWrite Off
    	Alphatest Greater 0
    	Tags {"Queue" = "Transparent" }
    	Blend SrcAlpha OneMinusSrcAlpha 
	
		Pass 
		{
			Lighting Off
			
			// Texture
			SetTexture [_MainTex] 
			{
				combine one - texture * primary, texture * primary
			} 
		}
	}
}