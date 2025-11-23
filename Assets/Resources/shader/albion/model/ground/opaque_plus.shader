Shader "Albion/Model/Ground/Opaque_plus" {
	Properties {
		_Color ("Vertex Color", Vector) = (1,1,1,1)
		_Tint ("Diffuse Tint ", Vector) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Height ("Height", Range(0.0001, 10)) = 1
		_Offset ("Offset", Float) = 0
		[Space(30)] [Toggle(_USE_VTX_ANIM)] _VertexAnimationToggle ("Use Vertex Animation", Float) = 0
		[Header((Freq)(Speed)(AmpH)(AmpV))] [Space(5)] _DispMain ("Main", Vector) = (1,1,1,1)
		_DispSecondary ("Secondary", Vector) = (1,1,1,1)
		_DispDetail ("Detail", Vector) = (1,1,1,1)
		[Header(Cap Detail Displacement)] [Space(5)] _CapDispDetail ("WindStrength to cap at", Range(0, 5)) = 1
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
	Fallback "Diffuse"
}