Shader "UniShader/Unlit/Opaque Color" 
{
	Properties 
	{
			_Color ("Main Color", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
	
		Pass 
		{
			Color[_Color]
		}
	}
}