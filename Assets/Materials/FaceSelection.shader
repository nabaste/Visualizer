Shader "Visualizer/FaceSelection"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [HDR] _SlopeColorFlat("Flat Slope Color", Color) = (0, 1, 0, 1)
        [HDR] _SlopeColorSteep("Steep Slope Color", Color) = (1, 0, 0, 1)
        [HDR] _HighlightColor("Highlight Color", Color) = (0, 0, 1, 1)
        
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Blend One Zero
            ZWrite On
            Cull Back

            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float4 color : COLOR;
            };

            Varyings vert(Attributes input) {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.color = input.color;
                return output;
            }

            float4 _SlopeColorFlat;
            float4 _SlopeColorSteep;
            float4 _HighlightColor;
            
            half4 frag(Varyings input) : SV_Target {
                float slopeValue = input.color.r; // slope data
                float highlight = input.color.a;  // selection

                float3 baseColor = lerp(_SlopeColorFlat.rgb, _SlopeColorSteep.rgb, slopeValue);
                float3 finalColor = lerp(baseColor, _HighlightColor.rgb, highlight); // blend if selected
                return half4(finalColor, 1.0);
            }

            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ColorMask 0
            Cull Back

            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }
    }

    FallBack "Hidden/InternalErrorShader"
}

