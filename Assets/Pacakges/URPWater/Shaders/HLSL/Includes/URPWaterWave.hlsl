//-----------------------------------------------------------------------------
// Copyright(C) Yan Verde - All Rights Reserved
// Copyright protected under Unity Asset Store EULA
// Refer to https://unity3d.com/legal/as_terms for more informations
// -----------------------------------------------------------------------------
// URP Water
// Author : Yan Verde
// Date : April 10, 2021
// -----------------------------------------------------------------------------

#ifndef URPWATER_WAVE_INCLUDED
#define URPWATER_WAVE_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"   
#include "URPWaterHelpers.hlsl"
#include "URPWaterVariables.hlsl"
#include "URPWaterGerstnerSimple.hlsl"
#include "URPWaterGerstnerPlus.hlsl"

void ComputeWaves(inout Attributes v, inout half3 outNormal, inout half3 outBinormal, inout half3 outTangent)
{
	half3 WavesNormal	= outNormal;
	half3 worldBinormal = outBinormal;
	half3 worldTangent	= outTangent;

	#if _DISPLACEMENTMODE_GERSTNER || _DISPLACEMENTMODE_GERSTNERPLUS
	
	float3 WavesOffset;
	float3 wPos = TransformObjectToWorld(v.vertex.xyz);
	
	#if _DISPLACEMENTMODE_GERSTNER
	ComputeGersterWaves(wPos, WavesOffset, WavesNormal);
	#endif

	#if _DISPLACEMENTMODE_GERSTNERPLUS
	ComputeGerstnerWavesPlus(wPos, WavesOffset, WavesNormal, worldBinormal, worldTangent);
	#endif

	float amplitude = _WaveAmplitude;

	// Distance Fade
	float cameraDistance = length(_WorldSpaceCameraPos - wPos);
	float distanceMask = 1.0 - saturate(InverseLerp(_WaveStartDistance, _WaveEndDistance, cameraDistance));
	amplitude *= distanceMask;

	#if _DISPLACEMENT_MASK_ON
	amplitude *= v.color.b;
	#endif

	#if _OCEAN_MASK_ON
	float2 pos = _OceanInfos.xy;
	float2 size = _OceanInfos.zw;

	float2 uv = CaptureUV(wPos.xz, pos, size);
	amplitude *= CaptureMask(uv, 0.4);
	#endif


	// TODO: Calculate waveHeight only if caps scattering or white caps
	/*
	float waveHeight = max(0.01, WavesOffset.y - v.vertex.y);
	float diff = (WavesOffset.y + _WaveEffectsBoost) - v.vertex.y;
	float dist = distance(v.vertex.xyz, v.vertex.xyz + WavesOffset.xz);

	v.waveHeight = (dist * diff) / waveHeight;
	v.waveHeight = saturate(v.waveHeight);
	v.waveHeight *= amplitude;
	*/

	//float wavesBound = v.vertex.y 

	float waveLength = (WavesOffset.y + _WaveEffectsBoost) - v.vertex.y;
	float waveHeight = max(0.01, waveLength);
	float l = length(WavesOffset.xyz);

	v.waveHeight = saturate(waveHeight * amplitude);


	v.vertex.xyz = TransformWorldToObject(wPos + WavesOffset * amplitude.xxx);
	WavesNormal = lerp(v.normal, WavesNormal, amplitude * _WaveNormal);

	#if _DISPLACEMENTMODE_GERSTNERPLUS
	worldBinormal	= lerp(outBinormal, worldBinormal, amplitude);
	worldTangent	= lerp(outTangent, worldTangent, amplitude);
	#endif

#endif

	outNormal = WavesNormal;
	outBinormal = worldBinormal;
	outTangent = worldTangent;
}

#endif
