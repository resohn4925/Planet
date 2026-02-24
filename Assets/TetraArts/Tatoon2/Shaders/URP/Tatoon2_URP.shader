Shader "Custom/Tatoon2_Simplified_URP"
{
    Properties
    {
        // 1. 漫反射贴图与颜色
        [MainColor] _BaseColor("Base Color", Color) = (1,1,1,1)
        [MainTexture] _MainTex("Base Map (Diffuse)", 2D) = "white" {}

        // 2. 颜色区域硬变化 (Toon Shading)
        _ShadowColor("Shadow Color", Color) = (0.5, 0.5, 0.5, 1)
        _Step("Toon Step (Threshold)", Range(0, 1)) = 0.5
        _Feather("Toon Feather (Smoothness)", Range(0, 0.1)) = 0.01

        // 3. 描边功能 - 修复偏移问题 + 上限 0.2
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth("Outline Width", Range(0, 0.2)) = 0.02

        // 4. Bump贴图 (Normal Map)
        [Normal] _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Normal Strength", Range(0, 2)) = 1.0

        // 5. 环境反射光调节
        _EnvReflectIntensity("Env Reflect Intensity", Range(0, 2)) = 0.2
    }

    SubShader
    {
        // 修改：稍微提高渲染顺序，确保先于普通几何体渲染，并留出空间给透明光效插件
        Tags { 
            "RenderPipeline" = "UniversalPipeline" 
            "RenderType" = "Opaque" 
            "Queue" = "Geometry+10" 
        }

        // --- Pass 1: 描边通道 ---
        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Cull Front
            ZWrite On
            // 关键修复：Offset 指令将描边深度往后拉一点，防止它和模型表面重叠导致偏移感
            Offset 1, 1

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
                
                // 将位置转换到裁剪空间
                float4 posCS = TransformObjectToHClip(input.positionOS.xyz);
                // 转换法线到屏幕空间（裁剪空间）
                float3 normalCS = TransformWorldToHClipDir(TransformObjectToWorldNormal(input.normalOS));
                
                // 修复逻辑：
                // 1. 乘以 posCS.w 保证远近粗细均匀
                // 2. 0.01 是为了适配面板上的 Range(0, 0.2)
                output.positionCS = posCS;
                output.positionCS.xy += normalCS.xy * _OutlineWidth * posCS.w * 0.1;
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target {
                return _OutlineColor;
            }
            ENDHLSL
        }

        // --- Pass 2: 正向渲染通道 ---
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            // 显式声明深度测试，防止旋转时闪烁
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // 增加更多光源关键字，增强在不同 URP 设置下的光源稳定性
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT

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

            // 卡通光照计算函数
            half3 CalculateToonLight(Light light, float3 normalWS, half3 shadowColor) {
                half NdotL = dot(normalWS, light.direction);
                half lightIntensity = smoothstep(_Step - _Feather, _Step + _Feather, NdotL);
                // 衰减 = 距离衰减 * 阴影衰减
                half attenuation = light.distanceAttenuation * light.shadowAttenuation;
                return light.color * attenuation * lerp(shadowColor, half3(1,1,1), lightIntensity);
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

                // 法线贴图处理
                float3 normalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, input.uv), _BumpScale);
                float3 bitangentWS = cross(input.normalWS, input.tangentWS.xyz) * input.tangentWS.w;
                float3x3 tbn = float3x3(input.tangentWS.xyz, bitangentWS, input.normalWS);
                float3 normalWS = normalize(mul(normalTS, tbn));

                // 1. 主光源 (强光)
                float4 shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                Light mainLight = GetMainLight(shadowCoord);
                half3 finalLighting = CalculateToonLight(mainLight, normalWS, _ShadowColor.rgb);

                // 2. 附加光源 (接收商店的点光源、弱光)
                #if defined(_ADDITIONAL_LIGHTS) || defined(_ADDITIONAL_LIGHTS_VERTEX)
                uint pixelLightCount = GetAdditionalLightsCount();
                for (uint lightIndex = 0u; lightIndex < pixelLightCount; ++lightIndex) {
                    Light addLight = GetAdditionalLight(lightIndex, input.positionWS);
                    finalLighting += CalculateToonLight(addLight, normalWS, _ShadowColor.rgb);
                }
                #endif

                // 3. 环境光 (修正原脚本反射过强问题)
                half3 ambient = SampleSH(normalWS) * _EnvReflectIntensity;
                
                half3 finalColor = texColor.rgb * (finalLighting + ambient);
                return half4(finalColor, texColor.a);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}