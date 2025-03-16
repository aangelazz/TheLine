// Purpose: Script to handle touch input to control ball, and scoring in the game.
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{
    public GameObject ball;
    public RectTransform bar;
    public RoadGenerator roadGenerator;
    public TMP_Text scoreText;
    public TMP_Text gameoverscoreText;
    public TMP_Text bestScoreText;

    private Camera mainCamera;
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    private bool hasStartedMoving = false;
    private bool isGameOver = false;
    private int score = 0;
    private int bestScore = 0;
    private float scoreTimer = 0f;

    void Start()
    {
        mainCamera = Camera.main;
        raycaster = bar.GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestScoreText.text = bestScore.ToString();
    }

    void Update()
    {
        // Only allow green bar interaction if not game over
        if (!isGameOver && Input.GetMouseButton(0))
        {
            Vector3 touchPos = Input.mousePosition;
            PointerEventData pointerData = new PointerEventData(eventSystem);
            pointerData.position = touchPos;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject == bar.gameObject)
                {
                    if (!hasStartedMoving)
                    {
                        startpathdown.startMoving = true;
                        PathSegmentMove.isGreenBarClicked = true;
                        roadGenerator.StartGenerating();
                        hasStartedMoving = true;
                    }

                    Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, mainCamera.nearClipPlane));
                    ball.transform.position = new Vector3(worldPos.x, ball.transform.position.y, ball.transform.position.z);
                    break;
                }
            }
        }

        // Only update score if game is active
        if (hasStartedMoving && !isGameOver)
        {
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= 0.1f)
            {
                score += 1;
                scoreText.text = score.ToString();
                gameoverscoreText.text = score.ToString();
                scoreTimer = 0f;
            }
        }
    }

    // Method to be called when collision is detected
    public void StopScoring()
    {
        isGameOver = true;
        SaveBestScore();
        Debug.Log("Scoring stopped due to collision. Final score: " + score);
    }

    public void ResetHasStartedMoving()
    {
        hasStartedMoving = false;
        isGameOver = false; // Reset game over flag
        score = 0;
        scoreText.text = score.ToString();
        scoreTimer = 0f;
        gameoverscoreText.text = score.ToString();
        Debug.Log("Score and game state reset");
    }

    public void SaveBestScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            bestScoreText.text = bestScore.ToString();
        }
    }
}