Shader "UniShader/Unlit/Opaque Color 2" 
{
	Properties 
	{
			_Color2 ("Main Color", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
	
		Pass 
		{
			Color[_Color2]
		}
	}
}