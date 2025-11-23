//-----------------------------------------------------------------------------
// Copyright(C) Yan Verde - All Rights Reserved
// Copyright protected under Unity Asset Store EULA
// Refer to https://unity3d.com/legal/as_terms for more informations
// -----------------------------------------------------------------------------
// URP Water
// Author : Yan Verde
// Date : April 10, 2021
// -----------------------------------------------------------------------------

#ifndef URPWATER_GERSTNERPLUS_INCLUDED
#define URPWATER_GERSTNERPLUS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"   
#include "URPWaterHelpers.hlsl"
#include "URPWaterVariables.hlsl"

struct WavePlus
{
    float Length;
    float Steepness;
    float Speed;
    float Direction;
    float DirectionOffset;
};

//#define PI 3.141592f
#define PI_2 PI * 2.f
#define WAVECOUNT 4.0

static float ClusterLengthMul[] = { 1.f, 0.8f, 0.5f, 0.3f };
static float ClusterSteepnessMul[] = { 1.f, 0.8f, 0.7f, 0.6f };
static float ClusterSpeedMul[] = { 1.f, 1.3f, 1.5f, 1.8f };
static float ClusterOffsetMul[] = { 0.0f, 0.1249f, 0.24845f, 0.52148f };

static float WaveLengthMul[] = { 1.f, 0.8f, 0.6f, 0.4f };
static float WaveSteepnessMul[] = { 1.f, 0.9f, 0.8f, 0.6f };
static float WaveSpeedMul[] = { 1.f, 1.3f, 1.5f, 1.8f };
static float WaveOffsetMul[] = { 0.0f, 0.1249f, 0.24845f, 0.52148f };


float3 GerstnerWave(WavePlus w, float3 p, inout float3 tangent, inout float3 binormal)
{
    float steepness = w.Steepness;
    float wavelength = w.Length;
    float k = 2 * PI / wavelength;
    float c = sqrt(9.8 / k);
    float time = _Time.x;

    float DirectionRads = (w.Direction + (w.DirectionOffset)) * PI_2;
    float2 d = normalize(float2(cos(DirectionRads), sin(DirectionRads))); // Calculate this in C# ?
    float f = k * (dot(d, p.xz) - c * time * w.Speed);
    float a = steepness / k;

    float sine = sin(f);
    float cosine = cos(f);

    // Tangent
    tangent += float3(
        -d.x * d.x * (steepness * sine),
        d.x * (steepness * cosine),
        -d.x * d.y * (steepness * sine)
        );
    // Binormal
    binormal += float3(
        -d.x * d.y * (steepness * sine),
        d.y * (steepness * cosine),
        -d.y * d.y * (steepness * sine)
        );

    // WPO
    return float3(
        d.x * (a * cosine),
        a * sine,
        d.y * (a * cosine)
        );
}

void ComputeGerstnerWavesPlus(float3 worldPos, inout float3 offsets, inout half3 normal, inout half3 binormal, inout half3 tangent)
{

    float3 WPO = 0.0;

    half3 Normal = half3(0, 0, 0);
    half3 Binormal = half3(0, 0, 1);
    half3 Tangent = half3(1, 0, 0);

    half ClustersCount = _ClustersCount;

    half WaveLength[] = { _WaveParams1.x, _WaveParams2.x, _WaveParams3.x, _WaveParams4.x };
    half WaveSteepness[] = { _WaveParams1.y, _WaveParams2.y, _WaveParams3.y, _WaveParams4.y };
    half WaveSpeed[] = { _WaveParams1.z, _WaveParams2.z, _WaveParams3.z, _WaveParams4.z };
    half WaveDirection[] = { _WaveParams1.w, _WaveParams2.w, _WaveParams3.w, _WaveParams4.w };
    half WaveOffset[] = { _WaveOffsets.x, _WaveOffsets.y, _WaveOffsets.z, _WaveOffsets.w };

    float maxWaves = min(_WavesPerCluster, WAVECOUNT);

    UNITY_LOOP
    for (int c = 0; c < ClustersCount; c++)
    {
        half clusterLength = WaveLength[c] * ClusterLengthMul[c];
        half clusterSteepness = WaveSteepness[c] * ClusterSteepnessMul[c];
        half clusterSpeed = WaveSpeed[c] * ClusterSpeedMul[c];
        half clusterDirection = WaveDirection[c];
        half clusterOffset = WaveOffset[c] + ClusterOffsetMul[c];

        UNITY_LOOP
        for (int x = 0; x < maxWaves; x++)
        {
            WavePlus w;
            w.Length = clusterLength * WaveLengthMul[x];
            w.Steepness = (clusterSteepness * WaveSteepnessMul[x]) / ((WAVECOUNT * ClustersCount * 0.25));
            w.Speed = clusterSpeed * WaveSpeedMul[x];
            w.Direction = clusterDirection;
            w.DirectionOffset = clusterOffset * WaveOffsetMul[x];

            WPO += GerstnerWave(w, worldPos, Tangent, Binormal);
        }
    }

    offsets = WPO;

    normal = normalize(cross(Binormal, Tangent));
    binormal = Binormal;
    tangent = Tangent;

    #ifndef _WORLD_UV
    binormal = -TransformObjectToWorldDir(half3(0, 0, 1));
    tangent = -TransformObjectToWorldDir(half3(1, 0, 0));
    #endif
}

#endif
