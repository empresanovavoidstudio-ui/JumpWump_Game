using System.Collections;
using UnityEngine;

public class Wump : MonoBehaviour
{
    public float jump = 5f;
    public float speed = 7f;
    Rigidbody2D physicsPlayer;
    Vector2 move;
    bool isGrounded = true;
    private bool canDash = true;
    private bool isDashing = false;
    private float dashTime = 0.2f;
    private float dashPower = 24f;
    private float dashCooldown = 1f;

    [SerializeField] private TrailRenderer tr;

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = physicsPlayer.gravityScale;
        physicsPlayer.gravityScale = 0f;
        physicsPlayer.linearVelocity = new Vector2(transform.localScale.x * dashPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;
        physicsPlayer.gravityScale = originalGravity;
    }

    void Start()
    {
        physicsPlayer = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
       
    }
    void FixedUpdate()
    {

        

        //
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        move = new Vector2(moveX, moveY).normalized;
        physicsPlayer.linearVelocity = new Vector2(move.x * speed, physicsPlayer.linearVelocity.y);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpPlayer();
        }
    }
    void JumpPlayer()
    {
        if (isGrounded == true)
        {
            physicsPlayer.linearVelocity = new Vector2(physicsPlayer.linearVelocity.x, jump);
        }
    }
}