// Upgrade NOTE: replaced 'SeperateSpecular' with 'SeparateSpecular'

Shader "UniShader/Unlit/Opaque Texture x Vertex Color" 
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
    
  	 	Tags { "RenderType" = "Opaque" }
	 	
	
		Pass 
		{
			Lighting Off
			
			// Texture
			SetTexture [_MainTex] 
			{
				combine texture * primary
			} 
		}
	}
}