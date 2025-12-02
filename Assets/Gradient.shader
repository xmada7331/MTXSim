Shader "Custom/GradientToTransparent"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)
        _MainTex ("Main Texture (optional)", 2D) = "white" {}
        _FadeStart ("Fade Start (0..1)", Range(0,1)) = 0.0
        _FadeEnd ("Fade End (0..1)", Range(0,1)) = 0.5
        _Smooth ("Smoothness", Range(0.001,1)) = 0.08
        _Invert ("Invert gradient (0 = bottom->top, 1 = top->bottom)", Float) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 100
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _FadeStart;
            float _FadeEnd;
            float _Smooth;
            float _Invert;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // base color (texture * tint) - texture is optional
                fixed4 baseCol = tex2D(_MainTex, i.uv) * _Color;

                // compute gradient factor based on UV.y (vertical gradient)
                float y = i.uv.y;
                if (_Invert > 0.5) y = 1.0 - y;

                // normalized t between FadeStart..FadeEnd
                float denom = max(_FadeEnd - _FadeStart, 0.0001);
                float t = saturate((y - _FadeStart) / denom);

                // smoothstep for nice fade
                float alphaFactor = 1.0 - smoothstep(0.0, 1.0, t * (1.0 / max(_Smooth, 0.0001)));

                // final color with gradient alpha
                baseCol.a *= alphaFactor;
                return baseCol;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}