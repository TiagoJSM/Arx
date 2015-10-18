Shader "_Shaders/NodeMesh"
{
	Properties
	{
		_InterpolationColour ("Interpolation colour", Color) = (1, 1, 1, 1)
		_InterpolationFactor ("Interpolation factor", float) = 0
		_MainTex ("Main texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
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

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;

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
				fragment.uv = TRANSFORM_TEX(input.texcoord, _MainTex);
				fragment.color = input.color;
				return fragment;
			}

			float4 frag(FragmentInput input) : COLOR
			{
				float4 color = tex2D(_MainTex, input.uv);
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