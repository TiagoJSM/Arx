#ifndef TERRAIN_HELPERS
#define TERRAIN_HELPERS

#pragma enable_d3d11_debug_symbols
#include "UnityCG.cginc"

inline float4 GetSurfaceColor(fixed2 uv, fixed4 coords, sampler2D tex)
{
	float2 leftBottom = float2(coords.x, coords.y);
	float2 originRange = float2(coords.z - coords.x, coords.w - coords.y);
	float2 processedUv = float2(originRange.x * uv.x, originRange.y * uv.y) + leftBottom;
	return tex2D(tex, processedUv);
}

#endif