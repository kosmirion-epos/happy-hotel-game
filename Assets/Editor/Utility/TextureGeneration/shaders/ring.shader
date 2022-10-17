Shader "Texture_Generation/Ring"
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

			fixed extremify(fixed f, fixed o, fixed p)
			{
				fixed bla = f > o ? 1 - o : o;
				return pow(abs(f - o), 1 / p) * sign(f - o) / pow(bla, 1 / p) * 0.5f + 0.5f;
			}

			fixed3 makeCurves(fixed uv, fixed3 channels, fixed res = 256)
			{
				return fixed3(
					max(0, 1 - res * distance(uv, fixed2(uv.x, channels.x))),
					max(0, 1 - res * distance(uv, fixed2(uv.x, channels.y))),
					max(0, 1 - res * distance(uv, fixed2(uv.x, channels.z)))
				);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed2 uv = i.uv * 2 - 1;

				fixed t = i.uv.x;
				fixed3 com = fixed3(
					extremify(t, 0.5f, 4),
					t,
					0
				);

				fixed3 c = extremify(max(0, 1 - 4 * abs(0.75f - length(uv))) * 1.05, 0.95f, 4);

				return fixed4(c.x, c.y, c.z, 1);
			}
			ENDCG
		}
	}
}
