Shader "Custom/CRT_Monitor"
{
    Properties
    {
        [Header(Scanlines)]
        _ScanlineIntensity("Scanline Intensity", Range(0.0, 1.0)) = 0.10
        _ScanlineCount("Scanline Count (0 = Auto)", Float) = 0.0
        _ScanlineSpeed("Scanline Drift Speed", Float) = 0.05
        _ScanlineSharpness("Scanline Sharpness", Range(0.5, 3.0)) = 1.0

        [Header(Phosphor Subpixel Mask)]
        _MaskIntensity("RGB Mask Intensity", Range(0.0, 0.5)) = 0.06
        _MaskScale("RGB Mask Scale (0 = Auto)", Float) = 0.0

        [Header(Screen Curvature and Bezel)]
        _Curvature("Screen Curvature", Range(0.0, 0.08)) = 0.02
        _CornerRounding("Corner Rounding", Range(0.0, 0.1)) = 0.025
        _CornerSmoothness("Corner Smoothness", Range(0.001, 0.05)) = 0.01
        _BezelColor("Bezel / Border Color", Color) = (0.02, 0.02, 0.02, 1.0)

        [Header(Vignette)]
        _VignetteIntensity("Vignette Intensity", Range(0.0, 1.0)) = 0.20
        _VignetteSmoothness("Vignette Smoothness", Range(0.1, 2.0)) = 0.8

        [Header(Chromatic Aberration)]
        _ChromaticAberration("Chromatic Aberration", Range(0.0, 0.008)) = 0.0012

        [Header(Phosphor Glow and Bleed)]
        _GlowIntensity("Phosphor Glow Intensity", Range(0.0, 0.5)) = 0.04
        _GlowOffset("Phosphor Glow Spread", Range(0.0001, 0.005)) = 0.001

        [Header(Analog Noise and Flicker)]
        _NoiseIntensity("Analog Noise Intensity", Range(0.0, 0.05)) = 0.01
        _FlickerIntensity("Cathode Flicker Intensity", Range(0.0, 0.05)) = 0.006

        [Header(Color Adjustments)]
        _Brightness("Brightness", Range(0.5, 1.5)) = 1.02
        _Contrast("Contrast", Range(0.5, 1.5)) = 1.03
        _ScreenTint("Screen Tint", Color) = (1.0, 1.0, 1.0, 1.0)
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque" 
            "RenderPipeline" = "UniversalPipeline"
        }
        LOD 100
        ZTest Always 
        ZWrite Off 
        Cull Off

        Pass
        {
            Name "CRT_Blit_Pass"

            HLSLPROGRAM
            #pragma target 3.5
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float _ScanlineIntensity;
                float _ScanlineCount;
                float _ScanlineSpeed;
                float _ScanlineSharpness;

                float _MaskIntensity;
                float _MaskScale;

                float _Curvature;
                float _CornerRounding;
                float _CornerSmoothness;
                float4 _BezelColor;

                float _VignetteIntensity;
                float _VignetteSmoothness;

                float _ChromaticAberration;

                float _GlowIntensity;
                float _GlowOffset;

                float _NoiseIntensity;
                float _FlickerIntensity;

                float _Brightness;
                float _Contrast;
                float4 _ScreenTint;
            CBUFFER_END

            // Barrel distortion on normalized [0, 1] screen UV
            float2 DistortUV(float2 uv, float curvature)
            {
                if (curvature <= 0.0001)
                    return uv;

                float2 cc = uv * 2.0 - 1.0;
                float aspect = _ScreenParams.x / max(_ScreenParams.y, 1.0);
                cc.x *= aspect;

                float dist = dot(cc, cc);
                cc = cc * (1.0 + dist * curvature * 0.15);

                cc.x /= aspect;
                return cc * 0.5 + 0.5;
            }

            // Mask for rounded CRT bezel edges
            float GetBezelMask(float2 uv, float cornerRounding, float cornerSmoothness)
            {
                if (uv.x < 0.0 || uv.x > 1.0 || uv.y < 0.0 || uv.y > 1.0)
                    return 0.0;

                if (cornerRounding <= 0.0001)
                    return 1.0;

                float2 d = abs(uv - 0.5) * 2.0;
                float cornerRadius = cornerRounding * 2.0;
                float2 cornerDist = max(d - (1.0 - cornerRadius), 0.0);
                float dist = length(cornerDist);

                return 1.0 - smoothstep(cornerRadius - cornerSmoothness, cornerRadius, dist);
            }

            // Chromatic aberration sample
            half4 SampleWithAberration(float2 uv, float aberration)
            {
                if (aberration <= 0.00001)
                {
                    return SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);
                }

                float2 dir = uv - 0.5;
                float dist = dot(dir, dir);
                float2 offset = dir * (dist * aberration * 4.0);

                half r = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv - offset).r;
                half g = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv).g;
                half b = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv + offset).b;
                half a = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv).a;

                return half4(r, g, b, a);
            }

            // CRT phosphor glow (slight bleed from adjacent pixels)
            half4 SampleWithGlow(float2 uv, float aberration, float glowIntensity, float glowOffset)
            {
                half4 col = SampleWithAberration(uv, aberration);
                if (glowIntensity <= 0.001)
                    return col;

                float2 offset = _BlitTexture_TexelSize.xy * (glowOffset * 1000.0);
                half4 blur = SampleWithAberration(uv + float2(offset.x, 0.0), aberration)
                           + SampleWithAberration(uv - float2(offset.x, 0.0), aberration)
                           + SampleWithAberration(uv + float2(0.0, offset.y), aberration)
                           + SampleWithAberration(uv - float2(0.0, offset.y), aberration);
                blur *= 0.25;

                return lerp(col, blur, glowIntensity);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                // Handle dynamic scaling if active
                float2 normUV = (input.texcoord - _BlitScaleBias.zw) / max(_BlitScaleBias.xy, 0.0001);

                // 1. CRT Curvature
                float2 distortedNormUV = DistortUV(normUV, _Curvature);
                float bezelMask = GetBezelMask(distortedNormUV, _CornerRounding, _CornerSmoothness);

                float2 sampleUV = distortedNormUV * _BlitScaleBias.xy + _BlitScaleBias.zw;

                // 2. Base Sample with Chromatic Aberration & Glow
                half4 color = SampleWithGlow(sampleUV, _ChromaticAberration, _GlowIntensity, _GlowOffset);

                // 3. Subtle Scanlines (Sine modulation, smooth & anti-aliased)
                if (_ScanlineIntensity > 0.001)
                {
                    float lineCount = _ScanlineCount > 1.0 ? _ScanlineCount : (_ScreenParams.y * 0.5);
                    float scanlinePhase = (distortedNormUV.y + _Time.y * _ScanlineSpeed * 0.05) * lineCount * 6.2831853;
                    float scanline = sin(scanlinePhase) * 0.5 + 0.5;
                    scanline = pow(scanline, _ScanlineSharpness);

                    float scanlineFactor = lerp(1.0 - _ScanlineIntensity, 1.0, scanline);
                    color.rgb *= scanlineFactor;
                }

                // 4. Subtle RGB Phosphor Mask (Aperture Grille Triads)
                if (_MaskIntensity > 0.001)
                {
                    float maskCount = _MaskScale > 1.0 ? _MaskScale : _ScreenParams.x;
                    float maskPhase = distortedNormUV.x * maskCount * 6.2831853;

                    half3 mask;
                    mask.r = sin(maskPhase) * 0.5 + 0.5;
                    mask.g = sin(maskPhase + 2.0943951) * 0.5 + 0.5; // +120 deg
                    mask.b = sin(maskPhase + 4.1887902) * 0.5 + 0.5; // +240 deg

                    mask = lerp(half3(1.0, 1.0, 1.0), mask * 1.15, _MaskIntensity);
                    color.rgb *= mask;
                }

                // 5. Subtle Vignette
                if (_VignetteIntensity > 0.001)
                {
                    float2 v = distortedNormUV * (1.0 - distortedNormUV.yx);
                    float vig = v.x * v.y * 15.0;
                    vig = clamp(pow(vig, _VignetteSmoothness), 0.0, 1.0);
                    color.rgb *= lerp(1.0 - _VignetteIntensity, 1.0, vig);
                }

                // 6. Subtle Analog Noise
                if (_NoiseIntensity > 0.001)
                {
                    float2 noiseUV = distortedNormUV + frac(_Time.y * 13.37);
                    float noise = frac(sin(dot(noiseUV, float2(12.9898, 78.233))) * 43758.5453);
                    color.rgb += (noise - 0.5) * _NoiseIntensity;
                }

                // 7. Subtle Cathode Flicker
                if (_FlickerIntensity > 0.001)
                {
                    float flicker = frac(sin(_Time.y * 59.94) * 43758.5453);
                    color.rgb *= 1.0 + (flicker - 0.5) * _FlickerIntensity;
                }

                // 8. Contrast and Brightness
                color.rgb = (color.rgb - 0.5) * _Contrast + 0.5;
                color.rgb *= _Brightness;
                color.rgb *= _ScreenTint.rgb;

                // 9. Apply Bezel Mask
                color.rgb = lerp(_BezelColor.rgb, color.rgb, bezelMask);

                return color;
            }
            ENDHLSL
        }
    }
    FallBack Off
}
