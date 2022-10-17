Shader "Texture_Generation/attemptedBangBubble"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            static const float PI = 3.14159265f;
            static const float REP = 5.0f;
            static const float OFFS = 0.0f;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed2 uv = 2.0f * i.uv - 1.0f;
                fixed base = 1.0f - length(uv);
                fixed a = abs(atan2(uv.x, uv.y) / 2 / PI + OFFS);
                fixed rep = a % (1.0f / REP) * REP;
                fixed sym = abs(2.0f * rep - 1.0f);
                fixed remap = 0.5f * (1.0f + sym);
                fixed sub = remap * length(uv);
                fixed dif = base - sub;

                fixed c = sym;
                //fixed c = smoothstep(max(0, dif), 0.0f, 0.01f);
                
                return fixed4(c, c, c, 1);
            }
            ENDCG
        }
    }
}
