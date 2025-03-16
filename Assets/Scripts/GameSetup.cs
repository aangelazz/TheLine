// Purpose: Measure the dimensions of the ball and path segment prefabs before the game starts, 
// and update the dimensions of active path segments in the scene. 
using UnityEngine;
using System.Collections.Generic;

public class GameSetup : MonoBehaviour
{
    public GameObject ball;
    public GameObject[] pathSegmentPrefabs;

    void Start()
    {
        MeasureObjects();
    }

    [ContextMenu("Measure Objects")]
    public void MeasureObjects()
    {
        // Measure the ball
        if (ball != null)
        {
            SpriteRenderer ballRenderer = ball.GetComponent<SpriteRenderer>();
            if (ballRenderer != null)
            {
                float ballDiameter = Mathf.Max(ballRenderer.bounds.size.x, ballRenderer.bounds.size.y);
                float ballRadius = ballDiameter / 2f;

                BallCollisionDetector ballCollider = ball.GetComponent<BallCollisionDetector>();
                if (ballCollider != null)
                {
                    ballCollider.ballRadius = ballRadius;
                    Debug.Log("Ball radius set to: " + ballRadius);
                }
                else
                {
                    Debug.LogError("BallCollisionDetector component not found on ball!");
                }
            }
            else
            {
                Debug.LogError("SpriteRenderer not found on ball!");
            }
        }
        else
        {
            Debug.LogError("Ball reference not set in GameSetup!");
        }

        // Measure prefabs
        Debug.Log($"Measuring {pathSegmentPrefabs.Length} path segment prefabs");
        foreach (GameObject segment in pathSegmentPrefabs)
        {
            if (segment != null)
            {
                CollidableObject collidable = segment.GetComponent<CollidableObject>();
                if (collidable != null)
                {
                    MeasureSegment(segment, collidable);
                }
                else
                {
                    Debug.LogError($"CollidableObject component not found on prefab: {segment.name}");
                }
            }
        }

        // Also measure active segments in the scene
        MeasureActiveSegments();
    }

    void MeasureSegment(GameObject segment, CollidableObject collidable)
    {
        SpriteRenderer renderer = segment.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            collidable.width = renderer.bounds.size.x;
            collidable.height = renderer.bounds.size.y;
            Debug.Log($"Path segment {segment.name} dimensions set: Width={collidable.width}, Height={collidable.height}");
        }
        else
        {
            Debug.LogWarning($"SpriteRenderer not found on {segment.name}. Using default dimensions.");
        }
    }

    public void MeasureActiveSegments()
    {
        // Find all active segments with the PathSegment tag
        GameObject[] activeSegments = GameObject.FindGameObjectsWithTag("PathSegment");
        Debug.Log($"Found {activeSegments.Length} active path segments in scene");

        foreach (GameObject segment in activeSegments)
        {
            CollidableObject collidable = segment.GetComponent<CollidableObject>();
            if (collidable != null)
            {
                MeasureSegment(segment, collidable);
            }
        }
    }
}