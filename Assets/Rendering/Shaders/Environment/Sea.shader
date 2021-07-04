Shader "Custom/Sea2"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
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
            float     _BaseAlpha;
            sampler2D _HeightsMap;
            sampler2D _SeaSquaresBase;
            float     _HeightCoefficient;
            float     _FoamAccuracy;
            sampler2D _Noise;
            sampler2D _Ripple;
            float _NoiseCoef;

            float4 _Noise_TexelSize;
            float4 _Ripple_TexelSize;

            static const float PI = 3.14159265f;

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

            float2 noisePos()
            {
                float t = _Time.x * 2;
                return float2(_CosTime.x * 2 + t, _SinTime.x * 2 + t);
            }

            float2 applyXStretch(float2 p, float4 texelSize)
            {
                return float2(p.x * texelSize.x * texelSize.w, p.y);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                const float2 pp = round(i.screenPos * _ScreenParams.xy / 2);
                const float2 p = pp / _ScreenParams.xy * 2;

                float foamLevel = tex2D(_SeaSquaresBase, p);

                // Noise

                foamLevel += 
                    (
                        tex2D(
                            _Noise, 
                            applyXStretch(p, _Noise_TexelSize) + _Time.x * float2(0.4, -0.1)
                        ).x
                    ) * 0.09;

                foamLevel += 
                    (
                        tex2D(
                            _Noise, 
                            applyXStretch(p, _Noise_TexelSize) + _Time.x * float2(-0.2, 0.7)
                        ).x
                    ) * 0.09;

                // Ripple

                foamLevel += 
                    (
                        tex2D(
                            _Ripple, 
                            applyXStretch(p, _Ripple_TexelSize) * 2.5 + _Time.x * float2(-0.7, -0.1)
                        ).x
                    ) * 0.1;

                foamLevel += 
                    (
                        tex2D(
                            _Ripple, 
                            applyXStretch(p, _Ripple_TexelSize) * 2.5 + _Time.x * float2(0.4, 0.2)
                        ).x
                    ) * 0.1;

                // Waves

                float wavePhase = sin(i.screenPos.y + i.screenPos.x * 0.7) * 60;

                foamLevel += abs(sin(_Time.x * 30 + wavePhase * 0.1) * 0.4 + 0.6 - foamLevel) < 0.1 ? 0.3 : 0;
                foamLevel += abs(sin(_Time.x * 30 + PI + wavePhase * 0.15) * 0.4 + 0.6 - foamLevel) < 0.05 ? 0.3 : 0;

                // Dither
                foamLevel += ((pp.x / 2 + pp.y) % 2 * 2 - 1) * 0.006;

                // Posterize
                foamLevel = posterize(clamp(foamLevel, 0, 1), 7);

                float4 c = lerp(_Color, _FoamColor, foamLevel);

                return fixed4(c.xyz, 1);
            }
            ENDCG
        }
    }
}