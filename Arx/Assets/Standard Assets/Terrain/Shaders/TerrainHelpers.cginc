#ifndef TERRAIN_HELPERS
#define TERRAIN_HELPERS

#include "UnityCG.cginc"

inline fixed4 SetSurfaceColor(float2 uv, fixed4 coords, sampler2D tex)
{
	float2 leftBottom = float2(coords.x, coords.y);
	float2 originRange = float2(coords.z - coords.x, coords.w - coords.y);
	float2 processedUv = float2(originRange.x * uv.x, originRange.y * uv.y) + leftBottom;
	return tex2D(tex, processedUv);
}


#endif