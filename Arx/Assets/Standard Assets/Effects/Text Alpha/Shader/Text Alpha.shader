// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit / Text Alpha"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0

		_AlphaTexture("Alpha Texture", 2D) = "white" {}
		_MinimumValue("Minimum Value", float) = 0
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
		_MaximumOutlineAlpha("Minimum Outline Alpha", Range(0.0, 1.0)) = 0
		_OutlineFocus("Outline Focus", Range(0.0, 10.0)) = 5
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				float2 alphaTexcoord : TEXCOORD2;
			};

			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;

			sampler2D _AlphaTexture;
			uniform float4 _AlphaTexture_ST;
			
			uniform float _MinimumValue;
			uniform float4 _OutlineColor;
			uniform float _MaximumOutlineAlpha;
			uniform float _OutlineFocus;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPosition = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
				OUT.alphaTexcoord = TRANSFORM_TEX(IN.texcoord, _AlphaTexture);

				OUT.texcoord = IN.texcoord;

				#ifdef UNITY_HALF_TEXEL_OFFSET
						OUT.vertex.xy += (_ScreenParams.zw - 1.0)*float2(-1,1);
				#endif

				OUT.color = IN.color * _Color;
				return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : SV_Target
			{
				/*half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

				color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

				#ifdef UNITY_UI_ALPHACLIP
						clip(color.a - 0.001);
				#endif

				return color;*/

				float4 mainTextureColor = tex2D(_MainTex, IN.texcoord);
				float4 col = IN.color;
				float4 alphaTex = tex2D(_AlphaTexture, IN.alphaTexcoord);
				
				col.a *= mainTextureColor.a;
				
				if (mainTextureColor.a < _MaximumOutlineAlpha && mainTextureColor.a > 0)
				{
					col = _OutlineColor;
					col.a *= mainTextureColor.a * _OutlineFocus;
				}
				
				if (alphaTex.r < _MinimumValue) {
					col.a = 0;
				}

				col.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

				#ifdef UNITY_UI_ALPHACLIP
					clip(col.a - 0.001);
				#endif
				
				return col ;
			}
			ENDCG
		}
	}
}

