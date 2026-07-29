using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 8f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;
    public bool canmove = true;　//Playerをアタッチする

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    [SerializeField, Range(0f, 1f)]
    private float jumpCutMultiplier = 0.46f;
    public bool isGround = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canmove)//ゴールした時にPlayerの動きを止める
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetFloat("Speed", 0f);
            return;
        }
        isGround = Physics2D.OverlapCircle(
     groundCheck.position,
     groundCheckRadius,
     groundLayer
 );

        // 左右移動
        float x = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            x = -1;
            sr.flipX = false;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            x = 1;
            sr.flipX = true;
        }

        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

        animator.SetFloat("Speed", Mathf.Abs(x));

        // ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && isGround)//地面についている時にスペースでジャンプできる
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isGround = false;
        }
        //ジャンプキーを早く離したら上昇を弱める
        if(Input.GetKeyUp(KeyCode.Space) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * jumpCutMultiplier
                );
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
}
