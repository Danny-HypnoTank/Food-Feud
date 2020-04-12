Shader "Unlit/DrawSplatMap"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_SplatTex("SplatTexture", 2D) = "white" {}
		_Coordinate("Coordinate", Vector) = (0,0,0,0)
		_Color("DrawColor", color) = (0,1,0,0)
		_ClearColor("ClearColor", color) = (1,1,1,1)
			//_ClearColor("ClearColor", color) = (0,0,0,0)
			_Size("Size", Range(0,500)) = 1
			_TexSize("TexSize", Range(0,1)) = 1
			_Rotation("Rotation", Float) = 0
		
		_Strength("Strength", Range(0,1)) = 1
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" }
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
				float _Rotation;
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
					s = s + (i.uv - _Coordinate.xy) / _Size;

					s.xy -= 0.5;
					float sn = -sin(_Rotation);
					float cs = cos(_Rotation);
					float2x2 rotationMatrix = float2x2 (cs, -sn, sn, cs);
					rotationMatrix *= 0.5;
					rotationMatrix += 0.5;
					rotationMatrix = rotationMatrix * 2 - 1;
					s.xy = mul(s.xy, rotationMatrix);
					s.xy += 0.5;

					fixed4 drawcol = tex2D(_SplatTex, s.xy);
					
					//create mask
					float isMask = tex2D(_SplatTex, s.xy).a != 0;

					
					//leave regular colour for texture, unless it is inside the mask, then make it the colour of the player
					col = (1 - isMask) * col + isMask * (_Color * tex2D(_SplatTex, s.xy).a);
					return col;
					
				}
				ENDCG
			}
		}
}
