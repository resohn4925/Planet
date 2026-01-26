Shader "Hidden/Ripple Effect URP"
{
    Properties
    {
        _MainTex("Base", 2D) = "white" {}
        _GradTex("Gradient", 2D) = "white" {}
        _Reflection("Reflection Color", Color) = (0, 0, 0, 0)
        _Params1("Parameters 1", Vector) = (1, 1, 0.8, 0)
        _Params2("Parameters 2", Vector) = (1, 1, 1, 0)
        _Drop1("Drop 1", Vector) = (0.49, 0.5, 0, 0)
        _Drop2("Drop 2", Vector) = (0.50, 0.5, 0, 0)
        _Drop3("Drop 3", Vector) = (0.51, 0.5, 0, 0)
        _radius("radius", Vector) = (0.50, 0.5, 0, 0)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        
        Cull Off
        ZWrite Off
        ZTest Always

        Pass
        {
            Name "RippleEffect"
            
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"

            TEXTURE2D_X(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize;

            TEXTURE2D(_GradTex);
            SAMPLER(sampler_GradTex);

            half4 _Reflection;
            float4 _Params1;
            float4 _Params2;
            float3 _Drop1;
            float3 _Drop2;
            float3 _Drop3;
            float3 _radius;

            float wave(float2 position, float2 origin, float time)
            {
                float d = length(position - origin);
                float t = time - d * _Params1.z;
                return (SAMPLE_TEXTURE2D(_GradTex, sampler_GradTex, float2(t, 0)).a - 0.5f) * 2;
            }

            float allwave(float2 position)
            {
                return wave(position, _Drop1.xy, _Drop1.z);
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
                
                float2 dx = float2(0.01f, 0);
                float2 dy = float2(0, 0.01f);

                float2 p = uv * _Params1.xy;
                float d = length(p - _Drop1.xy);
                float scale = saturate(_radius.x - d);

                float w = allwave(p);
                float2 dw = float2(allwave(p + dx * scale) - w, allwave(p + dy * scale) - w);

                float2 duv = dw * _Params2.xy * 0.2f * _Params2.z;
                half4 c = SAMPLE_TEXTURE2D_X(_MainTex, sampler_MainTex, uv + duv);
                
                float fr = pow(length(dw) * 3, 3);

                return lerp(c, _Reflection, fr);
            }
            ENDHLSL
        }
    }
    
    Fallback Off
}