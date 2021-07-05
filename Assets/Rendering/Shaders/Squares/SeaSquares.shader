Shader "Skak/Squares/SeaSquares"
{
    Properties
    {
        _MainTex ("Main texture", 2D) = "gray" {}
        _SpotsTex ("Spots texture", 2D) = "gray" {}
        _SquaresInSpot ("Squares in spot", float) = 4
        _SpotColor ("spot color", Color) = (1, 1, 1, 1)
        _BumpCoef ("Bump coef", float) = 1
        _Meta ("Meta texture", 2D) = "gray" {}
        _SeaLevel ("Sea level", float) = 0
        _FogColor ("Fog", Color) = (1, 1, 1, 1)
        _SeaColor ("Sea color", Color) = (1, 1, 1, 1)
        _SeaNoise ("Noise", 2D) = "gray" {}
        _Ripple ("Ripple", 2D) = "gray" {}
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
            #include "../Environment/SeaLevel.hlsl"
            #include "../Utils/UnityNoise.hlsl"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 screenPos : TEXCOORD1;
                float4 world : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _Meta;
            sampler2D _SpotsTex;
            sampler2D _FogNoise;
            float     _SquaresInSpot;
            float4    _SpotColor;
            float     _BumpCoef;
            float     _SeaLevel;
            float4    _FogColor;
            float4    _SeaColor;
            sampler2D _SeaNoise;
            sampler2D _Ripple;

            float4 _Noise_TexelSize;
            float4 _Ripple_TexelSize;
            float4 _SpotsTex_TexelSize;

            // global
            float     _ShadowHeightCoefficient;
            float     _HeightCoefficient;
            sampler2D _PiecesShadowMap;
            float4    _ShadowColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.world = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
                o.screenPos = ComputeScreenPos(o.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 blendMultiply(const float4 base, const float4 blend, const float opacity)
            {
                float4 c = base * blend;
                return lerp(base, c, opacity);
            }

            float4 shadow(float2 screenPos, float4 v)
            {
                float  zOffset = v.z * _ShadowHeightCoefficient;
                float2 offsetScreenPos = screenPos + float2(0, zOffset);
                float  shadow = tex2D(_PiecesShadowMap, offsetScreenPos).x;
                return (1 - shadow) * _ShadowColor;
            }

            float4 spotted(float4 v, float4 worldPos, float4 base)
            {
                const float  ppu = 16;
                const float  shift = ppu * _HeightCoefficient * v.z * _SpotsTex_TexelSize.y;
                const float2 uv = (v - worldPos) / _SquaresInSpot + float2(0, shift) + worldPos;
                const float4 spot = tex2D(_SpotsTex, uv);

                return blendMultiply(base, spot * _SpotColor, spot.a * _BumpCoef);
            }

            float4 posterize(const float In, const float steps)
            {
                return round(In / (1 / steps)) * (1 / steps);
            }

            float4 fog(float height, float2 screenPos, float4 base)
            {
                const float2 uv = float2(_Time.y * 0.03, 0) + screenPos;
                const float  noise = UnityNoise(uv, 80);
                float        fog = -height * 0.4 + 0.9 + noise * 0.4;
                fog = clamp(fog, 0, 1);
                const float opacity = posterize(fog, 6);

                return lerp(base, _FogColor, opacity);
            }

            float4 waves(float height, float2 screenPos, float4 base)
            {
                float wavesLevel = SeaLevel(
                    screenPos,
                    _SeaNoise,
                    _SeaNoise,
                    _Noise_TexelSize,
                    _Ripple,
                    _Ripple_TexelSize,
                    false
                );

                return lerp(_SeaColor, base, height > wavesLevel);
            }


            fixed4 frag(v2f i) : SV_Target
            {
                const float height = i.world.z - (tex2D(_Meta, i.uv).z / -_HeightCoefficient / 16 * 256);

                float4      main = tex2D(_MainTex, i.uv);
                const float alpha = (height > 0) * main.a;

                //main = spotted(i.vertex, i.world, main);
                main = fog(height, i.screenPos, main);
                //main = shadow(i.screenPos, i.vertex) * main;
                main = waves(height, i.screenPos, main);

                return fixed4(main.xyz, alpha);
            }
            ENDCG
        }
    }
}