Shader "Texture_Generation/rainbow"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
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
			static const float TAU = 2 * PI;
			static const fixed HS = 43758.5453f;
			static const fixed2 HF = fixed2(20.9898f, 4.1414f);
			static const fixed REP = 40;
			static const fixed OFFS = 0;
			static const fixed DIST = 0.7;
			static const fixed L = 0.18;
			static const fixed REF = 0.5;
			static const fixed W = TAU / REP;
			static const fixed ADD = 0.25;
			static const fixed TPR = 0.125;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed hash(fixed2 uv)
			{
				return frac(sin(dot(uv, HF)) * HS);
			}

			fixed mod(fixed f, fixed m)
			{
				return (f % m + m) % m;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed2 cuv = i.uv * 2 - 1;
				fixed a = atan2(cuv.x, cuv.y);
				fixed2 luv = fixed2((mod(a, W) - W / 2) / (W / 2), (length(cuv) - DIST) / L);

				fixed3 c = max(
					0,
					1 - fixed3(
						length(luv + fixed2(0, REF)),
						length(luv),
						length(luv - fixed2(0, REF))
					)
				) * max(0, TPR * TAU - abs(a)) / (TPR * TAU);

				fixed l = length(c);
				
				if (length(c) > 0)
					c += ADD;

				return fixed4(c.r, c.g, c.b, l);
			}
			ENDCG
		}
	}
}
