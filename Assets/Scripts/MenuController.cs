// Handles the game's main menu functionality, including pausing and retrying the game.
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject initialPath; // Reference to the initial path GameObject
    public GameObject ball; // Reference to the ball GameObject
    public TouchControl touchControl; // Reference to the TouchControl script
    // Add game over screen reference
    public GameObject gameOverScreen;

    private Vector3 initialPathOriginalPosition = new Vector3(-1.057902f, -0.2369416f, -9.941503f);

    void Start()
    {
        menuCanvas.SetActive(false);
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    public void OpenMenu()
    {
        Time.timeScale = 0;
        menuCanvas.SetActive(true);
    }

    public void CloseMenu()
    {
        Time.timeScale = 1;
        menuCanvas.SetActive(false);
    }

    public void RestartGame()
    {
        // Save the best score
        if (touchControl != null)
        {
            touchControl.SaveBestScore();
        }

        // Hide the game over screen
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
        else
        {
            // If not directly referenced, try to find it by tag or through the ball's detector
            BallCollisionDetector ballDetector = ball.GetComponent<BallCollisionDetector>();
            if (ballDetector != null)
            {
                ballDetector.HideGameOverScreen();
            }
        }

        // Reset the ball's position
        ball.transform.position = new Vector3(0, -1.25f, ball.transform.position.z);

        // Reset collision detector
        BallCollisionDetector collisionDetector = ball.GetComponent<BallCollisionDetector>();
        if (collisionDetector != null)
        {
            collisionDetector.Reset();
        }

        // Reset the initial path's position and reactivate it
        initialPath.transform.position = initialPathOriginalPosition;
        initialPath.SetActive(true);

        // Reset the startpathdown script on the initial path
        startpathdown startPathDownScript = initialPath.GetComponent<startpathdown>();
        if (startPathDownScript != null)
        {
            startPathDownScript.ResetDistanceTraveled();
        }

        // Disable all currently moving path segments
        PathSegmentMove[] pathSegments = FindObjectsOfType<PathSegmentMove>();
        foreach (PathSegmentMove segment in pathSegments)
        {
            segment.gameObject.SetActive(false);
        }

        // Reset any other necessary game state here
        PathSegmentMove.isGreenBarClicked = false;
        startpathdown.startMoving = false;

        // Reset the hasStartedMoving flag and score in the TouchControl script
        touchControl.ResetHasStartedMoving();

        // Close the menu and resume the game
        CloseMenu();
    }
}