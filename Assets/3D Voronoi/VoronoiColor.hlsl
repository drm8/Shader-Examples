#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct VoronoiPointData
{
	float3 positionWS;
	float3 color;
};

// Global data recieved from the voronoi manager every frame
//#ifdef _VoronoiPointsData
CBUFFER_START(DataUpdate)
StructuredBuffer<VoronoiPointData> _VoronoiPointsData;
int _VoronoiPointCount;
CBUFFER_END
//#endif

void getVoronoiColor_float(float3 positionWS, out float3 color)
{
	float minDistance = 999999999;
	color = float3(0.5f, 0.5f, 0.5f);
	
//#ifdef _VoronoiPointsData
	[loop] // Finding the color of the closest point
	for (int i = 0; i < _VoronoiPointCount; i++)
	{
		VoronoiPointData p = _VoronoiPointsData.Load(i);
		float3 diff = positionWS - p.positionWS;
		float distanceSqr = diff.x * diff.x + diff.y * diff.y + diff.z * diff.z;
		if (distanceSqr < minDistance)
		{
			minDistance = distanceSqr;
			color = p.color;
		}
	}
//#endif
}
