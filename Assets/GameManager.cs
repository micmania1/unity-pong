using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    public enum GameState {
        Running,
        Paused,
        Finished,
    }

    public TextMeshProUGUI leftScoreText;
    public TextMeshProUGUI rightScoreText;
    public Canvas scoresUnderlay;
    public Canvas pausedOverlay;
    public Canvas gameOver;
    public TextMeshProUGUI winnerText;

    public static Vector2 bottomLeft;
    public static Vector2 topRight;

    GameState state = GameState.Running;

    bool isMultiPlayer = true;

    public int winningScore = 1;
    int leftScore = 0;
    int rightScore = 0;

    public Ball ball;
    public Paddle paddleLeft;
    public Joystick paddleLeftJoystick;
    public Paddle paddleRight;
    public Joystick paddleRightJoystick;

    public PaddleAI paddleLeftAI;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        isMultiPlayer = PlayerPrefs.GetInt("numPlayers") == 2;
        if (isMultiPlayer) {
            // Remove the left paddle and joystick
            paddleLeft.transform.gameObject.SetActive(false);
            paddleLeftJoystick.transform.gameObject.SetActive(false);

            // Since we only have one player, give them the full screen
            Vector3 newJoystickPos = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
            newJoystickPos.z = 0;
            paddleRightJoystick.transform.position = newJoystickPos;

            // Enable the AI
            paddleLeftAI.transform.gameObject.SetActive(true);
        }
    }

    public void Update()
    {
        if (state == GameState.Finished) {
            GameOver();
        } else if (Input.GetKeyUp(KeyCode.P)) {
            PauseGame();
        }
    }

    public void UpdateLeftScore()
    {
        leftScore++;
        leftScoreText.text = leftScore.ToString();

        if (leftScore >= winningScore) {
            state = GameState.Finished;

            if (isMultiPlayer) {
                winnerText.text = "Left player won!";
                winnerText.color = Color.green;
            } else {
                winnerText.text = "Close one!";
                winnerText.color = Color.red;
            }
        }
    }

    public void UpdateRightScore()
    {
        rightScore++;
        rightScoreText.text = rightScore.ToString();

        if (rightScore >= winningScore) {
            state = GameState.Finished;

            if (isMultiPlayer) {
                winnerText.text = "Right player won!";
                winnerText.color = Color.green;
            } else {
                winnerText.text = "You won!";
                winnerText.color = Color.green;
            }
        }
    }

    void GameOver()
    {
        pausedOverlay.transform.gameObject.SetActive(false);
        scoresUnderlay.transform.gameObject.SetActive(false);
        paddleLeft.transform.gameObject.SetActive(false);
        paddleRight.transform.gameObject.SetActive(false);
        ball.transform.gameObject.SetActive(false);
        gameOver.transform.gameObject.SetActive(true);

        Time.timeScale = 0f;
    }

    void PauseGame()
    {
        if (state == GameState.Running) {
            pausedOverlay.transform.gameObject.SetActive(true);
            leftScoreText.enabled = false;
            rightScoreText.enabled = false;

            state = GameState.Paused;
            Time.timeScale = 0.0f;
        } else {
            pausedOverlay.transform.gameObject.SetActive(false);
            leftScoreText.enabled = true;
            rightScoreText.enabled = true;

            state = GameState.Running;
            Time.timeScale = 1.0f;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Rematch()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
