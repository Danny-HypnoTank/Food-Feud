Shader "Unlit/CutSplatMap"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_SplatTex("SplatTexture", 2D) = "white" {}
		_Coordinate("Coordinate", Vector) = (0,0,0,0)
		_Color("DrawColor", color) = (1,0,0,0)
		_ClearColor("ClearColor", color) = (1,1,1,1)
		_Size("Size", Range(0,500)) = 1
		_TexSize("TexSize", Range(0,1)) = 1

		_Strength("Strength", Range(0,1)) = 1
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


				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _Coordinate, _Color, _ClearColor;
				half _Size, _Strength, _TexSize;
				sampler2D _SplatTex;
				float2 s;
				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					fixed4 col = tex2D(_MainTex, i.uv);
					

					
					//get position and size
					s = float2(0.5, 0.5);
					s = s + (i.uv - _Coordinate.xy)/_Size;
					fixed4 drawcol = tex2D(_SplatTex, s.xy);
					
					//create mask
					float isMask = tex2D(_SplatTex, s.xy) == _Color;


					//leave regular colour for texture, unless it is inside the mask, then clear the colour inside
					col = (1 - isMask) * col - isMask * _Color;
					return col;
					
				}
				ENDCG
			}
		}
}
