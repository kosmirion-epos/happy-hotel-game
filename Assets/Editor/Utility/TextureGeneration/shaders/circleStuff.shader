Shader "Texture_Generation/Circle Stuff"
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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed2 uv = i.uv * fixed2(2, 1) - fixed2(1, 0);
				
				//fixed center = 5;
				//fixed scale = 1;
				//fixed ex = 3;
				//
				//fixed c = max(0, scale - length(uv)) / scale;
				//c = pow(c, ex);
				//c *= center;
				
				fixed s = 0.05;
				fixed hs = s * 0.5;
				fixed inv = 1 - s;

				fixed c = max(0, 1 - smoothstep(inv + uv.y * hs, 1, length(uv))) * smoothstep(0, hs, i.uv.y);

				return fixed4(c, c, c, 1);
			}
			ENDCG
		}
	}
}
