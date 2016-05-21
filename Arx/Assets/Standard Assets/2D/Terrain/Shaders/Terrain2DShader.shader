Shader "_Shaders/Terrain2DShader"
{
	Properties
	{
		_InterpolationColour ("Interpolation colour", Color) = (1, 1, 1, 1)
		_InterpolationFactor ("Interpolation factor", float) = 0
		_FloorEndingTexture ("Floor ending texture", 2D) = "black" {}
		_FloorTexture ("Floor texture", 2D) = "black" {}
		_SlopeEndingTexture ("Slope ending texture", 2D) = "black" {}
		_SlopeTexture ("Slope texture", 2D) = "black" {}
		_CeilingEndingTexture ("Ceiling ending texture", 2D) = "black" {}
		_CeilingTexture ("Ceiling texture", 2D) = "black" {}
		_FillingTexture ("Filling texture", 2D) = "black" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True" }
		Blend SrcAlpha OneMinusSrcAlpha
		Pass
		{
			Cull Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform float4 _InterpolationColour;
			uniform float _InterpolationFactor;

			uniform sampler2D _FloorEndingTexture;
			uniform float4 _FloorEndingTexture_ST;

			uniform sampler2D _FloorTexture;
			uniform float4 _FloorTexture_ST;

			uniform sampler2D _SlopeEndingTexture;
			uniform float4 _SlopeEndingTexture_ST;

			uniform sampler2D _SlopeTexture;
			uniform float4 _SlopeTexture_ST;

			uniform sampler2D _FillingTexture;
			uniform float4 _FillingTexture_ST;

			uniform sampler2D _CeilingTexture;
			uniform float4 _CeilingTexture_ST;

			uniform sampler2D _CeilingEndingTexture;
			uniform float4 _CeilingEndingTexture_ST;

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

				if(input.color.a == 0.0f)
				{
					fragment.uv = TRANSFORM_TEX(input.texcoord, _FloorEndingTexture);
				}
				else if(input.color.a == 0.1f)
				{
					fragment.uv = TRANSFORM_TEX(input.texcoord, _FloorTexture);
				}
				else if(input.color.a == 0.2f)
				{
					fragment.uv = TRANSFORM_TEX(input.texcoord, _SlopeEndingTexture);
				}
				else if(input.color.a == 0.3f)
				{
					fragment.uv = TRANSFORM_TEX(input.texcoord, _SlopeTexture);
				}
				else if(input.color.a == 0.4f)
				{
					fragment.uv = TRANSFORM_TEX(input.texcoord, _FillingTexture);
				}
				else if(input.color.a == 0.5f)
				{
					fragment.uv = TRANSFORM_TEX(input.texcoord, _CeilingEndingTexture);
				}
				else if(input.color.a == 0.6f)
				{
					fragment.uv = TRANSFORM_TEX(input.texcoord, _CeilingTexture);
				}

				fragment.color = input.color;
				return fragment;
			}

			float4 frag(FragmentInput input) : COLOR
			{
				float4 color;
				if(input.color.a == 0.0f)
				{
					color = tex2D(_FloorEndingTexture, input.uv);
				} 
				else if(input.color.a == 0.1f)
				{
					color = tex2D(_FloorTexture, input.uv);
				}
				else if(input.color.a == 0.2f)
				{
					color = tex2D(_SlopeEndingTexture, input.uv);
				}
				else if(input.color.a == 0.3f)
				{
					color = tex2D(_SlopeTexture, input.uv);
				}
				else if(input.color.a == 0.4f)
				{
					color = tex2D(_FillingTexture, input.uv);
				}
				else if(input.color.a == 0.5f)
				{
					color = tex2D(_CeilingEndingTexture, input.uv);
				}
				else
				{
					color = tex2D(_CeilingTexture, input.uv);
				}
				
				float alpha = color.a;
				color = lerp(color, _InterpolationColour, _InterpolationFactor);
				color.a = alpha;
				return color;
			}

			ENDCG
		}

	}
	
	FallBack "Diffuse"
}