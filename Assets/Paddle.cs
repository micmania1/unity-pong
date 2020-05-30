using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField]
    float speed = 25f;
    float height;

    string input;
    public bool isRight;
    public Joystick joystick;

    // Start is called before the first frame update
    void Start()
    {
        height = transform.localScale.y;

        if (isRight) {
            Vector2 pos = new Vector2(GameManager.topRight.x, 0);
            pos -= Vector2.right * transform.localScale.x;
            transform.position = pos;

            input = "PaddleRight";
        } else {
            Vector2 pos = new Vector2(GameManager.bottomLeft.x, 0);
            pos += Vector2.right * transform.localScale.x;
            transform.position = pos;

            input = "PaddleLeft";
        }
    }

    public void SetEnabled(bool enabled) => transform.gameObject.SetActive(enabled);

    // Update is called once per frame
    void Update()
    {
        float direction = Input.GetAxis(input);
        if (joystick && direction == 0) {
            direction = joystick.Vertical;
        }
        float move = direction * Time.deltaTime * speed;

        float halfHeight = height / 2;
        if (move < 0 && transform.position.y < GameManager.bottomLeft.y + halfHeight) {
            move = 0;
        }
        if (move > 0 && transform.position.y > GameManager.topRight.y - halfHeight) {
            move = 0;
        }

        transform.Translate(move * Vector2.up);
    }
}
