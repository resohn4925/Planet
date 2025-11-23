Shader "Albion/Landscape/Standard/TiledNoSSAO" {
	Properties {
		_MainTex ("Main (RGB)", 2D) = "white" {}
		_Color ("Color", Vector) = (1,1,1,1)
		_Scale ("Scale", Float) = 100
		[Space(30)] [Toggle(_USE_VTX_ANIM)] _VertexAnimationToggle ("Use Vertex Animation", Float) = 0
		[Header((Freq)(Speed)(AmpH)(AmpV))] [Space(5)] _DispMain ("Main", Vector) = (1,1,1,1)
		_DispSecondary ("Secondary", Vector) = (1,1,1,1)
		_DispDetail ("Detail", Vector) = (1,1,1,1)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Albion/Internal/VertexLit"
}