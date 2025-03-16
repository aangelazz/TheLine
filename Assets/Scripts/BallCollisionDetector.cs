// Script to detect collisions between the ball and the path segments
using UnityEngine;
using System.Collections.Generic;

public class BallCollisionDetector : MonoBehaviour
{
    public float ballRadius = 0.5f; // Radius of ball
    public GameObject gameOverScreen; // Reference to the GameOverScreen UI canvas
    public TouchControl touchControl; // Reference to TouchControl script
    public float immunityAfterStart = 0.5f; // Immunity period in seconds, to be added in case of the booster

    private List<GameObject> pathSegments = new List<GameObject>();
    private bool isGameOver = false;
    private float immunityTimer = 0f;
    private bool hasImmunity = false;

    void Start()
    {
        if (gameOverScreen == null)
        {
            Debug.LogError("GameOverScreen reference not set in BallCollisionDetector!");
        }

        RefreshPathSegments();

        if (touchControl == null)
        {
            touchControl = FindObjectOfType<TouchControl>();
        }
    }

    void Update()
    {
        if (isGameOver) return;

        // Update immunity timer if active
        if (hasImmunity)
        {
            immunityTimer -= Time.deltaTime;
            if (immunityTimer <= 0f)
            {
                hasImmunity = false;
            }
            return; // Skip collision detection during immunity
        }

        // Check collisions every frame
        CheckCollisions();
    }

    public void GrantImmunity()
    {
        immunityTimer = immunityAfterStart;
        hasImmunity = true;
    }

    void CheckCollisions()
    {
        // Refresh the path segments list periodically
        if (Time.frameCount % 30 == 0)
        {
            RefreshPathSegments();
        }

        // Get the ball position once
        Vector3 ballPos = transform.position;

        // Check for collisions with each path segment
        for (int i = 0; i < pathSegments.Count; i++)
        {
            GameObject segment = pathSegments[i];
            if (segment == null || !segment.activeSelf)
            {
                continue;
            }

            if (FastCollisionCheck(segment, ballPos))
            {
                ActivateGameOver();
                break;
            }
        }
    }

    public void RefreshPathSegments()
    {
        pathSegments.Clear();
        GameObject[] segments = GameObject.FindGameObjectsWithTag("PathSegment");
        pathSegments.AddRange(segments);
    }

    // Faster collision check without extra logging
    bool FastCollisionCheck(GameObject pathSegment, Vector3 ballPos)
    {
        // Get the dimensions of the path segment
        CollidableObject collidable = pathSegment.GetComponent<CollidableObject>();
        if (collidable == null) return false;

        // If dimensions are zero, try to measure
        if (collidable.width <= 0 || collidable.height <= 0)
        {
            collidable.AutoSetDimensions();
        }

        float segmentWidth = collidable.width;
        float segmentHeight = collidable.height;

        // Get segment position
        Vector3 segmentPos = pathSegment.transform.position;

        // Calculate the segment's bounds
        float segmentLeft = segmentPos.x - segmentWidth / 2;
        float segmentRight = segmentPos.x + segmentWidth / 2;
        float segmentTop = segmentPos.y + segmentHeight / 2;
        float segmentBottom = segmentPos.y - segmentHeight / 2;

        // Find the closest point on the segment to the ball
        float closestX = Mathf.Max(segmentLeft, Mathf.Min(ballPos.x, segmentRight));
        float closestY = Mathf.Max(segmentBottom, Mathf.Min(ballPos.y, segmentTop));

        // Calculate the distance from the ball to the closest point
        float distanceX = ballPos.x - closestX;
        float distanceY = ballPos.y - closestY;
        float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);

        // Check if the distance is less than the ball's radius
        return distanceSquared < (ballRadius * ballRadius);
    }

    void ActivateGameOver()
    {
        isGameOver = true;
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        // Stop the score counter
        if (touchControl != null)
        {
            touchControl.StopScoring();
        }
    }

    // Method to reset the collision detector when the game is restarted
    public void Reset()
    {
        isGameOver = false;
        hasImmunity = false;
        immunityTimer = 0f;
        HideGameOverScreen();
        RefreshPathSegments();
    }

    // Hide the game over screen
    public void HideGameOverScreen()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    // Visualize the ball's collision radius in the Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ballRadius);
    }
}