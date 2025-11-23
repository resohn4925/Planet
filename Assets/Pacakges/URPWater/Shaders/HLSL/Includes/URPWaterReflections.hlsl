//-----------------------------------------------------------------------------
// Copyright(C) Yan Verde - All Rights Reserved
// Copyright protected under Unity Asset Store EULA
// Refer to https://unity3d.com/legal/as_terms for more informations
// -----------------------------------------------------------------------------
// URP Water
// Author : Yan Verde
// Date : April 10, 2021
// -----------------------------------------------------------------------------

#ifndef URPWATER_REFlECTIONS_INCLUDED
#define URPWATER_REFlECTIONS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "URPWaterVariables.hlsl"
#include "URPWaterHelpers.hlsl"


float3 SampleReflections(float3 worldNormal, GlobalData data, float2 screenUV)
{
	float3 reflection = 0;
	float2 refOffset = 0;
	float3 worldViewDir = data.worldViewDir;

	#if _REFLECTIONMODE_CUBEMAP
		float3 reflectVector = reflect(-worldViewDir, worldNormal);
		reflection = SAMPLE_TEXTURECUBE(_CubemapTexture, sampler_CubemapTexture, reflectVector).rgb;
	#elif _REFLECTIONMODE_PROBES

		float3 reflectVector = reflect(-worldViewDir, worldNormal);
		//reflection = GlossyEnvironmentReflection(reflectVector, data.worldPosition, _ReflectionRoughness, 1, screenUV);
		//reflection = GlossyEnvironmentReflection(reflectVector, _ReflectionRoughness, 1);

		
		#if USE_FORWARD_PLUS
		reflection = SAMPLE_TEXTURECUBE_LOD(_GlossyEnvironmentCubeMap, sampler_GlossyEnvironmentCubeMap, reflectVector, _ReflectionRoughness).rgb;
		#else
		reflection = DecodeHDREnvironment(SAMPLE_TEXTURECUBE_LOD(unity_SpecCube0, samplerunity_SpecCube0, reflectVector, _ReflectionRoughness), unity_SpecCube0_HDR);
		#endif
		
		
	#elif _REFLECTIONMODE_REALTIME

		/*
		// get the perspective projection
		float2 p11_22 = float2(unity_CameraInvProjection._11, unity_CameraInvProjection._22) * 10;
		// conver the uvs into view space by "undoing" projection
		float3 viewDir = -(float3((screenUV * 2 - 1) / p11_22, -1));

		half3 viewNormal = mul(worldNormal, (float3x3)GetWorldToViewMatrix()).xyz;
		half3 reflectVector = reflect(-viewDir, viewNormal);
		*/


		float3 reflectVector = reflect(-worldViewDir, worldNormal);

		float2 reflectionUV = screenUV + worldNormal.zx * float2(0.02, 0.15);
		reflection = SAMPLE_TEXTURE2D_LOD(_ReflectionTexture, sampler_ScreenTextures_linear_clamp, reflectionUV, 6 * _ReflectionRoughness).rgb;//planar reflection
	#endif


	return reflection;
}


void ComputeReflections(inout GlobalData data, Varyings IN) 
{

#if _REFLECTIONMODE_CUBEMAP || _REFLECTIONMODE_PROBES || _REFLECTIONMODE_REALTIME

	// Reflection Distortion
	float3 reflectionNormal = data.worldNormal;
	float3 n = IN.normal.xyz;
	//n.xz *= _ReflectionDistortion;
	reflectionNormal = lerp(n, reflectionNormal, _ReflectionDistortion);

	float3 reflection = SampleReflections(reflectionNormal, data, data.screenUV.xy);

	// Fresnel Distortion
	#if _NORMAL_FADE_ON
	float fresnelFade = IN.paramsA.x * _ReflectionFresnelNormal;
	#else
	float fresnelFade = _ReflectionFresnelNormal;
	#endif

	// Fresnel mask
	float3 fresnelNormal = lerp(IN.normal.xyz, data.worldNormal, fresnelFade);
	float fresnel = 1 - dot(fresnelNormal, data.worldViewDir);
	fresnel = pow(saturate(fresnel), _ReflectionFresnel);

	float reflectionMask = _ReflectionIntensity;
	#if _FOAM_ON
	reflectionMask *= 1 - data.foamMask * 2;
	#endif
	//fresnel = smoothstep(0, _ReflectionFresnel, saturate(fresnel));
	
	float3 finalReflection = lerp(data.finalColor, saturate(reflection), fresnel * saturate(reflectionMask));

	#if _DOUBLE_SIDED_ON
	data.finalColor = lerp(data.finalColor, finalReflection, data.vFace);
	#else
	data.finalColor = finalReflection;
	#endif

#endif
	data.finalColor += data.addLight;

}

#endif