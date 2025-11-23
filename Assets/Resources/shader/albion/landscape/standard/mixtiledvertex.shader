Shader "Albion/Landscape/Standard/MixTiledVertex" {
	Properties {
		_WhiteTex ("White (RGB)", 2D) = "white" {}
		_BlackTex ("Black (RGB)", 2D) = "black" {}
		_WhiteColor ("White Color", Vector) = (1,1,1,1)
		_BlackColor ("Black Color", Vector) = (1,1,1,1)
		_WhiteScale ("White Scale", Float) = 100
		_BlackScale ("Black Scale", Float) = 100
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _WhiteTex;
		sampler2D _BlackTex;
		fixed4 _WhiteColor;
		fixed4 _BlackColor;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			// o.Albedo = tex2D(_WhiteTex, IN.uv_MainTex).rgb * _WhiteColor.rgb;
			o.Albedo = tex2D(_BlackTex, IN.uv_MainTex).rgb * _BlackColor.rgb;
			o.Alpha = 1;
		}
		ENDCG
	}
	Fallback "Albion/Internal/VertexLit"
}