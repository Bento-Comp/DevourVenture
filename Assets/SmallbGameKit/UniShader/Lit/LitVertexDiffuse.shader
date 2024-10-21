Shader "UniShader/Lit/Diffuse/Texture x Vertex Color" 
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
			ColorMaterial AmbientAndDiffuse
            
			Lighting On
			
			SetTexture [_MainTex] 
			{
            	Combine texture * primary DOUBLE
        	}

		}
	}
}