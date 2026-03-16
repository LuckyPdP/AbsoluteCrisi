Shader "Convertio/URP_Unreal_Surface"
{
    Properties {         [Header(Contorno)]
        _OutlineColor ("Color de Linea", Color) = (0,0,0,1)
        _OutlineThickness ("Grosor de Linea", Range(0.0, 0.1)) = 0.005

        [Header(Ajustes de Sombra Manga)]
        _ShadowCoverage ("Empujar Sombra (Slider)", Range(-1.5, 1.5)) = 0.0
        _CastShadowThreshold ("Sensibilidad Sombra Proyectada", Range(0.0, 1.0)) = 0.1

        [Header(Fresnel y Rim Light)]
        _RimColor ("Color del Borde (Fresnel)", Color) = (1,1,1,1)
        _RimPower ("Grosor del Borde", Range(0.1, 10.0)) = 3.0
        _RimThreshold ("Corte del Borde Manga", Range(0.0, 1.0)) = 0.5

        _Metallic ("Metallic", Float) = 0.0
        _Roughness ("Roughness", Float) = 1
        _RangoToonShader ("RangoToonShader", Float) = 0.491138
        _PosicionColorD ("PosicionColorD", Float) = 0.684
        _PosiconColorC ("PosiconColorC", Float) = 0.36
        _ColorD ("ColorD", Color) = (0.135417, 0.135417, 0.135417, 1)
        _ColorB ("ColorB", Color) = (0.927083, 0.927083, 0.927083, 1)
        _ColorC ("ColorC", Color) = (0.588542, 0.588542, 0.588542, 1)
        _ColorA ("ColorA", Color) = (1, 1, 1, 1)
        _FuerzaNoise ("FuerzaNoise", Float) = 3
        _PasoTiempo ("PasoTiempo", Color) = (1, 1, 0, 1)
        _UVTamaoPaso ("UV Tamaño Paso", Color) = (0.5, 0.5, 0, 1)
        _FuerzaSpecular ("FuerzaSpecular", Float) = 0.84
        _CorteSuaveSpecular ("CorteSuaveSpecular", Float) = 0.0
        _OpacidadSpecular ("OpacidadSpecular", Float) = 0.5
        _ColorSpecular ("ColorSpecular", Color) = (1, 1, 1, 1)
        _TexturaSpecular ("Textura Specular", 2D) = "white" {}
        _TextureDiffusion ("Texture Diffusion", 2D) = "white" {}
        _TextureDiffuse2 ("Texture Diffuse 2", 2D) = "white" {}
        _TextureDiffusin3 ("Texture Diffusin 3", 2D) = "white" {}
        _TextureDiffusion4 ("Texture Diffusion 4", 2D) = "white" {}
 }
    SubShader {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Geometry" }
        LOD 300

        // PASS 1: OUTLINE 
        Pass
        {
            Name "Outline"
            Cull Front 
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
            };

            struct Varyings {
                float4 positionHCS  : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)
        float4 _OutlineColor;
        float _OutlineThickness;
        float _ShadowCoverage;
        float _CastShadowThreshold;
        float4 _RimColor;
        float _RimPower;
        float _RimThreshold;
        float _Metallic;
        float _Roughness;
        float _RangoToonShader;
        float _PosicionColorD;
        float _PosiconColorC;
        float4 _ColorD;
        float4 _ColorB;
        float4 _ColorC;
        float4 _ColorA;
        float _FuerzaNoise;
        float4 _PasoTiempo;
        float4 _UVTamaoPaso;
        float _FuerzaSpecular;
        float _CorteSuaveSpecular;
        float _OpacidadSpecular;
        float4 _ColorSpecular;

            CBUFFER_END

            Varyings vert(Attributes IN) {
                Varyings OUT;
                float3 positionOS = IN.positionOS.xyz + (IN.normalOS * _OutlineThickness);
                OUT.positionHCS = TransformObjectToHClip(positionOS); 
                return OUT;
            }

            half4 frag() : SV_Target {
                return _OutlineColor;
            }
            ENDHLSL
        }

        // PASS 2: LUZ Y SOMBRA MANGA
        Pass {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
                float3 normalOS     : NORMAL;
            };
            struct Varyings {
                float4 positionHCS  : SV_POSITION;
                float3 positionWS   : TEXCOORD0;
                float2 uv           : TEXCOORD1;
                float3 normalWS     : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)
// (CBuffer duplicado)
        float4 _OutlineColor;
        float _OutlineThickness;
        float _ShadowCoverage;
        float _CastShadowThreshold;
        float4 _RimColor;
        float _RimPower;
        float _RimThreshold;
        float _Metallic;
        float _Roughness;
        float _RangoToonShader;
        float _PosicionColorD;
        float _PosiconColorC;
        float4 _ColorD;
        float4 _ColorB;
        float4 _ColorC;
        float4 _ColorA;
        float _FuerzaNoise;
        float4 _PasoTiempo;
        float4 _UVTamaoPaso;
        float _FuerzaSpecular;
        float _CorteSuaveSpecular;
        float _OpacidadSpecular;
        float4 _ColorSpecular;

            CBUFFER_END

        TEXTURE2D(_TexturaSpecular);
        SAMPLER(sampler_TexturaSpecular);
        TEXTURE2D(_TextureDiffusion);
        SAMPLER(sampler_TextureDiffusion);
        TEXTURE2D(_TextureDiffuse2);
        SAMPLER(sampler_TextureDiffuse2);
        TEXTURE2D(_TextureDiffusin3);
        SAMPLER(sampler_TextureDiffusin3);
        TEXTURE2D(_TextureDiffusion4);
        SAMPLER(sampler_TextureDiffusion4);


            Varyings vert(Attributes IN) {
                Varyings OUT;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(OUT.positionWS);
                OUT.uv = IN.uv;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target {
                float4 shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
                Light mainLight = GetMainLight(shadowCoord);
                float3 lightDir = normalize(mainLight.direction);
                float3 viewDir = normalize(GetCameraPositionWS() - IN.positionWS);
                float3 normalWS = normalize(IN.normalWS);

                // --- 1. RUIDO DE NORMALES ---
                float h0 = SAMPLE_TEXTURE2D(_TextureDiffuse2, sampler_TextureDiffuse2, IN.uv).r;
                float hx = SAMPLE_TEXTURE2D(_TextureDiffuse2, sampler_TextureDiffuse2, IN.uv + float2(0.005, 0)).r;
                float hy = SAMPLE_TEXTURE2D(_TextureDiffuse2, sampler_TextureDiffuse2, IN.uv + float2(0, 0.005)).r;
                float3 dNorm = float3(h0 - hx, h0 - hy, 0) * _FuerzaNoise * 15.0; 
                float3 pNormal = normalize(normalWS + dNorm);

                // --- 2. CÁLCULO DE SOMBRAS ---
                float dotNL = dot(pNormal, lightDir);
                dotNL = clamp(dotNL + _ShadowCoverage, -1.0, 1.0);

                float realShadow = step(_CastShadowThreshold, mainLight.shadowAttenuation);
                dotNL = lerp(-1.0, dotNL, realShadow); 

                float subtractVal = dotNL - _RangoToonShader;
                float shadowDepth = -subtractVal;

                // --- 3. MÁSCARAS SCREENTONE ---
                float tex1 = SAMPLE_TEXTURE2D(_TextureDiffusin3, sampler_TextureDiffusin3, IN.uv).r;
                float tex2 = SAMPLE_TEXTURE2D(_TextureDiffusion, sampler_TextureDiffusion, IN.uv).r;

                float noisePower = _FuerzaNoise * 0.1;
                float depthMid  = shadowDepth + (tex2 - 0.5) * noisePower;
                float depthDeep = shadowDepth + (tex1 - 0.5) * noisePower;

                float isMid = step(_PosiconColorC, depthMid);
                float isDeep = step(_PosicionColorD, depthDeep);

                // --- 4. CASCADA DE COLORES (Pura) ---
                float3 diffColor = _ColorA.rgb; 
                diffColor = lerp(diffColor, _ColorC.rgb, isMid); 
                diffColor = lerp(diffColor, _ColorD.rgb, isDeep); 

                // --- 5. ESPECULAR (Afilado y Metálico) ---
                float3 halfVector = normalize(viewDir + lightDir);
                float NdotH = saturate(dot(pNormal, halfVector));
                NdotH *= realShadow; 
                
                // Hacemos el Specular más duro (tipo anime)
                float specCalc = saturate((NdotH - _CorteSuaveSpecular) / max(_OpacidadSpecular, 0.0001) * _FuerzaSpecular);
                float3 SpecularResult = step(0.6, specCalc + (tex1 - 0.5) * noisePower) * _ColorSpecular.rgb;

                // --- 6. FRESNEL / RIM LIGHT (NODO UNREAL DETECTADO) ---
                // Calcula el ángulo entre la cámara y la superficie
                float rimDot = 1.0 - saturate(dot(viewDir, normalWS));
                float fresnel = pow(rimDot, _RimPower);
                // Lo cortamos de forma dura para que parezca dibujado
                float rimIntensity = step(_RimThreshold, fresnel);
                
                // El Fresnel suele brillar más en las zonas oscuras
                float3 rimColorResult = rimIntensity * _RimColor.rgb * isMid; 

                // --- ENSAMBLAJE FINAL ---
                float3 outputColor = diffColor + SpecularResult + rimColorResult;
                return half4(outputColor, 1.0);
            }
            ENDHLSL
        }
        Pass {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }
            ColorMask 0
            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            float3 _LightDirection;
            struct Attributes { float4 positionOS : POSITION; float3 normalOS : NORMAL; };
            struct Varyings { float4 positionCS : SV_POSITION; };
            Varyings ShadowPassVertex(Attributes IN) {
                Varyings OUT;
                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));
                return OUT;
            }
            half4 ShadowPassFragment(Varyings IN) : SV_Target { return 0; }
            ENDHLSL
        }
    }
}