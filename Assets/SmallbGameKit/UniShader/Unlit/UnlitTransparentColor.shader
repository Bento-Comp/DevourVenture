Shader "UniShader/Unlit/Transparent Color" 
{
	Properties 
	{
			_Color ("Main Color", Color) = (1,1,1,1)
	}

	SubShader
	{		
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 
	
		Pass 
		{
			Lighting Off
			Color[_Color]
		}
	}
}