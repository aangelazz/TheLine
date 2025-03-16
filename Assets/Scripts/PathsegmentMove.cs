using UnityEngine;

public class PathSegmentMove : MonoBehaviour
{
    public float speed = 1f;
    public static bool isGreenBarClicked = false; // Flag to indicate if the green bar is clicked

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }
}