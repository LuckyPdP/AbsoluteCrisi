Shader "Hidden/Unreal_GlobalTonemap"
{
    SubShader {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        Cull Off ZWrite Off ZTest Always
        Pass {
            Name "UnrealTonemap"
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            half4 frag (Varyings input) : SV_Target {
                float4 col = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, input.texcoord);
                
                float contrast = 1.15;
                col.rgb = saturate((col.rgb - 0.5) * contrast + 0.5);
                
                return col;
            }
            ENDHLSL
        }
    }
}