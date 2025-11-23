//-----------------------------------------------------------------------------
// Copyright(C) Yan Verde - All Rights Reserved
// Copyright protected under Unity Asset Store EULA
// Refer to https://unity3d.com/legal/as_terms for more informations
// -----------------------------------------------------------------------------
// URP Water
// Author : Yan Verde
// Date : April 10, 2021
// -----------------------------------------------------------------------------


#ifndef URPWATER_SCATTERING_INCLUDED
#define URPWATER_SCATTERING_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "URPWaterVariables.hlsl"
#include "URPWaterHelpers.hlsl"


void ComputeScattering(inout GlobalData data, Varyings IN)
{	
	#if _SCATTERING_ON

		Light mainLight = data.mainLight;
		half3 lightColor = mainLight.color;

		// ==================================
		// Scattering
		// ==================================
		half3 L = mainLight.direction;
		half3 V = data.worldViewDir;
		half3 N = lerp(half3(0,1,0), data.worldNormal, _CapsScatterNormals * 0.1);

		#if _DOUBLE_SIDED_ON
		L.y = lerp(-L.y, L.y, data.vFace);
		#endif


		half3 H = normalize(L + N);
		half VdotH = max(0, dot(V, -H));
		VdotH = smoothstep(_ScatteringRangeMin, _ScatteringRangeMax, VdotH);

		half scatterMask = saturate(VdotH * _ScatteringIntensity);

		// ===================================
		// Caps Scattering
		// ===================================

		#if _DISPLACEMENTMODE_GERSTNER || _DISPLACEMENTMODE_GERSTNERPLUS
		#if _CAPS_SCATTERING_ON

		half capsMask = smoothstep(_CapsScatteringRangeMin, _CapsScatteringRangeMax, saturate(IN.texcoord3.y));


		half3 capsNormals = lerp(half3(0, 1, 0), data.worldNormal, _CapsScatterNormals);
		//half NdotL = saturate(dot(IN.normal.xyz, -L) * 0.5 + 0.5);
		half NdotV = saturate(dot(IN.normal.xyz, V));
		half NdotV2 = saturate(dot(capsNormals, V));

		half fresnel = max(0, pow(NdotV,4) * NdotV2 );
		half topDownMask =  saturate(dot(float3(0, 1, 0), -GetViewForwardDir()));

		fresnel *= 1 - smoothstep(0.5, 1, topDownMask);
		half finalCapsMask = saturate(fresnel) * capsMask * _CapsScatteringIntensity;

		scatterMask += finalCapsMask;


		//data.debug.rgb = scatterMask;
		#endif
		#endif

		// ==================================
		// Output
		// ==================================

		//data.debug.rgb = scatterMask;

		half3 scatterColor = _ScatteringColor.rgb * saturate(scatterMask);
		data.scattering = scatterColor * saturate(lightColor);

		#if _DOUBLE_SIDED_ON
		data.scattering = lerp(data.scattering * 0.25, data.scattering, data.vFace);
		#endif


	#endif
}

#endif