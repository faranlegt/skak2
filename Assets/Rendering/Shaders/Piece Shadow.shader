Shader "Custom/Piece Shadow"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            fixed4 _Color;

            v2f vert(appdata_t IN)
            {
                // actually do nothinh
                // // matrix translates sprite on 1 unit down, bcs sprite will be rotated around origin which is (0.5, 0.5)
                const float4x4 translateMatrix = float4x4(1, 0, 0, 0,
                                                          0, 1, 0, 0,
                                                          0, 0, 1, 0,
                                                          0, 0, 0, 1);
                // rotates sprite on Pi around Z
                const float4x4 rotateZMatrix = float4x4(-1, 0, 0, 0,
                                                        0, -1, 0, 0,
                                                        0, 0, 1, 0,
                                                        0, 0, 0, 1);

                // flip by x 
                const float4x4 scaleMatrix = float4x4(-1, 0, 0, 0,
                                                      0, 1, 0, 0,
                                                      0, 0, 1, 0,
                                                      0, 0, 0, 1);
                const float4 localVertexPos = IN.vertex;

                const float4 localTranslated = mul(translateMatrix, localVertexPos);
                const float4 localScaledTranslated = mul(localTranslated, scaleMatrix);
                const float4 localScaledTranslatedRotated = mul(localScaledTranslated, rotateZMatrix);

                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(localScaledTranslatedRotated);
                //OUT.vertex.y += UnityObjectToClipPos(IN.vertex).z;
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;

                return OUT;
            }

            sampler2D _MainTex;
            sampler2D _AlphaTex;
            float _AlphaSplitEnabled;

            fixed4 SampleSpriteTexture(float2 uv)
            {
                fixed4 color = tex2D(_MainTex, uv);

                #if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
				{
				    color.a = tex2D (_AlphaTex, uv).r;
				}
                #endif

                return color;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
                c.rgb = 0;
                return c;
            }
            ENDCG
        }

    }
    Fallback "Sprites/Default"
}