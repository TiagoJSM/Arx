// Upgrade NOTE: replaced '_LightMatrix0' with 'unity_WorldToLight'

// Upgrade NOTE: replaced '_LightMatrix0' with 'unity_WorldToLight'
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_LightMatrix0' with 'unity_WorldToLight'

Shader "2DTerrain/Lit"
{
	Properties
	{
		_Texture("Terrain texture", 2D) = "black" {}

		_FloorLeftEnding("Floor left ending", Vector) = (0, 0, 0.3, 1)
		_Floor("Floor", Vector) = (0.3, 0, 0.6, 1)
		_FloorRightEnding("Floor right ending", Vector) = (0.6, 0, 1, 1)

		_CeilingLeftEnding("Ceiling left ending", Vector) = (0, 0, 0.3, 1)
		_Ceiling("Ceiling", Vector) = (0.3, 0, 0.6, 1)
		_CeilingRightEnding("Ceiling right ending", Vector) = (0.6, 0, 1, 1)

		_SlopeLeftEnding("Slope left ending", Vector) = (0, 0, 0.3, 1)
		_Slope("Slope", Vector) = (0.3, 0, 0.6, 1)
		_SlopeRightEnding("Slope right ending", Vector) = (0.6, 0, 1, 1)

		_InterpolationColour("Interpolation colour", Color) = (1, 1, 1, 1)
		_InterpolationFactor("Interpolation factor", float) = 0

		_FillingInterpolationColour("Filling Interpolation colour", Color) = (1, 1, 1, 1)
		_FillingInterpolationFactor("Filling Interpolation factor", float) = 0

		_FillingTexture("Filling texture", 2D) = "black" {}
	}

	SubShader
	{

		Tags{
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
		}

		//Cull Off
		ZWrite Off

		Blend SrcAlpha OneMinusSrcAlpha
		//BlendOp Min

		CGPROGRAM
#pragma surface surf SimpleLambert alpha

#include "UnityCG.cginc"
#include "TerrainHelpers.cginc"

	uniform sampler2D _Texture;
	uniform float4 _Texture_ST;

	uniform float4 _FloorLeftEnding;
	uniform float4 _Floor;
	uniform float4 _FloorRightEnding;

	uniform float4 _CeilingLeftEnding;
	uniform float4 _Ceiling;
	uniform float4 _CeilingRightEnding;

	uniform float4 _SlopeLeftEnding;
	uniform float4 _Slope;
	uniform float4 _SlopeRightEnding;

	uniform float4 _InterpolationColour;
	uniform float _InterpolationFactor;

	uniform float4 _FillingInterpolationColour;
	uniform float _FillingInterpolationFactor;

	uniform sampler2D _FillingTexture;

	struct Input
	{
		float2 uv_FillingTexture : TEXCOORD0;
		float4 color : COLOR;
		float4 pos : SV_POSITION;
	};

	half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten) {
		half4 c;
		c.rgb = s.Albedo *_LightColor0.rgb * atten;
		c.a = s.Alpha;
		return c;
	}

	void vert(inout appdata_full input, out Input o)
	{
		input.vertex = UnityPixelSnap(input.vertex);
		UNITY_INITIALIZE_OUTPUT(Input, o);
		o.color = input.color;
	}

	float4 SampleSpriteTexture(Input IN, float2 uv)
	{
		float2 uvFrac = frac(uv);

		if (IN.color.a == 0.6f)
		{
			return tex2D(_FillingTexture, uv);
		}

		return float4(0.0, 0.0, 0.0, 0.0);
	}

	float4 LayerInterpolation(float4 color, Input IN)
	{
		if (IN.color.a == 0.6f)
		{
			return lerp(color, _FillingInterpolationColour, _FillingInterpolationFactor);
		}
		float4 r = lerp(color, _InterpolationColour, _InterpolationFactor);
		r.a = color.a;
		return r;
	}

	void surf(Input IN, inout SurfaceOutput o)
	{
		float4 c = SampleSpriteTexture(IN, IN.uv_FillingTexture);
		c = LayerInterpolation(c, IN);
		o.Albedo = c.rgb * c.a;
		o.Alpha = c.a;
	}

	ENDCG

		CGPROGRAM
		#pragma surface surf SimpleLambert alpha

		#include "UnityCG.cginc"
		#include "AutoLight.cginc"
		#include "TerrainHelpers.cginc"


		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;

		uniform float4 _FloorLeftEnding;
		uniform float4 _Floor;
		uniform float4 _FloorRightEnding;

		uniform float4 _CeilingLeftEnding;
		uniform float4 _Ceiling;
		uniform float4 _CeilingRightEnding;

		uniform float4 _SlopeLeftEnding;
		uniform float4 _Slope;
		uniform float4 _SlopeRightEnding;

		uniform float4 _InterpolationColour;
		uniform float _InterpolationFactor;

		uniform float4 _FillingInterpolationColour;
		uniform float _FillingInterpolationFactor;

		uniform sampler2D _FillingTexture;

		struct Input
		{
			float2 uv_FillingTexture : TEXCOORD0;
			float4 color : COLOR;
			float4 pos : SV_POSITION;
			//float4 posLight : TEXCOORD1;
			//float4 posWorld : TEXCOORD2;
		};

		half4 LightingSimpleLambert(SurfaceOutput s, half3 lightDir, half atten) {
			half4 c;
			c.rgb = s.Albedo *_LightColor0.rgb * atten;
			c.a = s.Alpha;
			return c;
		}

		void vert(inout appdata_full input, out Input o)
		{
			input.vertex = UnityPixelSnap(input.vertex);
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = input.color;

			/*float4x4 modelMatrix = unity_ObjectToWorld;
			o.posWorld = mul(modelMatrix, input.vertex);
			o.posLight = mul(unity_WorldToLight, o.posWorld);*/
		}

		float4 SampleSpriteTexture(Input IN, float2 uv)
		{
			float2 uvFrac = frac(uv);

			if (IN.color.a == 0.0f)
			{
				return GetSurfaceColor(uvFrac, _FloorLeftEnding, _Texture).xyzw;
			}
			else if (IN.color.a == 0.1f)
			{
				return GetSurfaceColor(uvFrac, _Floor, _Texture).xyzw;
			}
			else if (IN.color.a == 0.2f)
			{
				return GetSurfaceColor(uvFrac, _FloorRightEnding, _Texture).xyzw;
			}
			else if (IN.color.a == 0.3f)
			{
				return GetSurfaceColor(uvFrac, _SlopeLeftEnding, _Texture).xyzw;
			}
			else if (IN.color.a == 0.4f)
			{
				return GetSurfaceColor(uvFrac, _Slope, _Texture).xyzw;
			}
			else if (IN.color.a == 0.5f)
			{
				return GetSurfaceColor(uvFrac, _SlopeRightEnding, _Texture).xyzw;
			}
			else if (IN.color.a == 0.6f)
			{
				return float4(0.0, 0.0, 0.0, 0.0);
			}
			else if (IN.color.a == 0.7f)
			{
				return GetSurfaceColor(uvFrac, _CeilingLeftEnding, _Texture).xyzw;
			}
			else if (IN.color.a == 0.8f)
			{
				return GetSurfaceColor(uvFrac, _Ceiling, _Texture).xyzw;
			}
			else if (IN.color.a == 0.9f)
			{
				return GetSurfaceColor(uvFrac, _CeilingRightEnding, _Texture).xyzw;
			}
			return float4(1.0, 0, 0, 1.0).xyzw;
		}

		float4 LayerInterpolation(float4 color, Input IN)
		{
			if (IN.color.a == 0.6f)
			{
				return lerp(color, _FillingInterpolationColour, _FillingInterpolationFactor);
			}
			float4 r = lerp(color, _InterpolationColour, _InterpolationFactor);
			r.a = color.a;
			return r;
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			//float distance = IN.posLight.z;
			/*float attenuation =
				tex2D(_LightTexture0, float2(distance, distance)).a;*/
			float4 c = SampleSpriteTexture(IN, IN.uv_FillingTexture);
			c = LayerInterpolation(c, IN);
			o.Albedo = c.rgb * c.a;// *_LightColor0;
			o.Alpha = c.a;
		}

		ENDCG
	}

	Fallback "Diffuse"
}