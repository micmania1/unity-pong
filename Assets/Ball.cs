using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float startSpeed = 6;

    public float speed = 6;

    float radius;
    public Vector2 direction;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        ResetDirection();
        speed = startSpeed;
        radius = transform.localScale.x / 2;

        // Create cached version of our game manager so we don't have to look it up mid-game
        gameManager = (GameManager)FindObjectOfType(typeof(GameManager));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        // Bounce off top & bottom
        if (transform.position.y < GameManager.bottomLeft.y + radius && direction.y < 0) {
            direction.y = -direction.y;
        }
        if (transform.position.y > GameManager.topRight.y - radius && direction.y > 0) {
            direction.y = -direction.y;
        }

        // Score a goal against left
        if (transform.position.x < GameManager.bottomLeft.x - radius && direction.x < 0) {
            gameManager.UpdateRightScore();
            ResetDirection();
            transform.position = Vector2.zero;
            speed = startSpeed;
        }
        // Score a goal against right
        if (transform.position.x > GameManager.topRight.x + radius && direction.x > 0) {
            gameManager.UpdateLeftScore();
            ResetDirection();
            transform.position = Vector2.zero;
            speed = startSpeed;
        }
    }

    void ResetDirection()
    {

        float[] options = { -1f, 1f };
        var list = new List<float>(options);

        System.Random random = new System.Random();
        int x = random.Next(list.Count);
        int y = random.Next(list.Count);

        direction = new Vector2(list[x], list[y]);
    }

    void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.tag == "Paddle") {
            bool isRight = otherObject.GetComponent<Paddle>().isRight;

            if (isRight && direction.x > 0) {
                direction.x = -direction.x;
            }
            if (!isRight && direction.x < 0) {
                direction.x = -direction.x;
            }

            speed++;
        }

        if (otherObject.tag == "PaddleAI") {
            if (direction.x < 0) {
                direction.x = -direction.x;
            }

            speed++;
        }
    }
}
