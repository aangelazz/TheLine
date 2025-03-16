// Purpose: Script to define the dimensions of a collidable object, to be detected by ball
using UnityEngine;

public class CollidableObject : MonoBehaviour
{
    public float width = 1f; // Width of the object
    public float height = 1f; // Height of the object

    void Start()
    {
        // Auto-measure on start if not set
        if (width <= 0 || height <= 0)
        {
            AutoSetDimensions();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 1f));
    }

    public void AutoSetDimensions()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            width = spriteRenderer.bounds.size.x;
            height = spriteRenderer.bounds.size.y;
        }
    }
}