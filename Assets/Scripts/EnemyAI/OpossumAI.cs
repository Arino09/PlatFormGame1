using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool faceLeft = true;
    private float leftX, rightX;
    private Vector3 respawnPosition;
    public float speed;
    public Transform leftPoint, rightPoint;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPosition = transform.position;

        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    void Update()
    {
        Movement();
        RespawnJudge(respawnPosition);
    }

    void Movement()
    {
        if(faceLeft)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (transform.position.x < leftX)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                faceLeft = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (transform.position.x > rightX)
            {
                transform.localScale = new Vector3(1, 1, 1);
                faceLeft = true;
            }
        }
    }
    void RespawnJudge(Vector3 respawnPosition)
    {
        if(Game.getGameOver()){
            Game.Respawn(this.gameObject, respawnPosition);
        }
    }
}
