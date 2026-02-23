Shader "Custom/Tatoon2_Simplified_URP"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1,1,1,1)
        [MainTexture] _MainTex("Base Map (Diffuse)", 2D) = "white" {}

        _ShadowColor("Shadow Color", Color) = (0.5, 0.5, 0.5, 1)
        _Step("Toon Step (Threshold)", Range(0, 1)) = 0.5
        _Feather("Toon Feather (Smoothness)", Range(0, 0.1)) = 0.01

        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth("Outline Width", Range(0, 0.5)) = 0.02

        [Normal] _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Normal Strength", Range(0, 2)) = 1.0

        _EnvReflectIntensity("Env Reflect Intensity", Range(0, 2)) = 0.2
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" "Queue" = "Geometry" }

        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Cull Front 
            ZWrite On

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
            };

            float _OutlineWidth;
            float4 _OutlineColor;

            Varyings vert(Attributes input) {
                Varyings output;
                float3 posWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                
                // 沿法线挤出
                posWS += normalWS * _OutlineWidth;
                output.positionCS = TransformWorldToHClip(posWS);
                return output;
            }

            half4 frag(Varyings input) : SV_Target {
                return _OutlineColor;
            }
            ENDHLSL
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // 主光源阴影 + 其他光源
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD3;
                float4 tangentWS : TEXCOORD4;
            };

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
            TEXTURE2D(_BumpMap); SAMPLER(sampler_BumpMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                half4 _BaseColor;
                half4 _ShadowColor;
                half _Step;
                half _Feather;
                half _BumpScale;
                half _EnvReflectIntensity;
                float4 _OutlineColor;
                float _OutlineWidth;
            CBUFFER_END

            half3 CalculateToonLight(Light light, float3 normalWS, half3 shadowColor) {
                half NdotL = dot(normalWS, light.direction);

                half lightIntensity = smoothstep(_Step - _Feather, _Step + _Feather, NdotL);
                return light.color * (light.distanceAttenuation * light.shadowAttenuation) * lerp(shadowColor, half3(1,1,1), lightIntensity);
            }

            Varyings vert(Attributes input) {
                Varyings output;
                VertexPositionInputs posInputs = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = posInputs.positionCS;
                output.positionWS = posInputs.positionWS;
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.tangentWS = float4(TransformObjectToWorldDir(input.tangentOS.xyz), input.tangentOS.w);
                return output;
            }

            half4 frag(Varyings input) : SV_Target {
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv) * _BaseColor;

                float3 normalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, input.uv), _BumpScale);
                float3 bitangentWS = cross(input.normalWS, input.tangentWS.xyz) * input.tangentWS.w;
                float3x3 tbn = float3x3(input.tangentWS.xyz, bitangentWS, input.normalWS);
                float3 normalWS = normalize(mul(normalTS, tbn));

                float4 shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                Light mainLight = GetMainLight(shadowCoord);
                half3 finalLighting = CalculateToonLight(mainLight, normalWS, _ShadowColor.rgb);

                #ifdef _ADDITIONAL_LIGHTS
                uint pixelLightCount = GetAdditionalLightsCount();
                for (uint lightIndex = 0u; lightIndex < pixelLightCount; ++lightIndex) {
                    Light addLight = GetAdditionalLight(lightIndex, input.positionWS);
                    finalLighting += CalculateToonLight(addLight, normalWS, _ShadowColor.rgb);
                }
                #endif

                half3 ambient = SampleSH(normalWS) * _EnvReflectIntensity;
                
                half3 finalColor = texColor.rgb * (finalLighting + ambient);
                return half4(finalColor, texColor.a);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}