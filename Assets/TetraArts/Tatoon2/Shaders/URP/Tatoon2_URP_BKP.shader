// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TetraArts/Tatoon2/URP/Tatoon2_URP"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		_MainTexture("MainTexture", 2D) = "white" {}
		_RimColor("Rim Color", Color) = (0,1,0.8758622,0)
		_DiffuseColor("Diffuse Color", Color) = (1,1,1,0)
		_RimBlend("Rim Blend", Range( 0 , 10)) = 0
		_ShadowSize("Shadow Size", Range( 0 , 1)) = 0.5
		[Toggle]_UseNormalMap("UseNormalMap", Float) = 0
		_ShadowBlend("ShadowBlend", Range( 0 , 1)) = 0.01
		_NormalStrength("Normal Strength", Float) = 1
		_RimSize("Rim Size", Range( 0 , 2)) = 0.1
		[Toggle]_Normalize("Normalize", Float) = 0
		_ShadowColor("Shadow Color", Color) = (0.5849056,0.5849056,0.5849056,1)
		[HideInInspector][Normal]_Texture0("Texture 0", 2D) = "bump" {}
		_Normal("Normal", 2D) = "bump" {}
		[NoScaleOffset]_ShadowTexture("Shadow Texture", 2D) = "white" {}
		_OutlineSize("OutlineSize", Float) = 0.3
		[Toggle]_UseRim("UseRim", Float) = 1
		[Toggle]_OutlineNoise("OutlineNoise", Float) = 0
		[Toggle]_OutlineNoise("OutlineNoise", Float) = 1
		[Toggle]_UseGradient("Use Gradient", Float) = 0
		[HDR]_OutlineColor("OutlineColor", Color) = (0,0,0,0)
		[Toggle]_UseOutline("UseOutline", Float) = 1
		[Toggle]_ShadowTextureViewProjection("Shadow Texture View Projection", Float) = 1
		_ColorB("Color B", Color) = (0,0.1264467,1,0)
		_ColorA("Color A", Color) = (1,0,0,0)
		_ShadowTextureTiling("Shadow Texture Tiling", Float) = 1
		_ShadowTextureRotation("Shadow Texture Rotation", Float) = 0
		_OutlineNoiseScale("OutlineNoiseScale", Float) = 0
		_GradientSize("Gradient Size", Range( 0 , 10)) = 0
		[Toggle]_UseShadow("UseShadow", Float) = 1
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_GradientPosition("Gradient Position", Float) = 0
		_GradientRotation("Gradient Rotation", Float) = 0
		_Metalic("Metalic", Range( 0 , 1)) = 0
		[NoScaleOffset]_RimTexture("Rim Texture", 2D) = "white" {}
		[Toggle]_UseSpecular("UseSpecular", Float) = 1
		[Toggle]_RimTextureViewProjection("Rim Texture View Projection", Float) = 0
		_RimTextureTiling("Rim Texture Tiling", Float) = 0
		[NoScaleOffset]_SpecularMap("Specular Map", 2D) = "gray" {}
		_RimTextureRotation("Rim Texture Rotation", Float) = 0
		[Toggle]_SpecularTextureViewProjection("Specular Texture View Projection", Float) = 0
		[Toggle]_RimLightColor("Rim Light Color", Float) = 1
		_SpecularTextureTiling("Specular Texture Tiling", Float) = 0
		_SpecularTextureRotation("Specular Texture Rotation", Float) = 0
		_RimLightIntensity("Rim Light Intensity", Range( 0 , 1)) = 0.2
		_SpecularColor("Specular Color", Color) = (0,1,0.8745098,0)
		[Toggle]_UseShadowTexture("UseShadowTexture", Float) = 0
		[Toggle]_SpecLightColor("Spec Light Color", Float) = 0
		_SpecularLightIntensity("Specular Light Intensity", Range( 0 , 10)) = 1
		_SpecularSize("Specular Size", Range( 0 , 0.01)) = 0.005
		_SpecularBlend("Specular Blend", Range( 0 , 1)) = 0
		[Toggle]_ChangeAxis("ChangeAxis", Float) = 1
		[Toggle]_Level2("Level2", Float) = 0
		[Toggle]_Level3("Level3", Float) = 0
		_XDirectionSpeed("XDirectionSpeed", Float) = 0
		_YDirectionSpeed("YDirectionSpeed", Float) = 0
		[Toggle]_Animate("Animate", Float) = 0
		_XSpeed("XSpeed", Float) = 0
		_YSpeed("YSpeed", Float) = 0
		_ShadowRamp("ShadowRamp", 2D) = "white" {}
		[Toggle]_UseRamp("UseRamp", Float) = 0
		_EmissiveMap("EmissiveMap", 2D) = "white" {}
		_Metal("Metal", 2D) = "white" {}
		[HDR]_EmissiveColor("EmissiveColor", Color) = (4,4,4,0)
		[Toggle]_UseEmissive("UseEmissive", Float) = 0
		[Toggle]_UseMetalSmooth("UseMetalSmooth", Float) = 0
		[ASEEnd][IntRange]_Zwrite("Zwrite", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		//_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		//_TransStrength( "Trans Strength", Range( 0, 50 ) ) = 1
		//_TransNormal( "Trans Normal Distortion", Range( 0, 1 ) ) = 0.5
		//_TransScattering( "Trans Scattering", Range( 1, 50 ) ) = 2
		//_TransDirect( "Trans Direct", Range( 0, 1 ) ) = 0.9
		//_TransAmbient( "Trans Ambient", Range( 0, 1 ) ) = 0.1
		//_TransShadow( "Trans Shadow", Range( 0, 1 ) ) = 0.5
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Overlay" }
		Cull Back
		AlphaToMask Off
		
		HLSLINCLUDE
		#pragma target 3.0

		#pragma prefer_hlslcc gles
		


		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS
		ENDHLSL

		
		Pass
		{
			Name "OutlinePass"
			
			
			Blend Off
			Cull Front
			ZWrite [_Zwrite]
			ZTest LEqual
			Offset 0,0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70301

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_EXTRA_PREPASS

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef ASE_FOG
				float fogFactor : TEXCOORD2;
				#endif
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _ColorA;
			float4 _Metal_ST;
			float4 _ColorB;
			float4 _EmissiveMap_ST;
			float4 _EmissiveColor;
			float4 _ShadowColor;
			float4 _DiffuseColor;
			float4 _MainTexture_ST;
			float4 _OutlineColor;
			float4 _Texture0_ST;
			float4 _RimColor;
			float4 _SpecularColor;
			float _SpecularLightIntensity;
			float _SpecularTextureRotation;
			float _SpecularSize;
			float _SpecularTextureTiling;
			float _SpecularTextureViewProjection;
			float _UseSpecular;
			float _SpecLightColor;
			float _SpecularBlend;
			float _Zwrite;
			float _RimSize;
			float _RimBlend;
			float _Level3;
			float _RimLightColor;
			float _RimLightIntensity;
			float _RimTextureViewProjection;
			float _RimTextureTiling;
			float _RimTextureRotation;
			float _UseEmissive;
			float _UseMetalSmooth;
			float _UseRim;
			float _Level2;
			float _Animate;
			float _YDirectionSpeed;
			float _UseOutline;
			float _OutlineNoise;
			float _OutlineSize;
			float _XSpeed;
			float _YSpeed;
			float _OutlineNoiseScale;
			float _UseNormalMap;
			float _NormalStrength;
			float _UseRamp;
			float _UseShadow;
			float _UseGradient;
			float _GradientPosition;
			float _GradientSize;
			float _ChangeAxis;
			float _GradientRotation;
			float _UseShadowTexture;
			float _ShadowSize;
			float _ShadowBlend;
			float _Normalize;
			float _ShadowTextureViewProjection;
			float _ShadowTextureTiling;
			float _Metalic;
			float _XDirectionSpeed;
			float _ShadowTextureRotation;
			float _Smoothness;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _MainTexture;


			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			

			VertexOutput VertexFunction ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float temp_output_201_0 = ( _OutlineSize / 100.0 );
				float2 appendResult411 = (float2(_XSpeed , _YSpeed));
				float2 texCoord211 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner410 = ( 1.0 * _Time.y * appendResult411 + texCoord211);
				float simplePerlin2D210 = snoise( panner410*(( _OutlineNoise )?( _OutlineNoiseScale ):( 0.0 )) );
				simplePerlin2D210 = simplePerlin2D210*0.5 + 0.5;
				float2 uv_MainTexture = v.ase_texcoord.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
				float4 tex2DNode138 = tex2Dlod( _MainTexture, float4( uv_MainTexture, 0, 0.0) );
				float Alpha209 = tex2DNode138.a;
				float3 unityObjectToViewPos208 = TransformWorldToView( TransformObjectToWorld( v.vertex.xyz) );
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = (( _UseOutline )?( ( v.ase_normal * (( _OutlineNoise )?( ( temp_output_201_0 * simplePerlin2D210 ) ):( (( _UseOutline )?( temp_output_201_0 ):( 0.0 )) )) * Alpha209 * max( unityObjectToViewPos208.z , 1.0 ) ) ):( float3( 0,0,0 ) ));
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				#ifdef ASE_FOG
				o.fogFactor = ComputeFogFactor( positionCS.z );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif
				
				float3 Color = _OutlineColor.rgb;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				return half4( Color, Alpha );
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend Off
			ZWrite On
			ZTest LEqual
			Offset 0,0
			ColorMask RGBA
			

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70301

			
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_FORWARD

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
			    #define ENABLE_TERRAIN_PERPIXEL_NORMAL
			#endif

			#define ASE_NEEDS_FRAG_POSITION
			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_FRAG_WORLD_BITANGENT
			#define ASE_NEEDS_FRAG_SHADOWCOORDS
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord : TEXCOORD0;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 lightmapUVOrVertexSH : TEXCOORD0;
				half4 fogFactorAndVertexLight : TEXCOORD1;
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord : TEXCOORD2;
				#endif
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 screenPos : TEXCOORD6;
				#endif
				float4 ase_texcoord7 : TEXCOORD7;
				float4 ase_texcoord8 : TEXCOORD8;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _ColorA;
			float4 _Metal_ST;
			float4 _ColorB;
			float4 _EmissiveMap_ST;
			float4 _EmissiveColor;
			float4 _ShadowColor;
			float4 _DiffuseColor;
			float4 _MainTexture_ST;
			float4 _OutlineColor;
			float4 _Texture0_ST;
			float4 _RimColor;
			float4 _SpecularColor;
			float _SpecularLightIntensity;
			float _SpecularTextureRotation;
			float _SpecularSize;
			float _SpecularTextureTiling;
			float _SpecularTextureViewProjection;
			float _UseSpecular;
			float _SpecLightColor;
			float _SpecularBlend;
			float _Zwrite;
			float _RimSize;
			float _RimBlend;
			float _Level3;
			float _RimLightColor;
			float _RimLightIntensity;
			float _RimTextureViewProjection;
			float _RimTextureTiling;
			float _RimTextureRotation;
			float _UseEmissive;
			float _UseMetalSmooth;
			float _UseRim;
			float _Level2;
			float _Animate;
			float _YDirectionSpeed;
			float _UseOutline;
			float _OutlineNoise;
			float _OutlineSize;
			float _XSpeed;
			float _YSpeed;
			float _OutlineNoiseScale;
			float _UseNormalMap;
			float _NormalStrength;
			float _UseRamp;
			float _UseShadow;
			float _UseGradient;
			float _GradientPosition;
			float _GradientSize;
			float _ChangeAxis;
			float _GradientRotation;
			float _UseShadowTexture;
			float _ShadowSize;
			float _ShadowBlend;
			float _Normalize;
			float _ShadowTextureViewProjection;
			float _ShadowTextureTiling;
			float _Metalic;
			float _XDirectionSpeed;
			float _ShadowTextureRotation;
			float _Smoothness;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Texture0;
			sampler2D _Normal;
			sampler2D _MainTexture;
			sampler2D _ShadowTexture;
			sampler2D _ShadowRamp;
			sampler2D _SpecularMap;
			sampler2D _RimTexture;
			sampler2D _EmissiveMap;
			sampler2D _Metal;


			float3 AdditionalLightsHalfLambert( float3 WorldPosition, float3 WorldNormal )
			{
				float3 Color = 0;
				#ifdef _ADDITIONAL_LIGHTS
				int numLights = GetAdditionalLightsCount();
				for(int i = 0; i<numLights;i++)
				{
					Light light = GetAdditionalLight(i, WorldPosition);
					half3 AttLightColor = light.color *(light.distanceAttenuation * light.shadowAttenuation);
					Color +=(dot(light.direction, WorldNormal)*0.5+0.5 )* AttLightColor;
					
				}
				#endif
				return Color;
			}
			
			real ASESafeNormalize(float inVec)
			{
				real dp3 = max(FLT_MIN, dot(inVec, inVec));
				return inVec* rsqrt( dp3);
			}
			
			real3 ASESafeNormalize(float3 inVec)
			{
				real dp3 = max(FLT_MIN, dot(inVec, inVec));
				return inVec* rsqrt( dp3);
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord7.xy = v.texcoord.xy;
				o.ase_texcoord8 = v.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord7.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 positionVS = TransformWorldToView( positionWS );
				float4 positionCS = TransformWorldToHClip( positionWS );

				VertexNormalInputs normalInput = GetVertexNormalInputs( v.ase_normal, v.ase_tangent );

				o.tSpace0 = float4( normalInput.normalWS, positionWS.x);
				o.tSpace1 = float4( normalInput.tangentWS, positionWS.y);
				o.tSpace2 = float4( normalInput.bitangentWS, positionWS.z);

				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				OUTPUT_SH( normalInput.normalWS.xyz, o.lightmapUVOrVertexSH.xyz );

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					o.lightmapUVOrVertexSH.zw = v.texcoord;
					o.lightmapUVOrVertexSH.xy = v.texcoord * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				half3 vertexLight = VertexLighting( positionWS, normalInput.normalWS );
				#ifdef ASE_FOG
					half fogFactor = ComputeFogFactor( positionCS.z );
				#else
					half fogFactor = 0;
				#endif
				o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
				
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				
				o.clipPos = positionCS;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				o.screenPos = ComputeScreenPos(positionCS);
				#endif
				return o;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.texcoord = v.texcoord;
				o.texcoord1 = v.texcoord1;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.texcoord = patch[0].texcoord * bary.x + patch[1].texcoord * bary.y + patch[2].texcoord * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif

			half4 frag ( VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float2 sampleCoords = (IN.lightmapUVOrVertexSH.zw / _TerrainHeightmapRecipSize.zw + 0.5f) * _TerrainHeightmapRecipSize.xy;
					float3 WorldNormal = TransformObjectToWorldNormal(normalize(SAMPLE_TEXTURE2D(_TerrainNormalmapTexture, sampler_TerrainNormalmapTexture, sampleCoords).rgb * 2 - 1));
					float3 WorldTangent = -cross(GetObjectToWorldMatrix()._13_23_33, WorldNormal);
					float3 WorldBiTangent = cross(WorldNormal, -WorldTangent);
				#else
					float3 WorldNormal = normalize( IN.tSpace0.xyz );
					float3 WorldTangent = IN.tSpace1.xyz;
					float3 WorldBiTangent = IN.tSpace2.xyz;
				#endif
				float3 WorldPosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldViewDirection = _WorldSpaceCameraPos.xyz  - WorldPosition;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 ScreenPos = IN.screenPos;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					ShadowCoords = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
				#endif
	
				WorldViewDirection = SafeNormalize( WorldViewDirection );

				float4 color440 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
				
				float2 uv_Texture0 = IN.ase_texcoord7.xy * _Texture0_ST.xy + _Texture0_ST.zw;
				float2 texCoord192 = IN.ase_texcoord7.xy * float2( 1,1 ) + float2( 0,0 );
				float3 unpack188 = UnpackNormalScale( tex2D( _Normal, texCoord192 ), _NormalStrength );
				unpack188.z = lerp( 1, unpack188.z, saturate(_NormalStrength) );
				float3 NormalMap182 = (( _UseNormalMap )?( unpack188 ):( UnpackNormalScale( tex2D( _Texture0, uv_Texture0 ), 1.0f ) ));
				
				float2 appendResult363 = (float2(IN.ase_texcoord8.xyz.x , IN.ase_texcoord8.xyz.y));
				float2 appendResult365 = (float2(IN.ase_texcoord8.xyz.y , IN.ase_texcoord8.xyz.z));
				float cos369 = cos( radians( _GradientRotation ) );
				float sin369 = sin( radians( _GradientRotation ) );
				float2 rotator369 = mul( (( _ChangeAxis )?( appendResult365 ):( appendResult363 )) - float2( 0,0 ) , float2x2( cos369 , -sin369 , sin369 , cos369 )) + float2( 0,0 );
				float smoothstepResult375 = smoothstep( _GradientPosition , ( _GradientPosition + _GradientSize ) , rotator369.x);
				float4 lerpResult378 = lerp( _ColorA , _ColorB , smoothstepResult375);
				float2 uv_MainTexture = IN.ase_texcoord7.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
				float4 tex2DNode138 = tex2D( _MainTexture, uv_MainTexture );
				float4 Color156 = ( _MainLightColor * ( (( _UseGradient )?( lerpResult378 ):( _DiffuseColor )) * tex2DNode138 ) );
				float3 normalizedWorldNormal = normalize( WorldNormal );
				float dotResult22 = dot( normalizedWorldNormal , SafeNormalize(_MainLightPosition.xyz) );
				float3 worldPosValue44_g1 = WorldPosition;
				float3 WorldPosition22_g1 = worldPosValue44_g1;
				float3 tanToWorld0 = float3( WorldTangent.x, WorldBiTangent.x, WorldNormal.x );
				float3 tanToWorld1 = float3( WorldTangent.y, WorldBiTangent.y, WorldNormal.y );
				float3 tanToWorld2 = float3( WorldTangent.z, WorldBiTangent.z, WorldNormal.z );
				float3 tanNormal12_g1 = NormalMap182;
				float3 worldNormal12_g1 = float3(dot(tanToWorld0,tanNormal12_g1), dot(tanToWorld1,tanNormal12_g1), dot(tanToWorld2,tanNormal12_g1));
				float3 worldNormalValue50_g1 = worldNormal12_g1;
				float3 WorldNormal22_g1 = worldNormalValue50_g1;
				float3 localAdditionalLightsHalfLambert22_g1 = AdditionalLightsHalfLambert( WorldPosition22_g1 , WorldNormal22_g1 );
				float3 halfLambertResult58_g1 = localAdditionalLightsHalfLambert22_g1;
				float3 temp_output_80_0 = halfLambertResult58_g1;
				float3 break98 = temp_output_80_0;
				float temp_output_99_0 = max( max( break98.x , break98.y ) , break98.z );
				float normalizeResult174 = ASESafeNormalize( temp_output_99_0 );
				float smoothstepResult28 = smoothstep( _ShadowSize , ( _ShadowSize + _ShadowBlend ) , ( dotResult22 + (( _Normalize )?( normalizeResult174 ):( temp_output_99_0 )) ));
				float ase_lightAtten = 0;
				Light ase_mainLight = GetMainLight( ShadowCoords );
				ase_lightAtten = ase_mainLight.distanceAttenuation * ase_mainLight.shadowAttenuation;
				float3 break149 = ( _MainLightColor.rgb * ase_lightAtten );
				float temp_output_124_0 = ( smoothstepResult28 * ( max( max( break149.x , break149.y ) , break149.z ) + (( _Normalize )?( normalizeResult174 ):( temp_output_99_0 )) ) );
				float Lighting276 = temp_output_124_0;
				float clampResult505 = clamp( Lighting276 , 0.0 , 1.0 );
				float temp_output_479_0 = ( 1.0 - clampResult505 );
				float4 temp_output_494_0 = ( clampResult505 * Color156 );
				float2 temp_cast_1 = (_ShadowTextureTiling).xx;
				float2 texCoord231 = IN.ase_texcoord7.xy * temp_cast_1 + float2( 0,0 );
				float3 temp_output_228_0 = ( ( _ShadowTextureTiling * 1 ) * mul( UNITY_MATRIX_VP, float4( WorldViewDirection , 0.0 ) ).xyz );
				float2 appendResult394 = (float2(_XDirectionSpeed , _YDirectionSpeed));
				float2 panner391 = ( 1.0 * _Time.y * appendResult394 + temp_output_228_0.xy);
				float cos250 = cos( radians( _ShadowTextureRotation ) );
				float sin250 = sin( radians( _ShadowTextureRotation ) );
				float2 rotator250 = mul( (( _ShadowTextureViewProjection )?( (( _Animate )?( float3( panner391 ,  0.0 ) ):( temp_output_228_0 )) ):( float3( texCoord231 ,  0.0 ) )).xy - float2( 0.5,0.5 ) , float2x2( cos250 , -sin250 , sin250 , cos250 )) + float2( 0.5,0.5 );
				float ShadowSize277 = _ShadowSize;
				float temp_output_239_0 = ( ShadowSize277 - -1.0 );
				float NdotL390 = dotResult22;
				float smoothstepResult252 = smoothstep( temp_output_239_0 , ( temp_output_239_0 + 0.2 ) , NdotL390);
				float cos242 = cos( radians( ( _ShadowTextureRotation + 90.0 ) ) );
				float sin242 = sin( radians( ( _ShadowTextureRotation + 90.0 ) ) );
				float2 rotator242 = mul( (( _ShadowTextureViewProjection )?( (( _Animate )?( float3( panner391 ,  0.0 ) ):( temp_output_228_0 )) ):( float3( texCoord231 ,  0.0 ) )).xy - float2( 0.5,0.5 ) , float2x2( cos242 , -sin242 , sin242 , cos242 )) + float2( 0.5,0.5 );
				float temp_output_241_0 = ( ShadowSize277 - 0.25 );
				float smoothstepResult254 = smoothstep( temp_output_241_0 , ( temp_output_241_0 + 0.2 ) , NdotL390);
				float cos247 = cos( radians( ( _ShadowTextureRotation + 45.0 ) ) );
				float sin247 = sin( radians( ( _ShadowTextureRotation + 45.0 ) ) );
				float2 rotator247 = mul( (( _ShadowTextureViewProjection )?( (( _Animate )?( float3( panner391 ,  0.0 ) ):( temp_output_228_0 )) ):( float3( texCoord231 ,  0.0 ) )).xy - float2( 0.5,0.5 ) , float2x2( cos247 , -sin247 , sin247 , cos247 )) + float2( 0.5,0.5 );
				float temp_output_236_0 = ( ShadowSize277 - 0.6 );
				float smoothstepResult257 = smoothstep( temp_output_236_0 , ( temp_output_236_0 + 0.2 ) , NdotL390);
				float4 temp_cast_20 = (smoothstepResult252).xxxx;
				float4 ShadowColor157 = (( _UseShadow )?( (( _UseShadowTexture )?( ( temp_output_494_0 + ( Color156 * ( _ShadowColor * ( temp_output_479_0 * ( ( 1.0 - ( ( ( 1.0 - tex2D( _ShadowTexture, rotator250 ) ) * ( 1.0 - smoothstepResult252 ) ) + (( _Level2 )?( ( ( 1.0 - tex2D( _ShadowTexture, rotator242 ) ) * ( 1.0 - smoothstepResult254 ) ) ):( float4( 0,0,0,0 ) )) + (( _Level3 )?( ( ( 1.0 - tex2D( _ShadowTexture, rotator247 ) ) * ( 1.0 - smoothstepResult257 ) ) ):( float4( 0,0,0,0 ) )) ) ) - temp_cast_20 ) ) ) ) ) ):( ( ( ( _ShadowColor * temp_output_479_0 ) * Color156 ) + temp_output_494_0 ) )) ):( Color156 ));
				float2 appendResult416 = (float2(NdotL390 , 0.0));
				float2 N2417 = appendResult416;
				float4 ShadowRamp424 = ( ( tex2D( _ShadowRamp, (N2417*0.5 + 0.5) ) * Color156 ) * ShadowColor157 );
				float2 temp_cast_21 = (_SpecularTextureTiling).xx;
				float2 texCoord330 = IN.ase_texcoord7.xy * temp_cast_21 + float2( 0,0 );
				float cos342 = cos( radians( _SpecularTextureRotation ) );
				float sin342 = sin( radians( _SpecularTextureRotation ) );
				float2 rotator342 = mul( (( _SpecularTextureViewProjection )?( ( ( _SpecularTextureTiling * 1 ) * mul( float4( WorldViewDirection , 0.0 ), UNITY_MATRIX_VP ).xyz ) ):( float3( texCoord330 ,  0.0 ) )).xy - float2( 0,0 ) , float2x2( cos342 , -sin342 , sin342 , cos342 )) + float2( 0,0 );
				float AddLight320 = (( _Normalize )?( normalizeResult174 ):( temp_output_99_0 ));
				float temp_output_344_0 = ( 1.0 - _SpecularSize );
				float3 normalizeResult332 = normalize( SafeNormalize(_MainLightPosition.xyz) );
				float3 normalizeResult336 = normalize( WorldViewDirection );
				float3 normalizeResult350 = normalize( ( normalizeResult332 + normalizeResult336 ) );
				float3 normalizeResult346 = normalize( WorldNormal );
				float dotResult353 = dot( normalizeResult350 , normalizeResult346 );
				float smoothstepResult355 = smoothstep( temp_output_344_0 , ( temp_output_344_0 + _SpecularBlend ) , dotResult353);
				float4 Specular359 = (( _UseSpecular )?( ( ( ( 1.0 - tex2D( _SpecularMap, rotator342 ) ) * (( _SpecLightColor )?( ( ( _MainLightColor + AddLight320 ) * _SpecularLightIntensity ) ):( _SpecularColor )) ) * smoothstepResult355 ) ):( float4( 0,0,0,0 ) ));
				float temp_output_296_0 = ( 1.0 - _RimSize );
				float dotResult292 = dot( WorldNormal , WorldViewDirection );
				float smoothstepResult312 = smoothstep( temp_output_296_0 , ( temp_output_296_0 + _RimBlend ) , ( ( 1.0 - dotResult292 ) * pow( abs( Lighting276 ) , 0.1 ) ));
				float2 temp_cast_26 = (_RimTextureTiling).xx;
				float2 texCoord291 = IN.ase_texcoord7.xy * temp_cast_26 + float2( 0,0 );
				float cos308 = cos( radians( _RimTextureRotation ) );
				float sin308 = sin( radians( _RimTextureRotation ) );
				float2 rotator308 = mul( (( _RimTextureViewProjection )?( ( ( _RimTextureTiling * 1 ) * mul( float4( WorldViewDirection , 0.0 ), UNITY_MATRIX_VP ).xyz ) ):( float3( texCoord291 ,  0.0 ) )).xy - float2( 0,0 ) , float2x2( cos308 , -sin308 , sin308 , cos308 )) + float2( 0,0 );
				float4 Rim317 = (( _UseRim )?( ( saturate( smoothstepResult312 ) * ( (( _RimLightColor )?( ( _RimLightIntensity * ( _MainLightColor + AddLight320 ) ) ):( _RimColor )) * tex2D( _RimTexture, rotator308 ) ) ) ):( float4( 0,0,0,0 ) ));
				float2 uv_EmissiveMap = IN.ase_texcoord7.xy * _EmissiveMap_ST.xy + _EmissiveMap_ST.zw;
				float4 Emissiv453 = (( _UseEmissive )?( ( _EmissiveColor * tex2D( _EmissiveMap, uv_EmissiveMap ) ) ):( float4( 0,0,0,0 ) ));
				float3 normalizeResult176 = ASESafeNormalize( temp_output_80_0 );
				float4 Result195 = ( (( _UseRamp )?( ShadowRamp424 ):( ( ( ShadowColor157 * ( 1.0 - temp_output_124_0 ) ) + ( temp_output_124_0 * Color156 ) ) )) + Specular359 + Rim317 + Emissiv453 + float4( (( _Normalize )?( normalizeResult176 ):( temp_output_80_0 )) , 0.0 ) );
				
				float2 uv_Metal = IN.ase_texcoord7.xy * _Metal_ST.xy + _Metal_ST.zw;
				float4 tex2DNode456 = tex2D( _Metal, uv_Metal );
				float MetalicVar466 = (( _UseMetalSmooth )?( ( tex2DNode456.a * ( 1.0 - _Metalic ) ) ):( 1.0 ));
				
				float SmoothnessVar467 = (( _UseMetalSmooth )?( ( tex2DNode456.a * _Smoothness ) ):( 0.0 ));
				
				float3 Albedo = color440.rgb;
				float3 Normal = NormalMap182;
				float3 Emission = Result195.rgb;
				float3 Specular = 0.5;
				float Metallic = MetalicVar466;
				float Smoothness = SmoothnessVar467;
				float Occlusion = 0.0;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = 0;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				InputData inputData;
				inputData.positionWS = WorldPosition;
				inputData.viewDirectionWS = WorldViewDirection;
				inputData.shadowCoord = ShadowCoords;

				#ifdef _NORMALMAP
					#if _NORMAL_DROPOFF_TS
					inputData.normalWS = TransformTangentToWorld(Normal, half3x3( WorldTangent, WorldBiTangent, WorldNormal ));
					#elif _NORMAL_DROPOFF_OS
					inputData.normalWS = TransformObjectToWorldNormal(Normal);
					#elif _NORMAL_DROPOFF_WS
					inputData.normalWS = Normal;
					#endif
					inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
				#else
					inputData.normalWS = WorldNormal;
				#endif

				#ifdef ASE_FOG
					inputData.fogCoord = IN.fogFactorAndVertexLight.x;
				#endif

				inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float3 SH = SampleSH(inputData.normalWS.xyz);
				#else
					float3 SH = IN.lightmapUVOrVertexSH.xyz;
				#endif

				inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, SH, inputData.normalWS );
				#ifdef _ASE_BAKEDGI
					inputData.bakedGI = BakedGI;
				#endif
				half4 color = UniversalFragmentPBR(
					inputData, 
					Albedo, 
					Metallic, 
					Specular, 
					Smoothness, 
					Occlusion, 
					Emission, 
					Alpha);

				#ifdef _TRANSMISSION_ASE
				{
					float shadow = _TransmissionShadow;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );
					half3 mainTransmission = max(0 , -dot(inputData.normalWS, mainLight.direction)) * mainAtten * Transmission;
					color.rgb += Albedo * mainTransmission;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 transmission = max(0 , -dot(inputData.normalWS, light.direction)) * atten * Transmission;
							color.rgb += Albedo * transmission;
						}
					#endif
				}
				#endif

				#ifdef _TRANSLUCENCY_ASE
				{
					float shadow = _TransShadow;
					float normal = _TransNormal;
					float scattering = _TransScattering;
					float direct = _TransDirect;
					float ambient = _TransAmbient;
					float strength = _TransStrength;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );

					half3 mainLightDir = mainLight.direction + inputData.normalWS * normal;
					half mainVdotL = pow( saturate( dot( inputData.viewDirectionWS, -mainLightDir ) ), scattering );
					half3 mainTranslucency = mainAtten * ( mainVdotL * direct + inputData.bakedGI * ambient ) * Translucency;
					color.rgb += Albedo * mainTranslucency * strength;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 lightDir = light.direction + inputData.normalWS * normal;
							half VdotL = pow( saturate( dot( inputData.viewDirectionWS, -lightDir ) ), scattering );
							half3 translucency = atten * ( VdotL * direct + inputData.bakedGI * ambient ) * Translucency;
							color.rgb += Albedo * translucency * strength;
						}
					#endif
				}
				#endif

				#ifdef _REFRACTION_ASE
					float4 projScreenPos = ScreenPos / ScreenPos.w;
					float3 refractionOffset = ( RefractionIndex - 1.0 ) * mul( UNITY_MATRIX_V, float4( WorldNormal, 0 ) ).xyz * ( 1.0 - dot( WorldNormal, WorldViewDirection ) );
					projScreenPos.xy += refractionOffset.xy;
					float3 refraction = SHADERGRAPH_SAMPLE_SCENE_COLOR( projScreenPos.xy ) * RefractionColor;
					color.rgb = lerp( refraction, color.rgb, color.a );
					color.a = 1;
				#endif

				#ifdef ASE_FINAL_COLOR_ALPHA_MULTIPLY
					color.rgb *= color.a;
				#endif

				#ifdef ASE_FOG
					#ifdef TERRAIN_SPLAT_ADDPASS
						color.rgb = MixFogColor(color.rgb, half3( 0, 0, 0 ), IN.fogFactorAndVertexLight.x );
					#else
						color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
					#endif
				#endif
				
				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				return color;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual
			AlphaToMask Off

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70301

			
			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_SHADOWCASTER

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _ColorA;
			float4 _Metal_ST;
			float4 _ColorB;
			float4 _EmissiveMap_ST;
			float4 _EmissiveColor;
			float4 _ShadowColor;
			float4 _DiffuseColor;
			float4 _MainTexture_ST;
			float4 _OutlineColor;
			float4 _Texture0_ST;
			float4 _RimColor;
			float4 _SpecularColor;
			float _SpecularLightIntensity;
			float _SpecularTextureRotation;
			float _SpecularSize;
			float _SpecularTextureTiling;
			float _SpecularTextureViewProjection;
			float _UseSpecular;
			float _SpecLightColor;
			float _SpecularBlend;
			float _Zwrite;
			float _RimSize;
			float _RimBlend;
			float _Level3;
			float _RimLightColor;
			float _RimLightIntensity;
			float _RimTextureViewProjection;
			float _RimTextureTiling;
			float _RimTextureRotation;
			float _UseEmissive;
			float _UseMetalSmooth;
			float _UseRim;
			float _Level2;
			float _Animate;
			float _YDirectionSpeed;
			float _UseOutline;
			float _OutlineNoise;
			float _OutlineSize;
			float _XSpeed;
			float _YSpeed;
			float _OutlineNoiseScale;
			float _UseNormalMap;
			float _NormalStrength;
			float _UseRamp;
			float _UseShadow;
			float _UseGradient;
			float _GradientPosition;
			float _GradientSize;
			float _ChangeAxis;
			float _GradientRotation;
			float _UseShadowTexture;
			float _ShadowSize;
			float _ShadowBlend;
			float _Normalize;
			float _ShadowTextureViewProjection;
			float _ShadowTextureTiling;
			float _Metalic;
			float _XDirectionSpeed;
			float _ShadowTextureRotation;
			float _Smoothness;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			

			
			float3 _LightDirection;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				float3 normalWS = TransformObjectToWorldDir(v.ase_normal);

				float4 clipPos = TransformWorldToHClip( ApplyShadowBias( positionWS, normalWS, _LightDirection ) );

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = clipPos;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif

			half4 frag(	VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );
				
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					#ifdef _ALPHATEST_SHADOW_ON
						clip(Alpha - AlphaClipThresholdShadow);
					#else
						clip(Alpha - AlphaClipThreshold);
					#endif
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif
				return 0;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70301

			
			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _ColorA;
			float4 _Metal_ST;
			float4 _ColorB;
			float4 _EmissiveMap_ST;
			float4 _EmissiveColor;
			float4 _ShadowColor;
			float4 _DiffuseColor;
			float4 _MainTexture_ST;
			float4 _OutlineColor;
			float4 _Texture0_ST;
			float4 _RimColor;
			float4 _SpecularColor;
			float _SpecularLightIntensity;
			float _SpecularTextureRotation;
			float _SpecularSize;
			float _SpecularTextureTiling;
			float _SpecularTextureViewProjection;
			float _UseSpecular;
			float _SpecLightColor;
			float _SpecularBlend;
			float _Zwrite;
			float _RimSize;
			float _RimBlend;
			float _Level3;
			float _RimLightColor;
			float _RimLightIntensity;
			float _RimTextureViewProjection;
			float _RimTextureTiling;
			float _RimTextureRotation;
			float _UseEmissive;
			float _UseMetalSmooth;
			float _UseRim;
			float _Level2;
			float _Animate;
			float _YDirectionSpeed;
			float _UseOutline;
			float _OutlineNoise;
			float _OutlineSize;
			float _XSpeed;
			float _YSpeed;
			float _OutlineNoiseScale;
			float _UseNormalMap;
			float _NormalStrength;
			float _UseRamp;
			float _UseShadow;
			float _UseGradient;
			float _GradientPosition;
			float _GradientSize;
			float _ChangeAxis;
			float _GradientRotation;
			float _UseShadowTexture;
			float _ShadowSize;
			float _ShadowBlend;
			float _Normalize;
			float _ShadowTextureViewProjection;
			float _ShadowTextureTiling;
			float _Metalic;
			float _XDirectionSpeed;
			float _ShadowTextureRotation;
			float _Smoothness;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			

			
			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;
				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif
			half4 frag(	VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				#ifdef ASE_DEPTH_WRITE_ON
				outputDepth = DepthValue;
				#endif
				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Meta"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _EMISSION
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70301

			
			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_META

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_FRAG_POSITION
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_SHADOWCOORDS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float4 ase_texcoord6 : TEXCOORD6;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _ColorA;
			float4 _Metal_ST;
			float4 _ColorB;
			float4 _EmissiveMap_ST;
			float4 _EmissiveColor;
			float4 _ShadowColor;
			float4 _DiffuseColor;
			float4 _MainTexture_ST;
			float4 _OutlineColor;
			float4 _Texture0_ST;
			float4 _RimColor;
			float4 _SpecularColor;
			float _SpecularLightIntensity;
			float _SpecularTextureRotation;
			float _SpecularSize;
			float _SpecularTextureTiling;
			float _SpecularTextureViewProjection;
			float _UseSpecular;
			float _SpecLightColor;
			float _SpecularBlend;
			float _Zwrite;
			float _RimSize;
			float _RimBlend;
			float _Level3;
			float _RimLightColor;
			float _RimLightIntensity;
			float _RimTextureViewProjection;
			float _RimTextureTiling;
			float _RimTextureRotation;
			float _UseEmissive;
			float _UseMetalSmooth;
			float _UseRim;
			float _Level2;
			float _Animate;
			float _YDirectionSpeed;
			float _UseOutline;
			float _OutlineNoise;
			float _OutlineSize;
			float _XSpeed;
			float _YSpeed;
			float _OutlineNoiseScale;
			float _UseNormalMap;
			float _NormalStrength;
			float _UseRamp;
			float _UseShadow;
			float _UseGradient;
			float _GradientPosition;
			float _GradientSize;
			float _ChangeAxis;
			float _GradientRotation;
			float _UseShadowTexture;
			float _ShadowSize;
			float _ShadowBlend;
			float _Normalize;
			float _ShadowTextureViewProjection;
			float _ShadowTextureTiling;
			float _Metalic;
			float _XDirectionSpeed;
			float _ShadowTextureRotation;
			float _Smoothness;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _MainTexture;
			sampler2D _Texture0;
			sampler2D _Normal;
			sampler2D _ShadowTexture;
			sampler2D _ShadowRamp;
			sampler2D _SpecularMap;
			sampler2D _RimTexture;
			sampler2D _EmissiveMap;


			float3 AdditionalLightsHalfLambert( float3 WorldPosition, float3 WorldNormal )
			{
				float3 Color = 0;
				#ifdef _ADDITIONAL_LIGHTS
				int numLights = GetAdditionalLightsCount();
				for(int i = 0; i<numLights;i++)
				{
					Light light = GetAdditionalLight(i, WorldPosition);
					half3 AttLightColor = light.color *(light.distanceAttenuation * light.shadowAttenuation);
					Color +=(dot(light.direction, WorldNormal)*0.5+0.5 )* AttLightColor;
					
				}
				#endif
				return Color;
			}
			
			real ASESafeNormalize(float inVec)
			{
				real dp3 = max(FLT_MIN, dot(inVec, inVec));
				return inVec* rsqrt( dp3);
			}
			
			real3 ASESafeNormalize(float3 inVec)
			{
				real dp3 = max(FLT_MIN, dot(inVec, inVec));
				return inVec* rsqrt( dp3);
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord4.xyz = ase_worldNormal;
				float3 ase_worldTangent = TransformObjectToWorldDir(v.ase_tangent.xyz);
				o.ase_texcoord5.xyz = ase_worldTangent;
				float ase_vertexTangentSign = v.ase_tangent.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord6.xyz = ase_worldBitangent;
				
				o.ase_texcoord2 = v.vertex;
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;
				o.ase_texcoord4.w = 0;
				o.ase_texcoord5.w = 0;
				o.ase_texcoord6.w = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.clipPos = MetaVertexPosition( v.vertex, v.texcoord1.xy, v.texcoord1.xy, unity_LightmapST, unity_DynamicLightmapST );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				o.ase_texcoord = v.ase_texcoord;
				o.ase_tangent = v.ase_tangent;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float4 color440 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
				
				float2 appendResult363 = (float2(IN.ase_texcoord2.xyz.x , IN.ase_texcoord2.xyz.y));
				float2 appendResult365 = (float2(IN.ase_texcoord2.xyz.y , IN.ase_texcoord2.xyz.z));
				float cos369 = cos( radians( _GradientRotation ) );
				float sin369 = sin( radians( _GradientRotation ) );
				float2 rotator369 = mul( (( _ChangeAxis )?( appendResult365 ):( appendResult363 )) - float2( 0,0 ) , float2x2( cos369 , -sin369 , sin369 , cos369 )) + float2( 0,0 );
				float smoothstepResult375 = smoothstep( _GradientPosition , ( _GradientPosition + _GradientSize ) , rotator369.x);
				float4 lerpResult378 = lerp( _ColorA , _ColorB , smoothstepResult375);
				float2 uv_MainTexture = IN.ase_texcoord3.xy * _MainTexture_ST.xy + _MainTexture_ST.zw;
				float4 tex2DNode138 = tex2D( _MainTexture, uv_MainTexture );
				float4 Color156 = ( _MainLightColor * ( (( _UseGradient )?( lerpResult378 ):( _DiffuseColor )) * tex2DNode138 ) );
				float3 ase_worldNormal = IN.ase_texcoord4.xyz;
				float3 normalizedWorldNormal = normalize( ase_worldNormal );
				float dotResult22 = dot( normalizedWorldNormal , SafeNormalize(_MainLightPosition.xyz) );
				float3 worldPosValue44_g1 = WorldPosition;
				float3 WorldPosition22_g1 = worldPosValue44_g1;
				float2 uv_Texture0 = IN.ase_texcoord3.xy * _Texture0_ST.xy + _Texture0_ST.zw;
				float2 texCoord192 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float3 unpack188 = UnpackNormalScale( tex2D( _Normal, texCoord192 ), _NormalStrength );
				unpack188.z = lerp( 1, unpack188.z, saturate(_NormalStrength) );
				float3 NormalMap182 = (( _UseNormalMap )?( unpack188 ):( UnpackNormalScale( tex2D( _Texture0, uv_Texture0 ), 1.0f ) ));
				float3 ase_worldTangent = IN.ase_texcoord5.xyz;
				float3 ase_worldBitangent = IN.ase_texcoord6.xyz;
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 tanNormal12_g1 = NormalMap182;
				float3 worldNormal12_g1 = float3(dot(tanToWorld0,tanNormal12_g1), dot(tanToWorld1,tanNormal12_g1), dot(tanToWorld2,tanNormal12_g1));
				float3 worldNormalValue50_g1 = worldNormal12_g1;
				float3 WorldNormal22_g1 = worldNormalValue50_g1;
				float3 localAdditionalLightsHalfLambert22_g1 = AdditionalLightsHalfLambert( WorldPosition22_g1 , WorldNormal22_g1 );
				float3 halfLambertResult58_g1 = localAdditionalLightsHalfLambert22_g1;
				float3 temp_output_80_0 = halfLambertResult58_g1;
				float3 break98 = temp_output_80_0;
				float temp_output_99_0 = max( max( break98.x , break98.y ) , break98.z );
				float normalizeResult174 = ASESafeNormalize( temp_output_99_0 );
				float smoothstepResult28 = smoothstep( _ShadowSize , ( _ShadowSize + _ShadowBlend ) , ( dotResult22 + (( _Normalize )?( normalizeResult174 ):( temp_output_99_0 )) ));
				float ase_lightAtten = 0;
				Light ase_mainLight = GetMainLight( ShadowCoords );
				ase_lightAtten = ase_mainLight.distanceAttenuation * ase_mainLight.shadowAttenuation;
				float3 break149 = ( _MainLightColor.rgb * ase_lightAtten );
				float temp_output_124_0 = ( smoothstepResult28 * ( max( max( break149.x , break149.y ) , break149.z ) + (( _Normalize )?( normalizeResult174 ):( temp_output_99_0 )) ) );
				float Lighting276 = temp_output_124_0;
				float clampResult505 = clamp( Lighting276 , 0.0 , 1.0 );
				float temp_output_479_0 = ( 1.0 - clampResult505 );
				float4 temp_output_494_0 = ( clampResult505 * Color156 );
				float2 temp_cast_1 = (_ShadowTextureTiling).xx;
				float2 texCoord231 = IN.ase_texcoord3.xy * temp_cast_1 + float2( 0,0 );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 temp_output_228_0 = ( ( _ShadowTextureTiling * 1 ) * mul( UNITY_MATRIX_VP, float4( ase_worldViewDir , 0.0 ) ).xyz );
				float2 appendResult394 = (float2(_XDirectionSpeed , _YDirectionSpeed));
				float2 panner391 = ( 1.0 * _Time.y * appendResult394 + temp_output_228_0.xy);
				float cos250 = cos( radians( _ShadowTextureRotation ) );
				float sin250 = sin( radians( _ShadowTextureRotation ) );
				float2 rotator250 = mul( (( _ShadowTextureViewProjection )?( (( _Animate )?( float3( panner391 ,  0.0 ) ):( temp_output_228_0 )) ):( float3( texCoord231 ,  0.0 ) )).xy - float2( 0.5,0.5 ) , float2x2( cos250 , -sin250 , sin250 , cos250 )) + float2( 0.5,0.5 );
				float ShadowSize277 = _ShadowSize;
				float temp_output_239_0 = ( ShadowSize277 - -1.0 );
				float NdotL390 = dotResult22;
				float smoothstepResult252 = smoothstep( temp_output_239_0 , ( temp_output_239_0 + 0.2 ) , NdotL390);
				float cos242 = cos( radians( ( _ShadowTextureRotation + 90.0 ) ) );
				float sin242 = sin( radians( ( _ShadowTextureRotation + 90.0 ) ) );
				float2 rotator242 = mul( (( _ShadowTextureViewProjection )?( (( _Animate )?( float3( panner391 ,  0.0 ) ):( temp_output_228_0 )) ):( float3( texCoord231 ,  0.0 ) )).xy - float2( 0.5,0.5 ) , float2x2( cos242 , -sin242 , sin242 , cos242 )) + float2( 0.5,0.5 );
				float temp_output_241_0 = ( ShadowSize277 - 0.25 );
				float smoothstepResult254 = smoothstep( temp_output_241_0 , ( temp_output_241_0 + 0.2 ) , NdotL390);
				float cos247 = cos( radians( ( _ShadowTextureRotation + 45.0 ) ) );
				float sin247 = sin( radians( ( _ShadowTextureRotation + 45.0 ) ) );
				float2 rotator247 = mul( (( _ShadowTextureViewProjection )?( (( _Animate )?( float3( panner391 ,  0.0 ) ):( temp_output_228_0 )) ):( float3( texCoord231 ,  0.0 ) )).xy - float2( 0.5,0.5 ) , float2x2( cos247 , -sin247 , sin247 , cos247 )) + float2( 0.5,0.5 );
				float temp_output_236_0 = ( ShadowSize277 - 0.6 );
				float smoothstepResult257 = smoothstep( temp_output_236_0 , ( temp_output_236_0 + 0.2 ) , NdotL390);
				float4 temp_cast_20 = (smoothstepResult252).xxxx;
				float4 ShadowColor157 = (( _UseShadow )?( (( _UseShadowTexture )?( ( temp_output_494_0 + ( Color156 * ( _ShadowColor * ( temp_output_479_0 * ( ( 1.0 - ( ( ( 1.0 - tex2D( _ShadowTexture, rotator250 ) ) * ( 1.0 - smoothstepResult252 ) ) + (( _Level2 )?( ( ( 1.0 - tex2D( _ShadowTexture, rotator242 ) ) * ( 1.0 - smoothstepResult254 ) ) ):( float4( 0,0,0,0 ) )) + (( _Level3 )?( ( ( 1.0 - tex2D( _ShadowTexture, rotator247 ) ) * ( 1.0 - smoothstepResult257 ) ) ):( float4( 0,0,0,0 ) )) ) ) - temp_cast_20 ) ) ) ) ) ):( ( ( ( _ShadowColor * temp_output_479_0 ) * Color156 ) + temp_output_494_0 ) )) ):( Color156 ));
				float2 appendResult416 = (float2(NdotL390 , 0.0));
				float2 N2417 = appendResult416;
				float4 ShadowRamp424 = ( ( tex2D( _ShadowRamp, (N2417*0.5 + 0.5) ) * Color156 ) * ShadowColor157 );
				float2 temp_cast_21 = (_SpecularTextureTiling).xx;
				float2 texCoord330 = IN.ase_texcoord3.xy * temp_cast_21 + float2( 0,0 );
				float cos342 = cos( radians( _SpecularTextureRotation ) );
				float sin342 = sin( radians( _SpecularTextureRotation ) );
				float2 rotator342 = mul( (( _SpecularTextureViewProjection )?( ( ( _SpecularTextureTiling * 1 ) * mul( float4( ase_worldViewDir , 0.0 ), UNITY_MATRIX_VP ).xyz ) ):( float3( texCoord330 ,  0.0 ) )).xy - float2( 0,0 ) , float2x2( cos342 , -sin342 , sin342 , cos342 )) + float2( 0,0 );
				float AddLight320 = (( _Normalize )?( normalizeResult174 ):( temp_output_99_0 ));
				float temp_output_344_0 = ( 1.0 - _SpecularSize );
				float3 normalizeResult332 = normalize( SafeNormalize(_MainLightPosition.xyz) );
				float3 normalizeResult336 = normalize( ase_worldViewDir );
				float3 normalizeResult350 = normalize( ( normalizeResult332 + normalizeResult336 ) );
				float3 normalizeResult346 = normalize( ase_worldNormal );
				float dotResult353 = dot( normalizeResult350 , normalizeResult346 );
				float smoothstepResult355 = smoothstep( temp_output_344_0 , ( temp_output_344_0 + _SpecularBlend ) , dotResult353);
				float4 Specular359 = (( _UseSpecular )?( ( ( ( 1.0 - tex2D( _SpecularMap, rotator342 ) ) * (( _SpecLightColor )?( ( ( _MainLightColor + AddLight320 ) * _SpecularLightIntensity ) ):( _SpecularColor )) ) * smoothstepResult355 ) ):( float4( 0,0,0,0 ) ));
				float temp_output_296_0 = ( 1.0 - _RimSize );
				float dotResult292 = dot( ase_worldNormal , ase_worldViewDir );
				float smoothstepResult312 = smoothstep( temp_output_296_0 , ( temp_output_296_0 + _RimBlend ) , ( ( 1.0 - dotResult292 ) * pow( abs( Lighting276 ) , 0.1 ) ));
				float2 temp_cast_26 = (_RimTextureTiling).xx;
				float2 texCoord291 = IN.ase_texcoord3.xy * temp_cast_26 + float2( 0,0 );
				float cos308 = cos( radians( _RimTextureRotation ) );
				float sin308 = sin( radians( _RimTextureRotation ) );
				float2 rotator308 = mul( (( _RimTextureViewProjection )?( ( ( _RimTextureTiling * 1 ) * mul( float4( ase_worldViewDir , 0.0 ), UNITY_MATRIX_VP ).xyz ) ):( float3( texCoord291 ,  0.0 ) )).xy - float2( 0,0 ) , float2x2( cos308 , -sin308 , sin308 , cos308 )) + float2( 0,0 );
				float4 Rim317 = (( _UseRim )?( ( saturate( smoothstepResult312 ) * ( (( _RimLightColor )?( ( _RimLightIntensity * ( _MainLightColor + AddLight320 ) ) ):( _RimColor )) * tex2D( _RimTexture, rotator308 ) ) ) ):( float4( 0,0,0,0 ) ));
				float2 uv_EmissiveMap = IN.ase_texcoord3.xy * _EmissiveMap_ST.xy + _EmissiveMap_ST.zw;
				float4 Emissiv453 = (( _UseEmissive )?( ( _EmissiveColor * tex2D( _EmissiveMap, uv_EmissiveMap ) ) ):( float4( 0,0,0,0 ) ));
				float3 normalizeResult176 = ASESafeNormalize( temp_output_80_0 );
				float4 Result195 = ( (( _UseRamp )?( ShadowRamp424 ):( ( ( ShadowColor157 * ( 1.0 - temp_output_124_0 ) ) + ( temp_output_124_0 * Color156 ) ) )) + Specular359 + Rim317 + Emissiv453 + float4( (( _Normalize )?( normalizeResult176 ):( temp_output_80_0 )) , 0.0 ) );
				
				
				float3 Albedo = color440.rgb;
				float3 Emission = Result195.rgb;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				MetaInput metaInput = (MetaInput)0;
				metaInput.Albedo = Albedo;
				metaInput.Emission = Emission;
				
				return MetaFragment(metaInput);
			}
			ENDHLSL
		}

	
	}
	
	CustomEditor "TatoonEditorURP"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18935
2152;31;1219;898;6735.356;1091.427;2.717231;True;False
Node;AmplifyShaderEditor.CommentaryNode;186;-6621.498,1165.718;Inherit;False;1621.35;791.9063;Comment;8;182;189;191;188;190;194;193;192;NormalMap;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;192;-6496.326,1663.225;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;193;-6474.307,1824.307;Inherit;False;Property;_NormalStrength;Normal Strength;7;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;190;-6505.327,1444.953;Inherit;True;Property;_Normal;Normal;12;0;Create;True;0;0;0;False;0;False;None;None;False;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;194;-6526.687,1219.257;Inherit;True;Property;_Texture0;Texture 0;11;2;[HideInInspector];[Normal];Create;False;0;0;0;False;0;False;None;None;False;bump;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;191;-6213.332,1220.696;Inherit;True;Property;_TextureSample1;Texture Sample 1;42;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;188;-6184.77,1630.759;Inherit;True;Property;_Texture;Texture;4;2;[NoScaleOffset];[Normal];Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;189;-5443.398,1475.497;Inherit;False;Property;_UseNormalMap;UseNormalMap;5;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;219;-10645.06,-2580.96;Inherit;False;6384.216;1741.552;Shadow;73;274;272;495;486;477;493;494;271;135;161;479;270;269;482;268;267;388;389;264;260;265;266;261;252;259;258;263;262;256;250;248;257;255;254;253;243;247;245;235;251;249;244;246;242;239;238;241;237;236;240;229;395;232;233;231;234;230;226;224;223;391;394;228;227;392;225;393;221;222;220;157;502;505;Shadow tex&color;0,0,0,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;182;-5197.996,1479.718;Inherit;False;NormalMap;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;19;-6969.498,-511.8786;Inherit;False;3319.942;1192.222;Comment;45;195;165;184;162;124;176;171;28;93;151;26;185;22;24;150;25;20;21;174;149;99;11;16;100;9;98;80;173;276;277;318;320;360;390;416;417;426;427;448;454;485;488;490;491;496;Lighting;1,0.9979696,0,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;173;-6885.007,424.9365;Inherit;False;182;NormalMap;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewProjectionMatrixNode;222;-10459.16,-1430.343;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;221;-10475.09,-1253.04;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;220;-9999.305,-1909.648;Inherit;False;Property;_ShadowTextureTiling;Shadow Texture Tiling;23;0;Create;True;0;0;0;False;0;False;1;2.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;225;-10119.58,-1418.158;Inherit;False;2;2;0;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;393;-10098.19,-1204.784;Inherit;False;Property;_XDirectionSpeed;XDirectionSpeed;52;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;392;-10096.85,-1120.745;Inherit;False;Property;_YDirectionSpeed;YDirectionSpeed;53;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleNode;227;-9997.782,-1537.301;Inherit;False;1;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;80;-6663.831,418.4126;Inherit;False;SRP Additional Light;-1;;1;6c86746ad131a0a408ca599df5f40861;7,6,1,9,0,23,1,26,0,27,0,24,0,25,0;6;2;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;15;FLOAT3;0,0,0;False;14;FLOAT3;1,1,1;False;18;FLOAT;0.5;False;32;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.BreakToComponentsNode;98;-6592.974,534.8729;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;228;-9845.81,-1397.316;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;394;-9850.07,-1186.108;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-6498.956,-123.0923;Inherit;False;Property;_ShadowSize;Shadow Size;4;0;Create;True;0;0;0;False;0;False;0.5;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;277;-6233.113,-218.6799;Inherit;False;ShadowSize;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;9;-6646.42,174.277;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;16;-6629.955,47.99187;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMaxOpNode;100;-6432.974,534.8729;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;20;-6643.451,-299.0959;Inherit;False;True;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;223;-9205.182,-1695.717;Inherit;False;Constant;_Float0;Float 0;14;0;Create;True;0;0;0;False;0;False;90;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;391;-9676.348,-1297.95;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldNormalVector;21;-6611.451,-443.096;Inherit;False;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;224;-9290.15,-1911.174;Inherit;False;Property;_ShadowTextureRotation;Shadow Texture Rotation;24;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;226;-9200.91,-1429.253;Inherit;False;Constant;_Float2;Float 2;15;0;Create;True;0;0;0;False;0;False;45;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;361;-10087.7,-3735.146;Inherit;False;3082.016;819.7475;Albedo And Gradient;25;382;380;378;377;376;375;374;373;372;371;370;369;368;367;366;365;364;363;362;140;138;209;156;500;501;MainColor & Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode;22;-6387.451,-363.096;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;99;-6269.695,561.584;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-6406.099,89.66991;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;234;-8907.563,-1425.8;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;395;-9483.512,-1481.87;Inherit;False;Property;_Animate;Animate;54;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;362;-10050.8,-3524.073;Inherit;True;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;232;-8335.319,-1632.365;Inherit;False;277;ShadowSize;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;231;-9721.881,-1944.156;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;230;-8899.764,-1736.5;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;233;-8393.768,-1171.593;Inherit;False;277;ShadowSize;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;365;-9783.755,-3421.854;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;363;-9786.586,-3530.119;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;149;-6240.757,81.9679;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.NormalizeNode;174;-6104.312,575.4169;Inherit;False;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;229;-8202.85,-2056.609;Inherit;False;277;ShadowSize;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;238;-8758.5,-1379.213;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;364;-9856.687,-3177.306;Inherit;False;Property;_GradientRotation;Gradient Rotation;30;0;Create;True;0;0;0;False;0;False;0;90;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;390;-6232.415,-442.3036;Inherit;False;NdotL;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;240;-9348.998,-1808.538;Inherit;False;Property;_ShadowTextureViewProjection;Shadow Texture View Projection;20;0;Create;True;0;0;0;False;0;False;1;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;236;-8039.541,-1160.277;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;237;-8756.831,-1758.13;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;241;-8007.093,-1609.049;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;247;-8527.24,-1421.523;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;245;-7796.208,-1559.602;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-6491.451,-33.09606;Inherit;False;Property;_ShadowBlend;ShadowBlend;6;0;Create;True;0;0;0;False;0;False;0.01;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;150;-6080.757,81.9679;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;242;-8527.979,-1810.414;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;185;-6115.825,236.5933;Inherit;False;Property;_Normalize;Normalize;9;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;367;-9639.562,-3173.516;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;239;-7966.217,-2051.279;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;235;-8860.768,-2051.988;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;246;-7818.656,-1094.832;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;368;-9635.586,-3484.119;Inherit;False;Property;_ChangeAxis;ChangeAxis;49;0;Create;True;0;0;0;False;0;False;1;True;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;251;-8018.509,-1237.652;Inherit;False;390;NdotL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;244;-10248.13,-1755.268;Inherit;True;Property;_ShadowTexture;Shadow Texture;13;1;[NoScaleOffset];Create;True;0;0;0;True;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.Vector2Node;366;-9896.174,-3315.594;Inherit;False;Constant;_Vector0;Vector 0;45;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;249;-7960.061,-1700.424;Inherit;False;390;NdotL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;243;-7810.103,-1957.491;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;254;-7645.16,-1698.842;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;248;-8004.143,-2167.83;Inherit;False;390;NdotL;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;253;-8246.065,-1434.316;Inherit;True;Property;_TextureSample3;Texture Sample 3;13;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;257;-7669.188,-1222.44;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;250;-8481.442,-2208.719;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;255;-8266.811,-1837.449;Inherit;True;Property;_TextureSample2;Texture Sample 2;13;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;370;-9359.71,-3020.031;Inherit;False;Property;_GradientSize;Gradient Size;26;0;Create;True;0;0;0;False;0;False;0;0.89;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;151;-5936.754,97.9679;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotatorNode;369;-9438.906,-3331.469;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;93;-6109.064,-361.5033;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;371;-9358.144,-3128.062;Inherit;False;Property;_GradientPosition;Gradient Position;29;0;Create;True;0;0;0;False;0;False;0;-0.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-6171.45,-65.09598;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;258;-7404.496,-1330.167;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;372;-8951.005,-3095.265;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;28;-5848.272,-262.7847;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;252;-7654.969,-2048.749;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;171;-5708.951,79.71095;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;263;-7811.538,-1436.125;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;256;-8247.549,-2362.262;Inherit;True;Property;_tex1;tex1;13;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;262;-7407.492,-1666.474;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;373;-9197.09,-3324.125;Inherit;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.OneMinusNode;259;-7804.551,-1837.239;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;266;-7232.154,-1460.466;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;260;-7629.534,-2329.422;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;265;-7233.824,-1748.251;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;375;-8816.933,-3272.666;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;376;-9083.063,-3509.429;Inherit;False;Property;_ColorB;Color B;21;0;Create;True;0;0;0;False;0;False;0,0.1264467,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;261;-7427.161,-2055.586;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;374;-9086.454,-3685.146;Inherit;False;Property;_ColorA;Color A;22;0;Create;True;0;0;0;False;0;False;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;-5536.753,-160.8329;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;276;-5285.822,-276.731;Inherit;False;Lighting;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;264;-7220.696,-2076.854;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;377;-8535.305,-3566.208;Inherit;False;Property;_DiffuseColor;Diffuse Color;2;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;388;-6899.875,-1774.005;Inherit;False;Property;_Level2;Level2;50;0;Create;True;0;0;0;False;0;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;140;-8276.937,-3163.377;Inherit;True;Property;_MainTexture;MainTexture;0;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ToggleSwitchNode;389;-6892.172,-1539.879;Inherit;False;Property;_Level3;Level3;51;0;Create;True;0;0;0;False;0;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;378;-8541.84,-3325.241;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;380;-8166.171,-3351.144;Inherit;False;Property;_UseGradient;Use Gradient;17;0;Create;True;0;0;0;False;0;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;267;-6585.963,-1772.699;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;482;-6114.328,-1529.722;Inherit;True;276;Lighting;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;138;-8048.472,-3159.889;Inherit;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;382;-7727.058,-3337.418;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;500;-7774.076,-3483.628;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.OneMinusNode;268;-6325.441,-1765.535;Inherit;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;505;-5866.067,-1588.405;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;269;-6131.811,-1774.672;Inherit;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;501;-7528.051,-3358.531;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;479;-5924.246,-1911.993;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;321;-10092.29,983.5153;Inherit;False;3301.643;968.2485;Specular;38;359;358;357;356;355;354;353;352;351;350;349;348;347;346;345;344;343;342;341;340;339;338;337;336;335;334;333;332;331;330;329;328;327;326;325;324;323;322;Specular;0,1,0.07320952,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;324;-10048.15,1178.442;Inherit;False;Property;_SpecularTextureTiling;Specular Texture Tiling;40;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;271;-5785.265,-1786.888;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;270;-6071.475,-2144.224;Inherit;False;Property;_ShadowColor;Shadow Color;10;0;Create;True;0;0;0;False;0;False;0.5849056,0.5849056,0.5849056,1;0.8018868,0.8018868,0.8018868,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;278;-10113.57,-533.3039;Inherit;False;2870.078;1172.304;;39;280;290;288;299;293;302;298;306;311;316;317;315;313;314;310;312;309;305;308;304;307;297;301;300;296;303;291;292;294;287;289;295;286;284;285;283;282;279;281;Rim;1,0.6396222,0,1;0;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;323;-10042.29,1386.789;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;156;-7259,-3349.511;Inherit;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewProjectionMatrixNode;322;-10044.63,1558.836;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.GetLocalVarNode;161;-5666.058,-1933.387;Inherit;False;156;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ScaleNode;325;-9773.996,1313.536;Inherit;False;1;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;281;-10031.34,315.1791;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;326;-9786.296,1415.062;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewProjectionMatrixNode;279;-10028.46,488.9972;Inherit;False;0;1;FLOAT4x4;0
Node;AmplifyShaderEditor.RangedFloatNode;280;-10036.71,162.6442;Inherit;False;Property;_RimTextureTiling;Rim Texture Tiling;35;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;477;-5639.185,-1809.828;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;135;-5652.952,-2087.768;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;493;-5487.625,-2081.738;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;329;-9305.279,1575.169;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;416;-5964.384,-462.1301;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;320;-5820.793,575.4242;Inherit;False;AddLight;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;494;-5400.131,-1862.512;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;286;-9807.011,393.254;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4x4;0,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;330;-9638.154,1180.933;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;486;-5406.048,-1748.626;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;331;-9326.555,1425.534;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;285;-10040.72,-312.8978;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;283;-10098.81,-140.7738;Inherit;False;276;Lighting;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;282;-10069.72,-469.8978;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;328;-9629.258,1313.536;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ScaleNode;284;-9693.787,290.7111;Inherit;False;1;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;327;-9417.783,1343.552;Inherit;False;Property;_SpecularTextureRotation;Specular Texture Rotation;41;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;337;-8688.996,1228.841;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.NormalizeNode;336;-9072.461,1579.638;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightColorNode;294;-9274.84,-30.01283;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;290;-10047.48,-57.02676;Inherit;False;Constant;_Float4;Float 4;44;0;Create;True;0;0;0;False;0;False;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;425;-3992.389,-2575.573;Inherit;False;1456.25;438.9319;Comment;9;424;503;504;422;423;419;421;420;418;ShadowRamp;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;502;-5218.439,-1795.629;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;335;-8733.801,1347.785;Inherit;False;320;AddLight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;417;-5790.1,-451.3981;Inherit;False;N2;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;495;-5252.068,-2005.556;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;288;-9561.109,-161.5148;Float;False;Property;_RimSize;Rim Size;8;0;Create;True;0;0;0;False;0;False;0.1;0.47;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;291;-9699.059,142.9003;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.AbsOpNode;295;-9918.898,-136.9488;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;287;-9485.851,370.9042;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;289;-9262.436,106.6261;Inherit;False;320;AddLight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;293;-9483.269,490.1271;Inherit;False;Property;_RimTextureRotation;Rim Texture Rotation;37;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;334;-9341.756,1175.232;Inherit;False;Property;_SpecularTextureViewProjection;Specular Texture View Projection;38;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;292;-9841.213,-385.8978;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;333;-9119.453,1315.834;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;332;-9070.937,1425.525;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;301;-8988.637,38.63316;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;299;-9547.101,-83.26283;Float;False;Property;_RimBlend;Rim Blend;3;0;Create;True;0;0;0;False;0;False;0;0.1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;298;-9126.58,-97.07584;Inherit;False;Property;_RimLightIntensity;Rim Light Intensity;42;0;Create;True;0;0;0;False;0;False;0.2;0.413;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;341;-8458.356,1292.019;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;343;-8970.74,1751.994;Inherit;False;Property;_SpecularSize;Specular Size;47;0;Create;True;0;0;0;False;0;False;0.005;0.007;0;0.01;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;340;-8836.324,1425.612;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;272;-5078.313,-1897.09;Inherit;False;Property;_UseShadowTexture;UseShadowTexture;44;0;Create;True;0;0;0;False;0;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RotatorNode;342;-8942.756,1076.744;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;418;-3914.127,-2298.932;Inherit;False;417;N2;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;303;-9806.467,-109.9528;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;338;-8532.152,1406.399;Inherit;False;Property;_SpecularLightIntensity;Specular Light Intensity;46;0;Create;True;0;0;0;False;0;False;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;339;-8890.547,1595.598;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;300;-9617.802,-385.7967;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;296;-9267.088,-247.1119;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;302;-9323.959,350.3221;Inherit;False;Property;_RimTextureViewProjection;Rim Texture View Projection;34;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RadiansOpNode;297;-9258.972,494.3002;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;306;-8595.169,-182.0109;Float;False;Property;_RimColor;Rim Color;1;0;Create;True;0;0;0;False;0;False;0,1,0.8758622,0;0.4339623,0.4339623,0.4339623,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalizeNode;346;-8682.613,1594.994;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;350;-8680.015,1427.514;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;347;-8670.809,1037.518;Inherit;True;Property;_SpecularMap;Specular Map;36;1;[NoScaleOffset];Create;True;0;0;0;False;1;;False;-1;None;None;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;420;-3942.389,-2525.573;Inherit;True;Property;_ShadowRamp;ShadowRamp;57;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.OneMinusNode;344;-8674.418,1760.802;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;309;-9065.535,-200.5919;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;345;-8878.43,1849.314;Inherit;False;Property;_SpecularBlend;Specular Blend;48;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;274;-4850.154,-1972.624;Inherit;False;Property;_UseShadow;UseShadow;27;0;Create;True;0;0;0;False;0;False;1;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;307;-9324.34,-370.5592;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;304;-9047.945,201.6002;Inherit;True;Property;_RimTexture;Rim Texture;32;1;[NoScaleOffset];Create;False;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;305;-8838.136,-55.53286;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;348;-8334.121,1122.549;Inherit;False;Property;_SpecularColor;Specular Color;43;0;Create;True;0;0;0;False;0;False;0,1,0.8745098,0;1,0.9575656,0.75,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;460;-4027.4,-1585.312;Inherit;False;1525.736;493.3411;Emissive;6;451;453;452;450;449;461;Emissive;1,1,1,1;0;0
Node;AmplifyShaderEditor.RotatorNode;308;-9026.793,434.1141;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;349;-8259.263,1327.726;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;421;-3705.389,-2307.573;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;0.5;False;2;FLOAT;0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;352;-8467.606,1760.357;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;157;-4529.555,-1976.406;Inherit;False;ShadowColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;354;-8057.359,1193.12;Inherit;False;Property;_SpecLightColor;Spec Light Color;45;0;Create;True;0;0;0;False;0;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;351;-8334.592,1043.444;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;310;-8732.416,373.4601;Inherit;True;Property;_RimTex;RimTex;25;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;423;-3306.21,-2261.473;Inherit;False;156;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.TexturePropertyNode;449;-3977.4,-1334.218;Inherit;True;Property;_EmissiveMap;EmissiveMap;59;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.DotProductOpNode;353;-8493.086,1494.507;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;419;-3470.89,-2492.973;Inherit;True;Property;_TextureSample4;Texture Sample 4;61;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;312;-8987.199,-385.2789;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;311;-8340.435,-2.39687;Inherit;False;Property;_RimLightColor;Rim Light Color;39;0;Create;True;0;0;0;False;0;False;1;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;314;-8127.96,94.58018;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;422;-3076.11,-2487.673;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;450;-3741.364,-1336.251;Inherit;True;Property;_TextureSample5;Texture Sample 5;63;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;356;-7728.097,1060.791;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;504;-3096.84,-2227.549;Inherit;False;157;ShadowColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;451;-3689.388,-1535.313;Inherit;False;Property;_EmissiveColor;EmissiveColor;61;1;[HDR];Create;True;0;0;0;False;0;False;4,4,4,0;2.996078,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;355;-8302.396,1498.198;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;313;-8692.225,-385.6523;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;503;-2922.84,-2476.549;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;315;-7905.764,-0.176899;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;357;-7467.732,1059.507;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;448;-5217.439,-146.0473;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;488;-5071.929,10.32632;Inherit;False;156;Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;162;-5018.442,-233.829;Inherit;False;157;ShadowColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;452;-3396.503,-1415.899;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;316;-7712.938,-29.89882;Inherit;False;Property;_UseRim;UseRim;15;0;Create;True;0;0;0;False;0;False;1;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;490;-4789.217,-159.8928;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;461;-3197.533,-1443.012;Inherit;False;Property;_UseEmissive;UseEmissive;62;0;Create;True;0;0;0;False;0;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;358;-7258.775,1031.967;Inherit;False;Property;_UseSpecular;UseSpecular;33;0;Create;True;0;0;0;False;0;False;1;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;424;-2781.208,-2491.473;Inherit;False;ShadowRamp;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;496;-4800.706,-61.94607;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;359;-7042.983,1040.713;Inherit;True;Specular;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalizeNode;176;-6280.327,460.8723;Inherit;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;427;-4656.553,65.42588;Inherit;False;424;ShadowRamp;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;453;-2917.064,-1428.475;Inherit;False;Emissiv;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;491;-4605.361,-120.9718;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;317;-7503.033,-30.36684;Inherit;True;Rim;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;454;-4472.706,542.287;Inherit;False;453;Emissiv;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;426;-4398.588,-97.64035;Inherit;False;Property;_UseRamp;UseRamp;58;0;Create;True;0;0;0;False;0;False;0;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;318;-4538.069,386.2617;Inherit;False;317;Rim;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;360;-4537.797,290.5531;Inherit;False;359;Specular;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;184;-5488.086,270.7674;Inherit;False;Property;_Normalize;Normalize;7;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;165;-4166.964,-45.23621;Inherit;True;5;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;471;-2147.199,-2134.107;Inherit;False;829.4039;716.6099;Comment;7;73;458;469;187;196;440;468;Master;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;195;-3884.955,-106.6678;Inherit;False;Result;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;197;-4876.14,1232.439;Inherit;False;2336.603;722.074;Comment;24;413;412;411;410;211;202;210;206;205;199;214;213;208;204;198;212;201;203;215;397;207;200;72;506;Outline;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;470;-6839.955,-3686.134;Inherit;False;1474.649;795.6191;Comment;11;457;459;467;455;456;466;218;217;463;465;464;Metalic/Smoothness;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;209;-7742.24,-3061.942;Inherit;False;Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;196;-2075.218,-1810.88;Inherit;False;195;Result;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;410;-4477.662,1508.254;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;217;-6584.931,-3149.515;Inherit;True;Property;_Smoothness;Smoothness;28;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;198;-3722.796,1866.773;Inherit;False;Constant;_Float3;Float 3;41;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;201;-4016.277,1480.841;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;459;-6150.807,-3367.923;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;458;-1963.441,-1543.922;Inherit;False;Constant;_Float5;Float 1;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;215;-4418.343,1686.973;Inherit;False;Property;_OutlineNoise;OutlineNoise;16;0;Create;True;0;0;0;False;0;False;1;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;203;-3281.603,1470.479;Inherit;False;4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ToggleSwitchNode;214;-3557.339,1512.973;Inherit;False;Property;_OutlineNoise;OutlineNoise;16;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;469;-2094.488,-1644.654;Inherit;False;467;SmoothnessVar;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;187;-2097.198,-1906.491;Inherit;False;182;NormalMap;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;204;-3523.726,1633.613;Inherit;False;209;Alpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;207;-4188.041,1477.105;Inherit;False;Property;_OutlineSize;OutlineSize;14;0;Create;True;0;0;0;False;0;False;0.3;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.UnityObjToViewPosHlpNode;208;-4118.64,1793.682;Inherit;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;457;-6166.617,-3576.934;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;506;-2994.185,1735.411;Inherit;False;Property;_Zwrite;Zwrite;64;1;[IntRange];Create;True;0;0;0;True;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;463;-6298.017,-3436.053;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;411;-4634.224,1573.241;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;211;-4834.136,1379.885;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;466;-5594.434,-3612.532;Inherit;False;MetalicVar;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;412;-4829.189,1543.7;Inherit;False;Property;_XSpeed;XSpeed;55;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;455;-6789.955,-3636.134;Inherit;True;Property;_Metal;Metal;60;0;Create;True;0;0;0;False;0;False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.ToggleSwitchNode;397;-3096.312,1457.927;Inherit;False;Property;_UseOutline;UseOutline;19;0;Create;True;0;0;0;False;0;False;1;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;456;-6549.131,-3636.108;Inherit;True;Property;_TextureSample6;Texture Sample 6;64;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NormalVertexDataNode;205;-3551.363,1330.352;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;468;-2065.808,-1727.264;Inherit;False;466;MetalicVar;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;212;-3834.339,1610.972;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;213;-4618.344,1765.973;Inherit;False;Property;_OutlineNoiseScale;OutlineNoiseScale;25;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;413;-4827.713,1632.324;Inherit;False;Property;_YSpeed;YSpeed;56;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;485;-5568.091,-303.0901;Inherit;False;ShadowMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;465;-6015.213,-3615.399;Inherit;False;Property;_UseMetalSmooth;UseMetalSmooth;63;0;Create;False;0;0;0;False;0;False;0;True;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;200;-3840.411,1438.369;Inherit;False;Property;_UseOutline;UseOutline;19;0;Create;True;0;0;0;False;0;False;1;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;210;-4185.34,1560.973;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;199;-4293.201,1795.904;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMaxOpNode;202;-3475.863,1742.374;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;206;-3312.309,1270.532;Inherit;False;Property;_OutlineColor;OutlineColor;18;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;440;-1963.443,-2084.107;Inherit;False;Constant;_Color0;Color 0;65;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;218;-6589.402,-3382.718;Inherit;True;Property;_Metalic;Metalic;31;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;467;-5595.305,-3353.182;Inherit;False;SmoothnessVar;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;464;-5949.607,-3466.853;Inherit;False;Property;_UseMetalSmooth;UseMetalSmooth;63;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;72;-2821.253,1324.802;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ExtraPrePass;0;0;OutlinePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;True;True;0;4;False;-1;1;False;-1;0;2;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;True;True;1;False;-1;True;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;True;True;False;255;False;-1;255;False;-1;255;False;-1;8;False;-1;3;False;-1;3;False;-1;3;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;True;2;True;506;True;0;False;-1;True;False;0;False;-1;0;False;-1;True;0;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;75;47.59428,-301.4305;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;76;47.59428,-301.4305;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;73;-1599.224,-1868.953;Float;False;True;-1;2;TatoonEditorURP;0;2;TetraArts/Tatoon2/URP/Tatoon2_URP;94348b07e5e8bab40bd6c8a1e3df54cd;True;Forward;0;1;Forward;18;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Overlay=Queue=0;True;2;True;18;all;0;True;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;True;1;False;506;True;0;False;-1;True;False;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;38;Workflow;1;0;Surface;0;0;  Refraction Model;0;0;  Blend;0;0;Two Sided;1;637873397267127940;Fragment Normal Space,InvertActionOnDeselection;0;0;Transmission;0;0;  Transmission Shadow;0.5,False,-1;0;Translucency;0;0;  Translucency Strength;1,False,-1;0;  Normal Distortion;0.5,False,-1;0;  Scattering;2,False,-1;0;  Direct;0.9,False,-1;0;  Ambient;0.1,False,-1;0;  Shadow;0.5,False,-1;0;Cast Shadows;1;0;  Use Shadow Threshold;0;0;Receive Shadows;1;0;GPU Instancing;1;0;LOD CrossFade;1;0;Built-in Fog;1;0;_FinalColorxAlpha;0;0;Meta Pass;1;0;Override Baked GI;0;0;Extra Pre Pass;1;0;DOTS Instancing;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,-1;0;  Type;0;0;  Tess;16,False,-1;0;  Min;10,False,-1;0;  Max;25,False,-1;0;  Edge Length;16,False,-1;0;  Max Displacement;25,False,-1;0;Write Depth;0;0;  Early Z;0;0;Vertex Position,InvertActionOnDeselection;1;0;0;6;True;True;True;True;True;False;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;74;47.59428,-301.4305;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;77;47.59428,-301.4305;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=Universal2D;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;191;0;194;0
WireConnection;188;0;190;0
WireConnection;188;1;192;0
WireConnection;188;5;193;0
WireConnection;189;0;191;0
WireConnection;189;1;188;0
WireConnection;182;0;189;0
WireConnection;225;0;222;0
WireConnection;225;1;221;0
WireConnection;227;0;220;0
WireConnection;80;2;173;0
WireConnection;98;0;80;0
WireConnection;228;0;227;0
WireConnection;228;1;225;0
WireConnection;394;0;393;0
WireConnection;394;1;392;0
WireConnection;277;0;25;0
WireConnection;100;0;98;0
WireConnection;100;1;98;1
WireConnection;391;0;228;0
WireConnection;391;2;394;0
WireConnection;22;0;21;0
WireConnection;22;1;20;0
WireConnection;99;0;100;0
WireConnection;99;1;98;2
WireConnection;11;0;16;1
WireConnection;11;1;9;0
WireConnection;234;0;224;0
WireConnection;234;1;226;0
WireConnection;395;0;228;0
WireConnection;395;1;391;0
WireConnection;231;0;220;0
WireConnection;230;0;224;0
WireConnection;230;1;223;0
WireConnection;365;0;362;2
WireConnection;365;1;362;3
WireConnection;363;0;362;1
WireConnection;363;1;362;2
WireConnection;149;0;11;0
WireConnection;174;0;99;0
WireConnection;238;0;234;0
WireConnection;390;0;22;0
WireConnection;240;0;231;0
WireConnection;240;1;395;0
WireConnection;236;0;233;0
WireConnection;237;0;230;0
WireConnection;241;0;232;0
WireConnection;247;0;240;0
WireConnection;247;2;238;0
WireConnection;245;0;241;0
WireConnection;150;0;149;0
WireConnection;150;1;149;1
WireConnection;242;0;240;0
WireConnection;242;2;237;0
WireConnection;185;0;99;0
WireConnection;185;1;174;0
WireConnection;367;0;364;0
WireConnection;239;0;229;0
WireConnection;235;0;224;0
WireConnection;246;0;236;0
WireConnection;368;0;363;0
WireConnection;368;1;365;0
WireConnection;243;0;239;0
WireConnection;254;0;249;0
WireConnection;254;1;241;0
WireConnection;254;2;245;0
WireConnection;253;0;244;0
WireConnection;253;1;247;0
WireConnection;257;0;251;0
WireConnection;257;1;236;0
WireConnection;257;2;246;0
WireConnection;250;0;240;0
WireConnection;250;2;235;0
WireConnection;255;0;244;0
WireConnection;255;1;242;0
WireConnection;151;0;150;0
WireConnection;151;1;149;2
WireConnection;369;0;368;0
WireConnection;369;1;366;0
WireConnection;369;2;367;0
WireConnection;93;0;22;0
WireConnection;93;1;185;0
WireConnection;26;0;25;0
WireConnection;26;1;24;0
WireConnection;258;0;257;0
WireConnection;372;0;371;0
WireConnection;372;1;370;0
WireConnection;28;0;93;0
WireConnection;28;1;25;0
WireConnection;28;2;26;0
WireConnection;252;0;248;0
WireConnection;252;1;239;0
WireConnection;252;2;243;0
WireConnection;171;0;151;0
WireConnection;171;1;185;0
WireConnection;263;0;253;0
WireConnection;256;0;244;0
WireConnection;256;1;250;0
WireConnection;262;0;254;0
WireConnection;373;0;369;0
WireConnection;259;0;255;0
WireConnection;266;0;263;0
WireConnection;266;1;258;0
WireConnection;260;0;256;0
WireConnection;265;0;259;0
WireConnection;265;1;262;0
WireConnection;375;0;373;0
WireConnection;375;1;371;0
WireConnection;375;2;372;0
WireConnection;261;0;252;0
WireConnection;124;0;28;0
WireConnection;124;1;171;0
WireConnection;276;0;124;0
WireConnection;264;0;260;0
WireConnection;264;1;261;0
WireConnection;388;1;265;0
WireConnection;389;1;266;0
WireConnection;378;0;374;0
WireConnection;378;1;376;0
WireConnection;378;2;375;0
WireConnection;380;0;377;0
WireConnection;380;1;378;0
WireConnection;267;0;264;0
WireConnection;267;1;388;0
WireConnection;267;2;389;0
WireConnection;138;0;140;0
WireConnection;382;0;380;0
WireConnection;382;1;138;0
WireConnection;268;0;267;0
WireConnection;505;0;482;0
WireConnection;269;0;268;0
WireConnection;269;1;252;0
WireConnection;501;0;500;0
WireConnection;501;1;382;0
WireConnection;479;0;505;0
WireConnection;271;0;479;0
WireConnection;271;1;269;0
WireConnection;156;0;501;0
WireConnection;325;0;324;0
WireConnection;326;0;323;0
WireConnection;326;1;322;0
WireConnection;477;0;270;0
WireConnection;477;1;271;0
WireConnection;135;0;270;0
WireConnection;135;1;479;0
WireConnection;493;0;135;0
WireConnection;493;1;161;0
WireConnection;416;0;390;0
WireConnection;320;0;185;0
WireConnection;494;0;505;0
WireConnection;494;1;161;0
WireConnection;286;0;281;0
WireConnection;286;1;279;0
WireConnection;330;0;324;0
WireConnection;486;0;161;0
WireConnection;486;1;477;0
WireConnection;328;0;325;0
WireConnection;328;1;326;0
WireConnection;284;0;280;0
WireConnection;336;0;329;0
WireConnection;502;0;494;0
WireConnection;502;1;486;0
WireConnection;417;0;416;0
WireConnection;495;0;493;0
WireConnection;495;1;494;0
WireConnection;291;0;280;0
WireConnection;295;0;283;0
WireConnection;287;0;284;0
WireConnection;287;1;286;0
WireConnection;334;0;330;0
WireConnection;334;1;328;0
WireConnection;292;0;282;0
WireConnection;292;1;285;0
WireConnection;333;0;327;0
WireConnection;332;0;331;0
WireConnection;301;0;294;0
WireConnection;301;1;289;0
WireConnection;341;0;337;0
WireConnection;341;1;335;0
WireConnection;340;0;332;0
WireConnection;340;1;336;0
WireConnection;272;0;495;0
WireConnection;272;1;502;0
WireConnection;342;0;334;0
WireConnection;342;2;333;0
WireConnection;303;0;295;0
WireConnection;303;1;290;0
WireConnection;300;0;292;0
WireConnection;296;0;288;0
WireConnection;302;0;291;0
WireConnection;302;1;287;0
WireConnection;297;0;293;0
WireConnection;346;0;339;0
WireConnection;350;0;340;0
WireConnection;347;1;342;0
WireConnection;344;0;343;0
WireConnection;309;0;296;0
WireConnection;309;1;299;0
WireConnection;274;0;161;0
WireConnection;274;1;272;0
WireConnection;307;0;300;0
WireConnection;307;1;303;0
WireConnection;305;0;298;0
WireConnection;305;1;301;0
WireConnection;308;0;302;0
WireConnection;308;2;297;0
WireConnection;349;0;341;0
WireConnection;349;1;338;0
WireConnection;421;0;418;0
WireConnection;352;0;344;0
WireConnection;352;1;345;0
WireConnection;157;0;274;0
WireConnection;354;0;348;0
WireConnection;354;1;349;0
WireConnection;351;0;347;0
WireConnection;310;0;304;0
WireConnection;310;1;308;0
WireConnection;353;0;350;0
WireConnection;353;1;346;0
WireConnection;419;0;420;0
WireConnection;419;1;421;0
WireConnection;312;0;307;0
WireConnection;312;1;296;0
WireConnection;312;2;309;0
WireConnection;311;0;306;0
WireConnection;311;1;305;0
WireConnection;314;0;311;0
WireConnection;314;1;310;0
WireConnection;422;0;419;0
WireConnection;422;1;423;0
WireConnection;450;0;449;0
WireConnection;356;0;351;0
WireConnection;356;1;354;0
WireConnection;355;0;353;0
WireConnection;355;1;344;0
WireConnection;355;2;352;0
WireConnection;313;0;312;0
WireConnection;503;0;422;0
WireConnection;503;1;504;0
WireConnection;315;0;313;0
WireConnection;315;1;314;0
WireConnection;357;0;356;0
WireConnection;357;1;355;0
WireConnection;448;0;124;0
WireConnection;452;0;451;0
WireConnection;452;1;450;0
WireConnection;316;1;315;0
WireConnection;490;0;162;0
WireConnection;490;1;448;0
WireConnection;461;1;452;0
WireConnection;358;1;357;0
WireConnection;424;0;503;0
WireConnection;496;0;124;0
WireConnection;496;1;488;0
WireConnection;359;0;358;0
WireConnection;176;0;80;0
WireConnection;453;0;461;0
WireConnection;491;0;490;0
WireConnection;491;1;496;0
WireConnection;317;0;316;0
WireConnection;426;0;491;0
WireConnection;426;1;427;0
WireConnection;184;0;80;0
WireConnection;184;1;176;0
WireConnection;165;0;426;0
WireConnection;165;1;360;0
WireConnection;165;2;318;0
WireConnection;165;3;454;0
WireConnection;165;4;184;0
WireConnection;195;0;165;0
WireConnection;209;0;138;4
WireConnection;410;0;211;0
WireConnection;410;2;411;0
WireConnection;201;0;207;0
WireConnection;459;0;456;4
WireConnection;459;1;217;0
WireConnection;215;1;213;0
WireConnection;203;0;205;0
WireConnection;203;1;214;0
WireConnection;203;2;204;0
WireConnection;203;3;202;0
WireConnection;214;0;200;0
WireConnection;214;1;212;0
WireConnection;208;0;199;0
WireConnection;457;0;456;4
WireConnection;457;1;463;0
WireConnection;463;0;218;0
WireConnection;411;0;412;0
WireConnection;411;1;413;0
WireConnection;466;0;465;0
WireConnection;397;1;203;0
WireConnection;456;0;455;0
WireConnection;212;0;201;0
WireConnection;212;1;210;0
WireConnection;485;0;28;0
WireConnection;465;1;457;0
WireConnection;200;1;201;0
WireConnection;210;0;410;0
WireConnection;210;1;215;0
WireConnection;202;0;208;3
WireConnection;202;1;198;0
WireConnection;467;0;464;0
WireConnection;464;1;459;0
WireConnection;72;0;206;0
WireConnection;72;3;397;0
WireConnection;73;0;440;0
WireConnection;73;1;187;0
WireConnection;73;2;196;0
WireConnection;73;3;468;0
WireConnection;73;4;469;0
WireConnection;73;5;458;0
ASEEND*/
//CHKSM=626AFFAB551019853CA99F1CEC39F5DFC979F950