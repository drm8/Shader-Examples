using Unity.Mathematics;
using UnityEngine;

public class WanderBehavior : MonoBehaviour
{
    private float noiseOffsetX;
	private float noiseOffsetY;
	private float noiseOffsetZ;

	private Vector3 initialPosition;

	[SerializeField] private float moveRate = 1;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		// Generate offsets
        noiseOffsetX = UnityEngine.Random.value * 10;
		noiseOffsetY = UnityEngine.Random.value * 10 + 10;
		noiseOffsetZ = UnityEngine.Random.value * 10 + 20;

		// Record initial position
		initialPosition = transform.localPosition;
	}

    // Update is called once per frame
    void Update()
    {
		// Calculate offset from initial position for this frame
		Vector3 offset = new Vector3(
			Mathf.PerlinNoise(Time.time * moveRate, noiseOffsetX),
			Mathf.PerlinNoise(Time.time * moveRate, noiseOffsetY),
			Mathf.PerlinNoise(Time.time * moveRate, noiseOffsetZ));

		transform.localPosition = initialPosition + offset;
	}
}
