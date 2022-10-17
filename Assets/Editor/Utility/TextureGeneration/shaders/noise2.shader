Shader "Texture_Generation/Noise2"
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
			//static const fixed HS = 1305.1004f;
			static const fixed HS = 43758.5453f;
			//static const fixed2 HF = fixed2(31.7263f, 101.2510f);
			static const fixed2 HF = fixed2(20.9898f, 4.1414f);
			static const float SCALE = 8;
			static const float RES = 256;
			static const float GROW = 2;
			static const fixed2 OFFS = fixed2(50, 80);

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

			fixed hash(fixed2 uv, fixed res)
			{
				return hash(uv % res);
			}

			fixed smoothHash(fixed2 uv, fixed res)
			{
				fixed2 base = floor(uv);
				fixed2 fac = frac(uv);
				fac = fac * fac * (3 - 2 * fac);

				fixed r = lerp(
					lerp(hash(base, res), hash(base + fixed2(0, 1), res), fac.y),
					lerp(hash(base + fixed2(1, 0), res), hash(base + 1, res), fac.y),
					fac.x
				);

				return r;
			}

			fixed noise(fixed2 uv)
			{
				uv /= SCALE;
				fixed res = RES / SCALE;
				fixed r = smoothHash(uv, res);
				fixed a = 1;

				fixed2 offs[] =
				{
					fixed2(-0.2389f, -0.5991f),
					fixed2(0.1235f, -0.2399f),
					fixed2(-0.8915f, 0.4492f),
					fixed2(0.5915f, -0.4451f),
					fixed2(0.0651f, 0.3390f),
					fixed2(-0.1635f, -0.8526f)
				};

				for (int i = 0; i + 2 < (log(RES) - log(SCALE)) / log(GROW); ++i)
				{
					uv += offs[i];
					res /= GROW;
					uv /= GROW;
					fixed add = (i + 2) * (i + 2);
					a += add;
					r += add * smoothHash(uv, res);
				}

				r /= a;

				return r;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed2 nuv = i.uv * RES + OFFS;
				fixed2 cuv = i.uv * 2 - 1;

				fixed n = pow(noise(nuv), .5f);
				//fixed n = noise(nuv);
				fixed rt = 0.75f;
				fixed p = 0.5f;
				fixed v = n - p;
				fixed c = sign(v) * pow(abs(v), rt) / pow(v < 0.0f ? p : 1.0f - p, rt) + p;

				c *= smoothstep(0, 1, 1 - length(cuv));

				//c = sign(v) * pow(abs(v), rt) / pow(0.5f, rt) + 0.5f;

				return fixed4(1, 1, 1, c);
			}
			ENDCG
		}
	}
}
