Shader "Texture_Generation/beam"
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
                //fixed4 col = tex2D(_MainTex, i.uv);
                fixed2 uv = i.uv;
                
                float s = 0.5f + 0.18f * sin(uv.x * 2 * PI) + 0.01f * sin((uv.x + 0.5f) * 20 * PI) + 0.025f * sin((uv.x + 0.8f) * 10 * PI);
                float v = max(0, 1 - abs(uv.y - s) * (3 + 0.75f * sin((uv.x + 0.67f) * 12 * PI)));
                v *= v * v * v;
                
                return fixed4(v, v, v, 1);
            }
            ENDCG
        }
    }
}
