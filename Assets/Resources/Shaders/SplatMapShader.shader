// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/SplatMapShader"
{
    Properties
    {
        _SplatMap ("SplatMap", 2D) = "black" {}
		[NoScaleOffset] _Paint1("Paint1", 2D) = "white" {}
		[NoScaleOffset] _Paint2("Paint2", 2D) = "white" {}
		[NoScaleOffset] _Paint3("Paint3", 2D) = "white" {}
		[NoScaleOffset] _Paint4("Paint4", 2D) = "white" {}
		[NoScaleOffset] _Paint5("Paint5", 2D) = "white" {}
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
			sampler2D _SplatMap;
			float4 _SplatMap_ST;

			sampler2D _Paint1, _Paint2, _Paint3, _Paint4, _Paint5;

            struct VertexData
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 position : SV_POSITION;
				float2 uvSplat : TEXCOORD1;
            };

            

			Interpolators vert (VertexData v)
			{
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);
				i.uv = TRANSFORM_TEX(v.uv, _SplatMap);
				i.uvSplat = v.uv;
				return i;
			}
			float4 frag (Interpolators i) : SV_Target
			{
				float4 splat = tex2D(_SplatMap, i.uvSplat);

				return 
					tex2D(_Paint1, i.uv) * splat.r + 
					tex2D(_Paint2, i.uv) * splat.g +
					tex2D(_Paint3, i.uv) * splat.b +
					tex2D(_Paint4, i.uv) * splat.a +
					tex2D(_Paint5, i.uv) * (1 - splat.r - splat.g - splat.b - splat.a);
			}
            

            
            ENDCG
        }
    }
}
