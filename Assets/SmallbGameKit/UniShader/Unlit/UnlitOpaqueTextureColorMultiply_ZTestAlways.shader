// Upgrade NOTE: replaced 'SeperateSpecular' with 'SeparateSpecular'

Shader "UniShader/Unlit/Opaque Texture x Color ZTest Always" 
{
	Properties 
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
	}

	SubShader
	{
  	 	Tags { "RenderType" = "Opaque" }
	 	ZTest Always
	
		Pass 
		{
			Lighting Off
			
			// Texture
			SetTexture [_MainTex] 
			{
				constantColor [_Color] 
				combine texture * constant
			} 
		}
	}
}