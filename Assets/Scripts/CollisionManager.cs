// Purpose: Detects collision between player and obstacles and stops the game if a collision is detected.
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public Transform player;
    public float collisionDistance = 0.1f;

    void Update()
    {
        foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            if (Vector3.Distance(player.position, obstacle.transform.position) < collisionDistance)
            {
                Debug.Log("Game Over!");
                Time.timeScale = 0; // Stop game
            }
        }
    }
}
