// Purpose: This script is responsible for generating the road segments.
// It instantiates the road segment prefab and adds the PathSegmentMove script to it. 
using UnityEngine;
using System.Collections;

public class RoadGenerator : MonoBehaviour
{
    public GameObject roadSegmentPrefab; // Reference to the road segment prefab
    public float segmentHeight = 10f; // Height of each road segment
    public float speed = 0.8f; // Speed at which the segments move down
    public float spawnInterval = 0.37f; // Time interval between spawning new segments
    // for randomly generating the path, which was not used: private float _heightRange = 0.6f;

    private float timer;

    private void Start()
    {
        // Road generation is started from the PathSegmentMove script
    }

    private void Update()
    {
        if (PathSegmentMove.isGreenBarClicked)
        {
            timer += Time.deltaTime;

            if (timer > spawnInterval)
            {
                SpawnSegment();
                timer = 0;
            }
        }
    }

    public void StartGenerating()
    {
        SpawnSegment(); // Spawn the first segment immediately
    }

    private void SpawnSegment()
    {
        Vector3 spawnPos = new Vector3(-12, 2, -5);
        GameObject segment = Instantiate(roadSegmentPrefab, spawnPos, Quaternion.identity);
        segment.AddComponent<PathSegmentMove>().speed = speed;
        Destroy(segment, 10f);
    }
}