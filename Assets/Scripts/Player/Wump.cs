using System.Collections;
using UnityEngine;

public class Wump : MonoBehaviour
{
    [Header("Movimento")]
    public float jump = 8f;
    public float speed = 10f;

    private Rigidbody2D physicsPlayer;
    private Vector2 move;

    [Header("Animação")]
    private Animator animator;

    [Header("Estado")]
    private bool isGrounded = true;
    private bool isWumping = false;
    private bool canWump = true;

    [Header("Dash")]
    private bool canDash = true;
    private bool isDashing = false;
    private float dashTime = 0.2f;
    private float dashPower = 24f;
    private float dashCooldown = 1f;

    [Header("WUMP")]
    public float wumpPower = 20f;

    [Header("Efeito do Dash")]
    [SerializeField] private TrailRenderer tr;

    void Start()
    {
        physicsPlayer = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpPlayer();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.S) && canWump && !isGrounded && !isDashing)
        {
            StartCoroutine(WumpAttack());
        }
    }

    void FixedUpdate()
    {
        if (isDashing || isWumping)
        {
            animator.SetBool("isWalking", false);
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");

        move = new Vector2(moveX, 0).normalized;

        physicsPlayer.linearVelocity = new Vector2(
            move.x * speed,
            physicsPlayer.linearVelocity.y
        );

        // ANIMAÇÃO
        if (moveX != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        // VIRAR O PLAYER
        if (moveX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void JumpPlayer()
    {
        if (isGrounded && !isDashing && !isWumping)
        {
            physicsPlayer.linearVelocity = new Vector2(
                physicsPlayer.linearVelocity.x,
                jump
            );

            isGrounded = false;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        animator.SetBool("isWalking", false);

        float originalGravity = physicsPlayer.gravityScale;

        physicsPlayer.gravityScale = 0f;

        physicsPlayer.linearVelocity = new Vector2(
            transform.localScale.x * dashPower,
            0f
        );

        if (tr != null)
        {
            tr.emitting = true;
        }

        yield return new WaitForSeconds(dashTime);

        if (tr != null)
        {
            tr.emitting = false;
        }

        physicsPlayer.gravityScale = originalGravity;

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    private IEnumerator WumpAttack()
    {
        canWump = false;
        isWumping = true;

        animator.SetBool("isWalking", false);

        float originalGravity = physicsPlayer.gravityScale;

        physicsPlayer.gravityScale = 0f;

        physicsPlayer.linearVelocity = Vector2.down * wumpPower;

        yield return new WaitForSeconds(0.8f);

        if (isWumping)
        {
            physicsPlayer.gravityScale = originalGravity;
            isWumping = false;
        }

        yield return new WaitForSeconds(0.5f);

        canWump = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isWumping)
        {
            isWumping = false;

            physicsPlayer.gravityScale = 3f;

            WumpReceiver objetoWump =
                collision.gameObject.GetComponent<WumpReceiver>();

            if (objetoWump != null)
            {
                objetoWump.ReceberWump();
            }

            CameraFollow cameraFollow =
                Camera.main.GetComponent<CameraFollow>();

            if (cameraFollow != null)
            {
                cameraFollow.Shake();
            }
        }

        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}