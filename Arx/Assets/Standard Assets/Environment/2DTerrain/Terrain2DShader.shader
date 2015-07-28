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
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _FloorTexture;
			uniform float4 _FloorTexture_ST;
			uniform sampler2D _SlopeTexture;
			uniform float4 _SlopeTexture_ST;

			struct VertexInput
			{
				float4 vertex: POSITION;
				float4 texcoord: TEXCOORD0;
				float4 color : COLOR;
			};

			struct FragmentInput
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
				float4 color : COLOR0;
			};

			FragmentInput vert(VertexInput input)
			{
				FragmentInput fragment;
				fragment.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				fragment.uv = TRANSFORM_TEX(input.texcoord, _FloorTexture);
				fragment.color = input.color;
				return fragment;
			}

			half4 frag(FragmentInput input) : COLOR
			{
				if(input.color.a == 0.0f)
				{
					return tex2D(_SlopeTexture, input.uv);
				}
				return tex2D(_FloorTexture, input.uv);
			}

			ENDCG
		}

	}
	
	FallBack "Diffuse"
}