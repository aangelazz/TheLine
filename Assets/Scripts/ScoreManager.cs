// Purpose: To keep track of the player's score and update the score text on the screen.
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    void Update()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
    }
}