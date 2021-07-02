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
            float     _HeightCoefficient;
            float     _FoamAccuracy;
            sampler2D _Noise;
            sampler2D _Ripple;
            float _NoiseCoef;

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

            float foam(float2 v, float h)
            {
                const fixed3 original = tex2D(_HeightsMap, v).b;
                const float  worldHeight = (tex2D(_HeightsMap, v).b - 0.5) * -16 / _HeightCoefficient;

                return original * (worldHeight > h);
                
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
                const float  pi2 = 6.28318530718;
                const float  directions = 32.0;
                const float  quality = 8.0;
                const float  size = 20.0;
                const float2 radius = size / _ScreenParams.xy;

                const float2 pp = round(i.screenPos * _ScreenParams.xy / 2);
                const float2 p = pp / _ScreenParams.xy * 2;
                const float  h = i.world.z;

                float foamLevel = foam(p, h);

                for(float d = 0; d < pi2 / 2; d += pi2 / 2 / directions)
                    for(float j = 1.0 / quality; j <= 1.0; j += 1.0 / quality)
                    {
                        foamLevel += foam(p + float2(cos(d), sin(d)) * radius * j, h);
                    }

                foamLevel /= quality * directions / 3;
                
                // Noise

                foamLevel += 
                    (
                        tex2D(
                            _Noise, 
                            applyXStretch(p, _Noise_TexelSize) + float2(_Time.x * 0.4, 0)
                        ).x
                    ) * 0.08;

                foamLevel += 
                    (
                        tex2D(
                            _Noise, 
                            applyXStretch(p, _Noise_TexelSize) + float2(0, _Time.x * 0.7)
                        ).x
                    ) * 0.08;

                // Ripple

                foamLevel += 
                    (
                        tex2D(
                            _Ripple, 
                            applyXStretch(p, _Ripple_TexelSize) * 2.5 + float2(-_Time.x * 0.7, 0)
                        ).x
                    ) * 0.1;

                foamLevel += 
                    (
                        tex2D(
                            _Ripple, 
                            applyXStretch(p, _Ripple_TexelSize) * 2.5 + float2(_Time.x * 0.4, _Time.x * 0.1)
                        ).x
                    ) * 0.1;

                // Dither
                foamLevel += ((pp.x / 2 + pp.y) % 2 * 2 - 1) * 0.006;

                // Posterize
                foamLevel = posterize(clamp(foamLevel, 0, 1), 6);

                float4 c = lerp(_Color, _FoamColor, foamLevel);

                return fixed4(c.xyz, 1);
            }
            ENDCG
        }
    }
}