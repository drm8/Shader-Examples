using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

[ExecuteInEditMode]
public class VoronoiManager : MonoBehaviour
{
	// Singleton
    private static VoronoiManager singleton;
	public static VoronoiManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                GameObject manager = new GameObject();
                manager.name = "Voronoi Manager";
				manager.AddComponent<VoronoiManager>();
			}

            return singleton;

		}
    }

	// Buffer
	private ComputeBuffer pointBuffer;
	private int maxBufferSize = 32;
	private VoronoiPointData[] framePointData;
	private int framePointCount = 0;

	// Struct
	public struct VoronoiPointData
	{
		public float3 positionWS;
		public float3 color;
	}
	private VoronoiPointData defaultPoint;

	private void Awake()
	{
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);

		pointBuffer = new ComputeBuffer(maxBufferSize, 24, ComputeBufferType.Structured, ComputeBufferMode.SubUpdates);
		framePointData = new VoronoiPointData[maxBufferSize];
		WriteDefaultBuffer();
	}

	// Writes default point data to the point buffer
	private void WriteDefaultBuffer()
	{
		NativeArray<VoronoiPointData> nativePointData =
				pointBuffer.BeginWrite<VoronoiPointData>(0, 1);

		nativePointData[0] = defaultPoint;
		pointBuffer.EndWrite<VoronoiPointData>(1);

		// There must be at least one entry in the buffer, but it won't be read since the point count is set to 0.
		Shader.SetGlobalBuffer("_VoronoiPointsData", pointBuffer);
		Shader.SetGlobalInt("_VoronoiPointCount", 0);
	}

	public Action pointRequest;

	// Update is called once per frame
	void Update()
    {
		// Get point data.
		// Each point is saved to an array before the buffer
		// is written to get a count of how many there are.
		framePointCount = 0;
		pointRequest?.Invoke();

		// Send point data to shader
		if (framePointCount == 0)
		{
			WriteDefaultBuffer();
			return;
		}
		else
		{
			// Write data to buffer
			NativeArray<VoronoiPointData> nativePointData =
				pointBuffer.BeginWrite<VoronoiPointData>(0, framePointCount);

			for (int i = 0; i < framePointCount; i++)
				nativePointData[i] = framePointData[i];

			pointBuffer.EndWrite<VoronoiPointData>(framePointCount);

			// Apply changes to global shader variables
			Shader.SetGlobalBuffer("_VoronoiPointData", pointBuffer);
			Shader.SetGlobalInt("_VoronoiPointCount", framePointCount);
		}
	}

	public void AddPointData(VoronoiPointData data)
	{
		if (framePointCount < maxBufferSize)
		{
			framePointData[framePointCount] = data;
			framePointCount++;
		}
	}

	private void OnDestroy() { discardBuffer(); }
	private void OnDisable() { discardBuffer(); }

	private void discardBuffer()
	{
		// Cleaning up memory
		if (pointBuffer != null)
		{
			pointBuffer.Release();
			pointBuffer = null;
		}
	}
}
