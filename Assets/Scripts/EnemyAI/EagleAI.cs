using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 respawnPosition;
    public bool isUp = true;
    public float topY, bottomY;
    public float speed;
    public Transform topPoint, bottomPoint;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPosition = transform.position;

        topY = topPoint.position.y;
        bottomY = bottomPoint.position.y;
        Destroy(topPoint.gameObject);
        Destroy(bottomPoint.gameObject);
    }

    void Update()
    {
        Movement();
        RespawnJudge(respawnPosition);
    }

    void Movement()
    {
        if(isUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
            if (transform.position.y > topY)
            {
                isUp = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);
            if (transform.position.y < bottomY)
            {
                isUp = true;
            }
        }

    }

    void RespawnJudge(Vector3 respawnPosition)
    {
        if(Game.getGameOver())
        {
            Game.Respawn(this.gameObject,respawnPosition);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            isUp = true;
        }
    }
}
