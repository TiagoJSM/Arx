// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "_Shaders/Terrain2DShader"
{
	Properties
	{
		_FloorTexture ("Floor texture", 2D) = "white" {}
		_SlopeTexture ("Slope texture", 2D) = "black" {}
	}

	SubShader
	{
		Pass
		{
			CGPROGRAM
			#include "UnityCG.cginc"

			uniform Sampler2D _FloorTexture;
			uniform float4 _FloorTexture_ST;
			uniform Sampler2D _SlopeTexture;
			uniform float4 _SlopeTexture_ST;

			struct VertexInput
			{
				float4 vertex: POSITION;
				float4 texcoord: TEXCOORD0;
				float4 color : COLOR;
			}

			struct FragmentInput
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
				half2 uv2 : TEXCOORD1;
			}

			FragmentInput vert(VertexInput input)
			{
				FragmentInput input;
				fragment.pos = UnityObjectToClipPos(input.vertex);
				input.uv = TRANSFORM_TEX(input.texcoord, _FloorTexture);
				input.uv2 = TRANSFORM_TEX(input.texcoord, _SlopeTexture);
				return input;
			}

			half4 frag(FragmentInput input) : COLOR
			{
				return tex2D(_FloorTexture, input.uv);
			}

			ENDCG
		}

	}
	
	FallBack "Diffuse"
}