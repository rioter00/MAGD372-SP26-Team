Shader "Custom/URPDepthFogNoSkybox"
{
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" }

        Pass
        {
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
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D_X(_BlitTexture);
            SAMPLER(sampler_BlitTexture);

            float4 _FogColor;
            float _FogStart;
            float _FogEnd;
            float _FogStrength;

            float3 _HeadlightPosA;
            float3 _HeadlightDirA;
            float3 _HeadlightPosB;
            float3 _HeadlightDirB;
            float _BeamLength;
            float _FogClearStrength;
            float _BeamRadius;

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = GetFullScreenTriangleVertexPosition(v.vertexID);
                o.uv = GetFullScreenTriangleTexCoord(v.vertexID);
                return o;
            }

            float HeadlightMask(float3 worldPos, float3 lightPos, float3 lightDir)
            {
                float3 toPoint = worldPos - lightPos;
                float forwardDist = dot(toPoint, normalize(lightDir));

                if (forwardDist <= 0.0 || forwardDist >= _BeamLength)
                    return 0.0;

                float3 closestPointOnBeam = lightPos + normalize(lightDir) * forwardDist;
                float distFromBeamCenter = distance(worldPos, closestPointOnBeam);

                float radiusAtPoint = lerp(_BeamRadius, _BeamRadius * 4.0, forwardDist / max(_BeamLength, 0.001));

                float radialMask = 1.0 - saturate(distFromBeamCenter / max(radiusAtPoint, 0.001));
                float lengthMask = 1.0 - saturate(forwardDist / max(_BeamLength, 0.001));

                return radialMask * lengthMask;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_BlitTexture, i.uv);

                float rawDepth = SampleSceneDepth(i.uv);

                // skybox/background
                if (rawDepth >= 0.9999)
                    return col;

                float linearDepth = LinearEyeDepth(rawDepth, _ZBufferParams);

                float3 worldPos = ComputeWorldSpacePosition(i.uv, rawDepth, UNITY_MATRIX_I_VP);

                float fogFactor = saturate((linearDepth - _FogStart) / max(_FogEnd - _FogStart, 0.001));
                fogFactor *= _FogStrength;

                float maskA = HeadlightMask(worldPos, _HeadlightPosA, _HeadlightDirA);
                float maskB = HeadlightMask(worldPos, _HeadlightPosB, _HeadlightDirB);

                float beamMask = saturate(max(maskA, maskB) * _FogClearStrength);

                fogFactor *= (1.0 - beamMask);

                col.rgb = lerp(col.rgb, _FogColor.rgb, fogFactor);

                return col;
            }

            ENDHLSL
        }
    }
}