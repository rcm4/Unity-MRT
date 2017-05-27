Shader "Custom/MRTs"
{
	Properties
	{
		_Color("Color", color) = (1, 0, 0, 1)
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
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{				
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal: NORMAL;
				float3 worldNormal : TEXCOORD2;
			};

			struct fout 
			{
				float4 rt0 : SV_Target0;
				float4 rt1 : SV_Target1;
				float4 rt2 : SV_Target2;
				float4 rt3 : SV_Target3;				
			};
			
			float4 _Color;

			// *********

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = v.normal;
				o.worldNormal = mul(unity_ObjectToWorld, float4(v.normal, 0.0f)).xyz;

				return o;
			}
			
			fout frag(v2f i)
			{
				float depth = i.pos.z / i.pos.w;

				fout o;
				o.rt0 = float4(depth, depth, depth, 1);
				o.rt1 = _Color;
				o.rt2 = float4(i.normal.xyz, 1);
				o.rt3 = float4(i.worldNormal.xyz, 1);
				
				return o;
			}
			ENDCG
		}
	}
}
