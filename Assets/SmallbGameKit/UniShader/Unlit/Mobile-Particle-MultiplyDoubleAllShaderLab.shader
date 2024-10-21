// Simplified Multiply Particle shader. Differences from regular Multiply Particle one:
// - no Smooth particle support
// - no AlphaTest
// - no ColorMask

Shader "Mobile/Particles/Multiply (Double All Shader Lab)" 
{
	Properties
	{
		_MainTex ("Particle Texture", 2D) = "white" {}
	}

	Category 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend DstColor SrcColor
		Cull Off Lighting Off ZWrite Off Fog { Color (0.5,0.5,0.5,0.5) }
		
		BindChannels 
		{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		
		SubShader
		{
			Pass
			{		
				SetTexture [_MainTex] 
				{
					Combine texture * primary DOUBLE
				}
				
				SetTexture [_MainTex] 
				{
					constantColor (0.5,0.5,0.5,0.5)
					combine previous lerp (previous) constant
				}
			}
		}
	}
}
