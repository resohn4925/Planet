//-----------------------------------------------------------------------------
// Copyright(C) Yan Verde - All Rights Reserved
// Copyright protected under Unity Asset Store EULA
// Refer to https://unity3d.com/legal/as_terms for more informations
// -----------------------------------------------------------------------------
// URP Water
// Author : Yan Verde
// Date : April 10, 2021
// -----------------------------------------------------------------------------

#ifndef URPWATER_NORMALS_INCLUDED
#define URPWATER_NORMALS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"   
#include "URPWaterHelpers.hlsl"
#include "URPWaterVariables.hlsl"

// Flow map
half4 SampleFlowMap(Texture2D tex,	half2 texUV, Texture2D flowMap,half2 uv, half speed,	half intensity)
{

	half4 flowVal = (SAMPLE_TEXTURE2D(flowMap, URPWater_trilinear_repeat_sampler, uv) * 2 - 1) * intensity;

	half dif1 = frac(_Time.x * speed + 0.5);
	half dif2 = frac(_Time.x * speed);

	half lerpVal = abs((0.5 - dif1) / 0.5);

	half4 col1 = SAMPLE_TEXTURE2D(tex, URPWater_trilinear_repeat_sampler, texUV - flowVal.xy * dif1);
	half4 col2 = SAMPLE_TEXTURE2D(tex, URPWater_trilinear_repeat_sampler, texUV - flowVal.xy * dif2);

	return lerp(col1, col2, lerpVal);
}

// Global Normal Blend
half3 UnpackScaleNormal(half4 packednormal, half bumpScale) {
	#if defined(UNITY_NO_DXT5nm)
		return packednormal.xyz * 2 - 1;
	#else
		half3 normal;
		normal.xy = (packednormal.wy * 2 - 1);
		#if (SHADER_TARGET >= 30)
			// SM2.0: instruction count limitation
			// SM2.0: normal scaler is not supported
			normal.xy *= bumpScale;
		#endif
		normal.z = sqrt(1.0 - saturate(dot(normal.xy, normal.xy)));
		return normal;
	#endif
}

half3 BlendNormals(half3 n1, half3 n2)
{
	return normalize(half3(n1.xy + n2.xy, n1.z * n2.z));
}

half3 NormalBlendReoriented(half3 A, half3 B)
{
	half3 t = A.xyz + half3(0.0, 0.0, 1.0);
	half3 u = B.xyz * half3(-1.0, -1.0, 1.0);
	return (t / t.z) * dot(t, u) - u;
}

void ComputeNormals(inout GlobalData data, Varyings IN)
{
	/*
	half3 tangentNormal;

	#if _NORMALSMODE_SINGLE


		half4 nA = SAMPLE_TEXTURE2D(_NormalMapA, URPWater_trilinear_repeat_sampler, IN.texcoord.xy);
		half3 normalA = UnpackNormal(nA);
		normalA.xy *= -1;

		tangentNormal = normalA;

	#endif

	half3 normalWS = TransformTangentToWorld(tangentNormal, data.tangentToWorld, true);
	data.worldNormal = normalWS;
	*/

	
	#if _NORMALSMODE_FACET
		half3 dpdx = ddx(data.worldPosition);
		half3 dpdy = ddy(data.worldPosition);
		data.worldNormal = normalize(cross(dpdy, dpdx));
	#else
		half3 tangentNormal;

		#if _NORMALSMODE_SINGLE || _NORMALSMODE_DUAL


			half4 nA = SAMPLE_TEXTURE2D(_NormalMapA, URPWater_trilinear_repeat_sampler, IN.texcoord.xy);
			half4 nB = SAMPLE_TEXTURE2D(_NormalMapA, URPWater_trilinear_repeat_sampler, IN.texcoord.zw);

			half3 normalA = UnpackScaleNormal(nA, _NormalMapAIntensity);
			half3 normalB = UnpackScaleNormal(nB, _NormalMapAIntensity);

			tangentNormal = NormalBlendReoriented(normalA, normalB);

			#if _NORMALSMODE_DUAL
				half3 normalC = UnpackScaleNormal(SAMPLE_TEXTURE2D(_NormalMapB, URPWater_trilinear_repeat_sampler, IN.texcoord1.xy), _NormalMapBIntensity);
				tangentNormal = NormalBlendReoriented(tangentNormal, normalC);
			#endif
		#endif

		#if _NORMALSMODE_FLOWMAP
			half4 flowNormal = SampleFlowMap(_NormalMapA, IN.texcoord.xy, _FlowMap, IN.texcoord.zw, _FlowSpeed, _FlowIntensity);
			tangentNormal = UnpackNormalScale(flowNormal, _NormalMapAIntensity);
		#endif

		// Far Normals
		#if _NORMAL_FAR_ON
			half4 nfA = SAMPLE_TEXTURE2D(_NormalMapFar, URPWater_trilinear_repeat_sampler, IN.texcoord2.xy);
			half4 nfB = SAMPLE_TEXTURE2D(_NormalMapFar, URPWater_trilinear_repeat_sampler, IN.texcoord2.zw);

			half3 normalFarA = UnpackScaleNormal(nfA, _NormalMapFarIntensity);
			half3 normalFarB = UnpackScaleNormal(nfB, _NormalMapFarIntensity);

			half3 farNormals = NormalBlendReoriented(normalFarA, normalFarB);

			tangentNormal = lerp(tangentNormal, farNormals, IN.texcoord3.x);
		#endif

		// Dynamic Normal
		#if _DYNAMIC_EFFECTS_ON
			
			half4 normalDynamic = data.dynamicData;
			normalDynamic = normalDynamic * 2 - 1;
			normalDynamic.xy *= _DynamicNormal.xx * data.dynamicData.aa;

			#if defined(UNITY_NO_DXT5nm)
			normalDynamic.b = 1;
			#else
			normalDynamic.b = sqrt(1.0 - saturate(dot(normalDynamic.xy, normalDynamic.xy)));
			#endif

			tangentNormal = NormalBlendReoriented(tangentNormal, normalDynamic.rgb);
		#endif

		#if _DISPLACEMENTMODE_GERSTNERPLUS
		//tangentNormal.xy *= -1;
		#endif

		// Combined tangent to world
		half3 normalWS = TransformTangentToWorld(tangentNormal, data.tangentToWorld);
		data.worldNormal = normalize(normalWS);
	#endif


	#if _DOUBLE_SIDED_ON
		half3 flippedNormals = data.worldNormal;
		flippedNormals.y = -flippedNormals.y;

		data.worldNormal = lerp(flippedNormals, data.worldNormal, data.vFace);
	#endif
	
}

#endif
