using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Pulo")]
    public float forcaPulo = 12f;

    [Header("Assistência")]
    public float coyoteTime = 0.15f;
    public float jumpBuffer = 0.15f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    private float coyoteCounter;
    private float bufferCounter;

    private bool grounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // verifica se está no chão
        grounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer);

        // coyote time
        if (grounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;

        // jump buffer
        if (Input.GetKeyDown(KeyCode.Space))
            bufferCounter = jumpBuffer;
        else
            bufferCounter -= Time.deltaTime;

        // pulo
        if (bufferCounter > 0f && coyoteCounter > 0f && grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);

            bufferCounter = 0f;
            coyoteCounter = 0f;

            grounded = false; // faz com que não seja permitido o jogador pular várias vezes
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}