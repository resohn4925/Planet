Shader "AdriftTeam/Water"
{
    Properties
    {
        _WaterColor("Water Color", Color) = (0, 0, 0, 1)
        _FadeColor("Fade Color", Color) = (1, 1, 1, 1)
        _FoamColor("Foam Color", Color) = (1, 1, 1, 1)
        _FadeDistance("Fade Distance", Float) = 1.0
        _FlowDirection("Flow Direction", Float) = 1.0
        _DistortionStrength("Distortion Strength", Float) = 0.1
        _DistortionSpeed("Distortion Speed", Float) = 1.0
        _NoiseTexture("Noise Texture", 2D) = "white" {} // Exposed noise texture
        _Tiling("Noise Tiling", Float) = 1.0
        _NoiseOpacity("Noise Opacity", Float) = 1.0
        _NoiseRefraction("Noise Refraction Texture", 2D) = "white" {} // New noise refraction property
        _RefractionTiling("Refraction Tiling", Float) = 1.0 // Tiling for noise refraction
        _NoiseFoam("Noise Foam Texture", 2D) = "white" {} // New noise refraction property
        _FoamAmount("Foam Amount", Float) = 1.0
        _FoamScale("Foam Tiling", Float) = 1.0
        _FoamCutoff("Foam Cutoff", Float) = 1.0
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 200

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                float4 _WaterColor;
                float4 _FadeColor;
                float4 _FoamColor;
                float _FadeDistance;
                float _FlowDirection;
                float _DistortionStrength;
                float _DistortionSpeed;
                sampler2D _NoiseTexture;
                sampler2D _NoiseFoam;
                sampler2D _NoiseRefraction; // New noise refraction sampler
                float _Tiling;
                float _RefractionTiling;
                float _FoamAmount;
                float _FoamScale;
                float _FoamCutoff;
                float _NoiseOpacity;

                sampler2D _CameraDepthTexture;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 pos : SV_POSITION;
                    float4 screenPos : TEXCOORD1;
                    float3 worldPos : TEXCOORD2; // World position
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.uv = v.uv;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.screenPos = ComputeScreenPos(o.pos);
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // Get world position
                    return o; 
                }

                float DepthFade(float4 screenPos, float fadeDistance)
                {
                    float existingDepthLinear = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, screenPos).r);
                    float depthDifference = existingDepthLinear - screenPos.w;
                    float fade = saturate(depthDifference / fadeDistance);
                    return smoothstep(1.0, 0.0, fade);
                }

                float2 FlowDirection(float flowDir)
                {
                    float2 direction;
                    direction.x = cos((flowDir * 2.0 - 1.0) * 3.14);
                    direction.y = sin((flowDir * 2.0 - 1.0) * 3.14);
                    return direction;
                }

                float FoamEdge(float4 screenPos, float foamDistance, float foamCutoff)
                {
                    float existingDepthLinear = LinearEyeDepth(tex2Dproj(_CameraDepthTexture, screenPos).r);
                    float depthDifference = existingDepthLinear - screenPos.w;
                    float foam = saturate(depthDifference / foamDistance);

                    return smoothstep(1.0, 0.0, foam * foamCutoff);
                }




                fixed4 frag(v2f i) : SV_Target
                {
                    // Base color
                    fixed4 col = _WaterColor;


                // Calculate fade factor based on depth
                float fade = DepthFade(i.screenPos, _FadeDistance);

                // Prepare FlowDirection and time Offset
                float2 flowDir = FlowDirection(_FlowDirection);
                float timeOffset = _Time.y * _DistortionSpeed;

                // Add dynamic distortion
                float2 distortionOffset = flowDir * timeOffset;

                // Sample the refraction texture to modulate the noise texture's UVs
                float2 refractionUV = i.uv * _RefractionTiling + distortionOffset;
                float refractionValue = tex2D(_NoiseRefraction, refractionUV).r;


                float2 noiseUV = i.uv * _Tiling + distortionOffset;
                noiseUV += refractionValue * _DistortionStrength; // Add refraction effect to noise UV

                float2 noiseUVextra = i.uv * _Tiling * 1.5 + distortionOffset;
                noiseUVextra += refractionValue * _DistortionStrength;

                // Sample the noise texture
                float noiseValue = tex2D(_NoiseTexture, noiseUV).r;
                float noiseValueExtra = tex2D(_NoiseFoam, noiseUVextra).r;


                // Distortion for fade factor based on noise
                float2 fadeDistortionUV = i.uv * _Tiling + distortionOffset;
                float fadeNoise = tex2D(_NoiseRefraction, fadeDistortionUV).r;
                fade += fadeNoise * 0.5 ; // Add distortion to fade factor

                // Blend with fade color
                col.rgb = lerp(col.rgb, _FadeColor.rgb, saturate(fade));

                // foam edge is for the foam texture and
                //foam edge real is for the line around objects
                float foamEdge = FoamEdge(i.screenPos, _FoamAmount, _FoamCutoff);
                float foamEdgeReal = FoamEdge(i.screenPos, _FoamAmount, 6.8);

                // Sample the noise texture
                float2 noiseUVfoam = i.uv * _FoamScale + distortionOffset;
                noiseUVfoam += refractionValue * _DistortionStrength;

                float noiseValueEdge = tex2D(_NoiseFoam, noiseUVfoam).r;

                float foam = lerp(0.0, foamEdge, noiseValueEdge);

                col.rgb = lerp(col.rgb, _FoamColor.rgb, foam);

                float foamReal = lerp(0.0, 1.0, foamEdgeReal);

                col.rgb = lerp(col.rgb, _FoamColor.rgb, foamReal);


                // Use noise to modulate the water's appearance
                col.rgb += noiseValueExtra * (_NoiseOpacity-(_NoiseOpacity-0.02)) * _FadeColor.rgb;

                col.rgb += noiseValue * _NoiseOpacity * _FoamColor.rgb;
                

                return col;
            }
            ENDCG
        }
        }

            FallBack "Transparent/VertexLit"
}