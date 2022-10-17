Shader "Texture_Generation/Star"
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
			static const fixed REP = 4;
			static const fixed OFFS = 0;
			static const fixed MIN_DIST = 1.5;
			static const fixed MAX_DIST = 0.5;

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

			fixed remap(fixed v, fixed oldLo, fixed oldHi, fixed newLo, fixed newHi)
			{
				return (v - oldLo) / (oldHi - oldLo) * (newHi - newLo) + newLo;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed2 cuv = i.uv * 2 - 1;
				fixed a = atan2(cuv.x, cuv.y);
				fixed2 luv = fixed2(1, 1);

				fixed c = 0;

				for (int i = 0; i < REP; ++i)
				{
					fixed curA = i * TAU / REP;
					fixed2 dir = fixed2(sin(curA), cos(curA));
					fixed s = sqrt(remap(length(cuv), 0, 1, MIN_DIST, MAX_DIST));
					c += pow(max(0, s - abs(sin(abs(curA) - a)) * length(cuv)) / s, 10);
				}

				c *= max(0, 1 - length(cuv));

				return fixed4(c, c, c, 1);
			}
			ENDCG
		}
	}
}
