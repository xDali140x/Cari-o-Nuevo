Shader "Custom/ContornoInteractuableURP"
{
    Properties
    {
        _OutlineColor("Color del contorno", Color) = (1, 0.65, 0, 1)
        _OutlineWidth("Grosor", Range(0.001, 0.03)) = 0.007
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue" = "Geometry-1"
        }

        Pass
        {
            Name "Contorno"

            Cull Front
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _OutlineColor;
                float _OutlineWidth;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;

                float3 posicionWS =
                    TransformObjectToWorld(input.positionOS.xyz);

                float3 normalWS =
                    TransformObjectToWorldNormal(input.normalOS);

                posicionWS += normalize(normalWS) * _OutlineWidth;

                output.positionCS =
                    TransformWorldToHClip(posicionWS);

                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                return _OutlineColor;
            }

            ENDHLSL
        }
    }
}