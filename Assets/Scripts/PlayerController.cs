using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public GameObject deathFx;
    public AudioSource jumpSounds, cherrySounds, hurtSounds, deathSounds;
    public Vector3 respawnPosition;
    public float speed, jumpForce;  //速度和跳跃力
    public int cherry = 0;  //获取樱桃数量
    public Collider2D coll;
    public LayerMask ground;  //地面排序图层
    public Text cherryNumber;  //UI界面的樱桃数量

    // Start函数在游戏开始时被调用
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  //初始化刚体
        animator = GetComponent<Animator>();  //初始化动画器
        respawnPosition = transform.position;
    }

    // 每一帧更新一次
    void Update()
    {
        Movement();
        Jump();
        SwitchAnime();
        RespawnJudge(respawnPosition);
    }

    // 移动函数
    void Movement()
    {
        if (!animator.GetBool("isHurt"))  //不受伤时才可以移动
        {
            // 水平移动
            float horizontalMove = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
            animator.SetFloat("running", Mathf.Abs(horizontalMove));
            if (horizontalMove != 0)
            {
                transform.localScale = new Vector3(horizontalMove, 1, 1);
            }
        }
    }
    void Jump()
    {
        // 仅在不跳跃/下落时才可跳跃
        if (Input.GetButtonDown("Jump") && !animator.GetBool("isJumping") && !animator.GetBool("isFalling"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // 跳跃动作
            jumpSounds.Play(); // 跳跃音效
            animator.SetBool("isJumping", true); // 跳跃动画
        }
    }
    // 受伤反馈
    void Hurt(Collision2D collision)
    {
        animator.SetBool("isHurt", true); //激活受伤动画
        hurtSounds.Play();
        //在敌人左侧向左弹
        if(collision.gameObject.transform.position.x > transform.position.x)
        {
            rb.velocity = new Vector2(-2.5f, 10f);  //受伤反弹效果
        }
        else //否则向右弹
        {
            rb.velocity = new Vector2(2.5f, 10f);  //受伤反弹效果
        }
    }
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
    }

    // 收集樱桃
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Collections"))
        {
            Destroy(collision.gameObject);
            cherrySounds.Play();
            cherry++;
            cherryNumber.text = cherry.ToString();
        }
    }

    // 与敌人碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemies"))
        {
            // 下落且不是受伤状态中碰撞
            if (animator.GetBool("isFalling") && !animator.GetBool("isHurt"))
            {
                Transform enemyTrans = collision.gameObject.transform;
                //消灭敌人并生成死亡特效
                Instantiate(deathFx,enemyTrans.position,enemyTrans.rotation);
                Destroy(collision.gameObject);
                // 在敌人身上跳跃
                rb.velocity = new Vector2(rb.velocity.x, jumpForce*0.7f);
                animator.SetBool("isJumping", true);
            }
            else
            {
                //  不在下落中则受伤
                Hurt(collision);
            }
        }
        else
        {
            // 没有碰撞时将受伤状态置否
            animator.SetBool("isHurt", false);
        }
    }

    //  触发无敌时间
    void Eninvincible()
    {
        Physics.IgnoreLayerCollision(8,9,true);
    }
    //  取消无敌时间
    void Uninvincible()
    {
        Physics.IgnoreLayerCollision(9,8,false);
    }
    //  重生判断
    void RespawnJudge(Vector3 respawnPosition)
    {
        if (transform.position.y <= -7)
        {
            Game.setGameOver(true);
            deathSounds.Play();
        }
        if(Game.getGameOver()){
            transform.position = respawnPosition;
            rb.velocity = new Vector2(0,0);
            Game.setGameOver(false);
        }
    }
}
