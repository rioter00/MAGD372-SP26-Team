Shader "Custom/CloseFogFullScreen_FIXED"
{
    Properties
    {
        _FogColor ("Fog Color", Color) = (0.7, 0.7, 0.7, 1)
        _FogStart ("Fog Start", Float) = 0
        _FogEnd ("Fog End", Float) = 5
        _FogStrength ("Fog Strength", Range(0,1)) = 1
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            Name "CloseFog"
            ZWrite Off
            ZTest Always
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct Attributes
            {
                uint vertexID : SV_VertexID;
            };

            struct Varyings
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            float4 _FogColor;
            float _FogStart;
            float _FogEnd;
            float _FogStrength;

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.pos = GetFullScreenTriangleVertexPosition(v.vertexID);
                o.uv = GetFullScreenTriangleTexCoord(v.vertexID);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, i.uv);

                float rawDepth = SampleSceneDepth(i.uv);

                if (rawDepth >= 0.9999)
                    return col;

                float linearDepth = LinearEyeDepth(rawDepth, _ZBufferParams);

                float fogFactor = 1.0 - saturate((linearDepth - _FogStart) / max(_FogEnd - _FogStart, 0.001));
                fogFactor *= _FogStrength;

                col.rgb = lerp(col.rgb, _FogColor.rgb, fogFactor);
                return col;
            }

            ENDHLSL
        }
    }
}