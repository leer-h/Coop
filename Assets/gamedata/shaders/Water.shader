Shader "Custom/URPWaterShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0, 0.5, 0.8, 0.5)
        _WaveSpeed ("Wave Speed", Float) = 0.5
        _WaveScale ("Wave Scale", Float) = 0.1
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _DepthFactor ("Depth Factor", Float) = 1.0
        _FoamColor ("Foam Color", Color) = (1, 1, 1, 1)
        _FoamThreshold ("Foam Threshold", Float) = 0.2
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
                float fogFactor : TEXCOORD3;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _WaveSpeed;
                float _WaveScale;
                sampler2D _NoiseTex;
                float4 _NoiseTex_ST;
                float _DepthFactor;
                float4 _FoamColor;
                float _FoamThreshold;
            CBUFFER_END

            TEXTURE2D(_CameraDepthTexture);
            SAMPLER(sampler_CameraDepthTexture);

            Varyings vert(Attributes input)
            {
                Varyings output;
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float noise = tex2Dlod(_NoiseTex, float4(input.uv * _NoiseTex_ST.xy + _Time.y * _WaveSpeed, 0, 0)).r;
                positionWS.y += sin(_Time.y + noise * 6.28) * _WaveScale;

                output.positionCS = TransformWorldToHClip(positionWS);
                output.uv = input.uv;
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.screenPos = ComputeScreenPos(output.positionCS);
                output.fogFactor = ComputeFogFactor(output.positionCS.z);
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                float2 screenUV = input.screenPos.xy / input.screenPos.w;
                float sceneDepth = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, screenUV).r;
                float waterDepth = LinearEyeDepth(sceneDepth, _ZBufferParams) - LinearEyeDepth(input.positionCS.z, _ZBufferParams);
                float depthEffect = saturate(waterDepth * _DepthFactor);

                float noise = tex2D(_NoiseTex, input.uv * _NoiseTex_ST.xy + _Time.y * _WaveSpeed).r;
                float foam = step(_FoamThreshold, 1.0 - depthEffect) * noise;

                float4 color = lerp(_BaseColor, _FoamColor, foam);
                color.a = _BaseColor.a * depthEffect;

                color.rgb = MixFog(color.rgb, input.fogFactor);
                return color;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}