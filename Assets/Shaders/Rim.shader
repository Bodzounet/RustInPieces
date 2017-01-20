Shader "Custom/Test" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color Text", Color) = (1, 1, 1, 1)
		_RimColor ("Color Rim", Color) = (1, 1, 1, 1)
		_Effect ("Rim Effect", Range(0, 10)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float4 _Color;
		float4 _RimColor;
		float _Effect;

		struct Input {
			float2 uv_MainTex;
			 float3 viewDir;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _Color.rgb;
			o.Alpha = c.a;
			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
			o.Emission = _RimColor.rgb * pow (rim, _Effect);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
