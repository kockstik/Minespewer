Shader "Custom/MRStencil"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white"

        _NoiseTex("Noise", 2D) = "white" {}
        _NoiseScale("Noise Scale", Float) = 4.0
        _NoiseStrength("Noise Strength", Float) = 0.3
        _NoiseSpeed("Noise Speed", Float) = 0.5

        _HideCenter("Hide Center", Vector) = (0, 0, 0, 0)
        _HideRadius("Radius", Float) = 3.0
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" "Queue" = "Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float2 noiseUV : TEXCOORD2;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 noiseUV : TEXCOORD2;
                float3 worldPos : TEXCOORD1;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
                float4 _NoiseTex_ST;
                float4 _HideCenter;
                float _HideRadius;
                float _NoiseScale;
                float _NoiseStrength;
                float _NoiseSpeed;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.noiseUV = TRANSFORM_TEX(IN.noiseUV, _NoiseTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 worldXZ = IN.worldPos.xz;
                float2 hideXZ = _HideCenter.xz;

                float2 noiseUV = IN.noiseUV / _NoiseScale; 
                noiseUV += _Time.y * _NoiseSpeed;
                float noise = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, noiseUV).r;
                noise *= _NoiseStrength;

                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                float dist = distance(worldXZ, hideXZ) + noise - _NoiseStrength * 0.8;

                float hideFactor = smoothstep(_HideRadius, _HideRadius - 1.0, dist);

                color.a *= 1 - hideFactor;

                return color;
            }
            ENDHLSL
        }
    }
}
