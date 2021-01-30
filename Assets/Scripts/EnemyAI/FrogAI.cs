using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator animator;
    private bool faceLeft = true;
    private float leftX, rightX;
    private Vector3 respawnPosition;
    public Transform leftPoint, rightPoint;
    public float speed, jumpForce;
    public LayerMask ground;


    void Start()
    {
        // 初始化必要对象
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        respawnPosition = transform.position;

        // 获得左右点的坐标，并销毁点来释放资源
        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    void Update()
    {
        SwitchAnime();
        RespawnJudge(respawnPosition);
    }

    // 跳跃，动画事件使用，设置在转身之后
    void Jump()
    {
        if (!animator.GetBool("isJumping") && !animator.GetBool("isFalling"))
        {
            if (faceLeft)
            {
                rb.velocity = new Vector2(-speed, jumpForce);
            }
            else
            {
                rb.velocity = new Vector2(speed, jumpForce);
            }
            animator.SetBool("isJumping", true);
        }

    }
    // 判断跳跃的方向，转身，动画事件使用，设置在跳跃之前
    void JudgeDirection()
    {
        if (transform.position.x < leftX)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            faceLeft = false;
        }
        if (transform.position.x > rightX)
        {
            transform.localScale = new Vector3(1, 1, 1);
            faceLeft = true;
        }
    }

    // 动画器控制
    void SwitchAnime()
    {
        // 状态改变
        animator.SetBool("isIdle", false);
        // 跳跃
        if (animator.GetBool("isJumping"))
        {
            if (rb.velocity.y < 0)
            {
                // 跳跃中y速度小于零激活下落动画
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", true);
            }
        }

        if (coll.IsTouchingLayers(ground)) // 落地时回到idle动画
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("isIdle", true);
        }
        // 速度小于0且不接触地面时激活下落动画
        if(rb.velocity.y < 0 && !coll.IsTouchingLayers(ground))
        {
            animator.SetBool("isFalling", true);
        }
        // 转身动画切换
    }
    void RespawnJudge(Vector3 respawnPosition)
    {
        if(Game.getGameOver()){
            Game.Respawn(this.gameObject,respawnPosition);
        }
    }
}
