Shader "Custom/SpriteColorReplace"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}

        _TargetColor ("Target Color", Color) = (1,1,1,1)
        _ReplacementColor ("Replacement Color", Color) = (1,0,0,1)
        _Tolerance ("Tolerance", Range(0, 1)) = 0.01

        [MaterialToggle] PixelSnap ("Pixel Snap", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
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
            #pragma multi_compile _ PIXELSNAP_ON
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

            sampler2D _MainTex;
            fixed4 _TargetColor;
            fixed4 _ReplacementColor;
            float _Tolerance;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color;

                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap(OUT.vertex);
                #endif

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, IN.texcoord);

                // Compare RGB distance from target color
                float difference = distance(texColor.rgb, _TargetColor.rgb);

                // 1 when close enough, 0 otherwise
                float replaceAmount = step(difference, _Tolerance);

                fixed4 finalColor = lerp(texColor, _ReplacementColor, replaceAmount);

                // Preserve original alpha
                finalColor.a = texColor.a;

                // Preserve SpriteRenderer color tint
                finalColor *= IN.color;

                // Premultiply alpha for sprite blending
                finalColor.rgb *= finalColor.a;

                return finalColor;
            }
            ENDCG
        }
    }
}