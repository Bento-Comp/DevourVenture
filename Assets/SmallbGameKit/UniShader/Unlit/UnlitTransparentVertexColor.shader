// Upgrade NOTE: replaced 'SeperateSpecular' with 'SeparateSpecular'

Shader "UniShader/Unlit/Transparent Vertex Color" 
{
	Properties 
	{
	}

	SubShader
	{
		BindChannels 
		{
	        Bind "Color", color
	        Bind "Vertex", vertex
	    }
    
		ZWrite Off
    	Alphatest Greater 0
    	Tags {"Queue" = "Transparent" }
    	Blend SrcAlpha OneMinusSrcAlpha 
	
		Pass 
		{
			Lighting Off
		}
	}
}