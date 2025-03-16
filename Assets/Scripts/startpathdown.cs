// Purpose: Move the initial path down, which is displayed on every start screen
using UnityEngine;

public class startpathdown : MonoBehaviour
{
    public float speed = 4f;
    public static bool startMoving = false;
    private float distanceToTravel;
    private float distanceTraveled = 0f;
    public float extraDistance = 2f; // Additional distance to travel beyond the screen length

    void Start()
    {
        // Calculate the distance to travel based on the camera's orthographic size plus extra distance
        distanceToTravel = 2 * Camera.main.orthographicSize + extraDistance;
    }

    void Update()
    {
        if (startMoving)
        {
            float movement = speed * Time.deltaTime;
            transform.Translate(Vector3.down * movement);
            distanceTraveled += movement;

            // Check if the object has traveled the full length of the screen plus extra distance
            if (distanceTraveled >= distanceToTravel)
            {
                startMoving = false;
                gameObject.SetActive(false); // Deactivate the GameObject
            }
        }
    }

    public void StartMoving()
    {
        startMoving = true;
        distanceTraveled = 0f; // Reset the distance traveled
    }

    public void ResetDistanceTraveled()
    {
        distanceTraveled = 0f;
    }
}