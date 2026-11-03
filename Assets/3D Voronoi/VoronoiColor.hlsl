struct VoronoiPointData
{
	float3 positionWS;
	float3 color;
};

// Global data recieved from the voronoi manager every frame
CBUFFER_START(DataUpdate)
StructuredBuffer<VoronoiPointData> _VoronoiPointsData;
int _VoronoiPointCount;
CBUFFER_END

void getVoronoiColor_float(float3 positionWS, out float3 color)
{
	float minDistance = 999999999;
	color = float3(0.5f, 0.5f, 0.5f);
	
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
}
