using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleAI : MonoBehaviour
{
    [SerializeField]
    float speed = 15f;
    float acceleration = 1f;
    float directionToMove = 0f;
    float height;
    public bool isRight = false;
    public Ball ball;

    // Start is called before the first frame update
    void Start()
    {
        height = transform.localScale.y;

        Vector2 pos = new Vector2(GameManager.bottomLeft.x, 0);
        pos += Vector2.right * transform.localScale.x;
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        // The ball is heading towards the AI
        if (ball.direction.x < 0 && ball.transform.position.x < 0) {
            MovementTowardsBall();
        } else {
            MovementTowardsCenter();
        }

        float move = directionToMove * Time.deltaTime * speed;

        if (transform.position.y < GameManager.bottomLeft.y + height / 2 && move < 0) {
            move = 0;
            directionToMove = 0;
        }
        if (transform.position.y > GameManager.topRight.y - height / 2 && move > 0) {
            move = 0;
            directionToMove = 0;
        }

        transform.Translate(move * Vector2.up);
    }

    public void SetEnabled(bool enabled) => transform.gameObject.SetActive(enabled);

    void MovementTowardsY(float y, float currentAcceleration, float maxAcceleration)
    {
        // The ball is below the paddle, move down.
        if (transform.position.y > y) {
            if (directionToMove > 0) {
                directionToMove = 0;
            }

            // Add acceleration
            directionToMove -= currentAcceleration;

            // Ensure we're not going too fast
            if (directionToMove < -maxAcceleration) {
                directionToMove = -maxAcceleration;
            }
        }
        
        // The ball is above the paddle, move up.
        if (transform.position.y < y) {
            if (directionToMove < 0) {
                directionToMove = 0;
            }

            // Add acceleration
            directionToMove += currentAcceleration;

            // Ensure we're not going too fast
            if (directionToMove > maxAcceleration) {
                directionToMove = maxAcceleration;
            }
        }
    }

    void MovementTowardsCenter()
    {
        MovementTowardsY(0, 0.2f, 0.5f);
    }

    void MovementTowardsBall()
    {
        MovementTowardsY(ball.transform.position.y, acceleration, 1f);
    }
}
