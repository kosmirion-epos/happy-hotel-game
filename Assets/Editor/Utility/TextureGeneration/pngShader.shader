Shader "Util/png"
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
			static const fixed2 HF = fixed2(18.9898f, 4.1414f);
			static const float SCALE = 1;
			static const float RES = 256;
			static const float GROW = 2;
			static const fixed2 OFFS = fixed2(5, 40);

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed hash(fixed2 uv, fixed hs = HS, fixed2 hf = HF)
			{
				return frac(sin(dot(uv, HF)) * HS);
			}

			fixed tiledHash(fixed2 uv, fixed res, fixed hs = HS, fixed2 hf = HF)
			{
				return hash(uv % res, hs, hf);
			}

			fixed smoothHash(fixed2 uv, fixed res, fixed hs = HS, fixed2 hf = HF)
			{
				fixed2 base = floor(uv);
				fixed2 fac = frac(uv);
				fac = fac * fac * (3 - 2 * fac);

				fixed r = lerp(
					lerp(tiledHash(base, res, hs, hf), tiledHash(base + fixed2(0, 1), res, hs, hf), fac.y),
					lerp(tiledHash(base + fixed2(1, 0), res, hs, hf), tiledHash(base + 1, res, hs, hf), fac.y),
					fac.x
				);

				return r;
			}

			fixed noise(fixed2 uv, fixed res = RES / SCALE, fixed hs = HS, fixed2 hf = HF)
			{
				uv /= SCALE;
				fixed r = smoothHash(uv, res, hs, hf);
				fixed a = 1;

				fixed2 offs[] =
				{
					fixed2(0.3763613371127079, -0.3829215883859989),
					fixed2(0.025031439727789095, 0.5243832683238745),
					fixed2(0.5512264613455053, 0.7122188367772075),
					fixed2(-0.5187709421889914, 0.13053085828210098),
					fixed2(-0.29681928770661625, -0.2131392398749279),
					fixed2(-0.757299463785422, 0.9911216743536806),
					fixed2(-0.8268442197162922, -0.21165556700319255),
					fixed2(-0.28882642286874693, 0.6281112522045809),
					fixed2(0.3074421945301189, 0.9831877327656537),
					fixed2(-0.38780845821067467, 0.11020006498254742),
					fixed2(0.8298332675418201, 0.8552059264472802),
					fixed2(0.795160646385356, -0.8334991079597787),
					fixed2(0.31978167544968894, 0.3109613601584005),
					fixed2(-0.401978341132583, -0.8918462976604986),
					fixed2(-0.21118392271143183, 0.6739848599207188),
					fixed2(0.10834224511655832, -0.2847908852501413),
					fixed2(0.49164242461474306, 0.893431876803525),
					fixed2(-0.9581243314493311, -0.696056498925377),
					fixed2(0.08513041594727189, 0.6764413415881247),
					fixed2(-0.4662446508943774, -0.38839632276084135),
				};

				for (int i = 0; i + 2 < (log(RES) - log(SCALE)) / log(GROW); ++i)
				{
					uv += offs[i];
					res /= GROW;
					uv /= GROW;
					fixed add = (i + 2) * (i + 2);
					a += add;
					r += add * smoothHash(uv, res, hs, hf);
				}

				r /= a;

				return r;
			}

			fixed extremify(fixed f, fixed o, fixed p)
			{
				fixed bla = f > o ? 1 - o : o;
				return pow(abs(f - o) / bla, 1 / p) * sign(f - o) * bla + o;
			}

			fixed remap(fixed v, fixed oldLo, fixed oldHi, fixed newLo, fixed newHi, bool clampResult)
			{
				fixed result = (v - oldLo) / (oldHi - oldLo) * (newHi - newLo) + newLo;
				return clampResult ? clamp(result, newLo, newHi) : result;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed2 nuv = i.uv * RES + OFFS;
				fixed2 cuv = i.uv * 2 - 1;
				fixed2 hcuv1 = i.uv * fixed2(2, 1) + fixed2(-1, 0);
				fixed2 hcuv2 = i.uv * fixed2(2, 1) + fixed2(-1, -1);

				fixed n = noise(nuv);
				fixed l1 = 5;
				fixed l2 = 1;
				fixed sm = 0.05;
				fixed c = smoothstep(0.025, 0, abs(0.5 - n))
					* smoothstep(0, sm, max(0, l1 - length(hcuv1)))
					* smoothstep(0, sm, max(0, l2 - length(hcuv2)))
					* smoothstep(1, 1 - sm, abs(cuv.x))
					* smoothstep(1, 1 - sm, abs(cuv.y));

				//fixed cut = 0.75;
				//fixed n = extremify(noise(nuv), cut, 4);
				//fixed c = remap(n, 0.2, 1, 0, 1, true) * smoothstep(1, 0.9, abs(cuv.x)) * smoothstep(1, 0.9, abs(cuv.y));
				//c = sign(v) * pow(abs(v), rt) / pow(0.5f, rt) + 0.5f;

				return fixed4(1, 1, 1, c);
			}
			ENDCG
		}
	}
}
