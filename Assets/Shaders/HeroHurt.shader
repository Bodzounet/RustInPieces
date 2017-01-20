Shader "Hidden/HeroHurt"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_hurtIntensity ("Intensity", Range(0, 1)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
 
			#include "UnityCG.cginc"
 
			uniform sampler2D _MainTex;
			uniform float _hurtIntensity;
 
			float4 frag(v2f_img i) : COLOR 
			{
				float4 c = tex2D(_MainTex, i.uv);

				float r = sqrt(pow(i.uv.x - 0.5f, 2) + pow(i.uv.y - 0.5f, 2)) * _hurtIntensity;
				if (r > 0.f)
				c.r += r;

				
				/*float lum = c.r*.3 + c.g*.59 + c.b*.11;
				float3 bw = float3( lum, lum, lum ); 
				
				float4 result = c;
				result.rgb = lerp(c.rgb, bw, _hurtIntensity);*/
				return c;
			}
			ENDCG
		}
	}
}
