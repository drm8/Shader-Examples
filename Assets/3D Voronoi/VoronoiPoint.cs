using System;
using Unity.Mathematics;
using UnityEngine;
using static VoronoiManager;

[ExecuteInEditMode]
public class VoronoiPoint : MonoBehaviour
{
    [SerializeField] private Color color;
    private VoronoiPointData data = new VoronoiPointData();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VoronoiManager.Singleton.pointRequest += OnVoronoiUpdate;
        data.color = new float3(color.r, color.g, color.b);
	}

    public void OnVoronoiUpdate()
    {
        data.positionWS = (float3)transform.position;
        VoronoiManager.Singleton.AddPointData(data);
	}
}
