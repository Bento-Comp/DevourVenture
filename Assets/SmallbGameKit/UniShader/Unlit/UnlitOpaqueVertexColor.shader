Shader "UniShader/Unlit/Opaque Vertex Color" 
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
    
    	Tags { "RenderType" = "Opaque" }
	
		Pass 
		{
			Lighting Off
		}
	}
}