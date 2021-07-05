Shader "Custom/Sea2"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _ShallowColor ("Shallow Color", Color) = (1,1,1,1)
        _FoamColor ("Foam Color", Color) = (1,1,1,1)
        _BaseAlpha ("Base alpha", Range(0,1)) = 0.0
        _Noise ("Noise", 2D) = "gray" {}
        _Ripple ("Ripple", 2D) = "gray" {}
        _NoiseCoef ("Noise Coef", Range(0, 1)) = 0.4
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

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 screenPos : TEXCOORD1;
                float4 world : TEXCOORD2;
            };

            float4    _Color;
            float4    _FoamColor;
            float4    _ShallowColor;
            float     _BaseAlpha;
            sampler2D _HeightsMap;
            sampler2D _SeaSquaresBase;
            float     _HeightCoefficient;
            float     _FoamAccuracy;
            sampler2D _Noise;
            sampler2D _Ripple;
            float     _NoiseCoef;

            float4 _Noise_TexelSize;
            float4 _Ripple_TexelSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.world = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            float4 posterize(const float In, const float steps)
            {
                return round(In / (1 / steps)) * (1 / steps);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float foamLevel = SeaLevel(
                    i.screenPos,
                    _SeaSquaresBase,
                    _Noise,
                    _Noise_TexelSize,
                    _Ripple,
                    _Ripple_TexelSize,
                    true
                );
                foamLevel = posterize(foamLevel, 7);

                float4 c = (foamLevel < 0.65) * lerp(_Color, _ShallowColor, foamLevel / 0.65);
                c += (foamLevel >= 0.65) * lerp(_ShallowColor, _FoamColor, (foamLevel - 0.65) / 0.35);

                return fixed4(c.xyz, 1);
            }
            ENDCG
        }
    }
}