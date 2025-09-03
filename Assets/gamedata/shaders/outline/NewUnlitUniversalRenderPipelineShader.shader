Shader "Custom/URPOutlineUnlitImproved"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TintColor ("Tint Color", Color) = (1, 1, 1, 1)
        _Brightness ("Brightness", Range(0, 2)) = 1.0
        _OutlineColor ("Outline Color", Color) = (1, 0, 0, 1)
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.02
        _OutlineTransparency ("Outline Transparency", Range(0, 1)) = 1.0
        _OutlineScale ("Outline Scale Factor", Range(0.1, 2.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        LOD 200

        Pass
        {
            Name "Outline"
            Cull Front
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            float _OutlineWidth;
            float4 _OutlineColor;
            float _OutlineTransparency;
            float _OutlineScale;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                float3 offset = normalWS * _OutlineWidth * _OutlineScale;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz + offset);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return float4(_OutlineColor.rgb, _OutlineColor.a * _OutlineTransparency);
            }
            ENDHLSL
        }

        Pass
        {
            Name "Unlit"
            Tags { "LightMode" = "UniversalForward" }
            Cull Back
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _TintColor;
            float _Brightness;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                texColor.rgb = texColor.rgb * _TintColor.rgb * _Brightness;
                return texColor;
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Unlit"
}