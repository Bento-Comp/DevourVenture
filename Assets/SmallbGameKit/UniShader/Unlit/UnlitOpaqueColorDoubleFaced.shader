Shader "UniShader/Unlit/Opaque Color Double Faced" 
{
	Properties 
	{
			_Color ("Main Color", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
	Cull Off
		Pass 
		{
			Color[_Color]
		}
	}
}