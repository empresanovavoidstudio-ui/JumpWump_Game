
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Wump : MonoBehaviour
{
    [Header("Movimento")]
    public float jump = 12f;
    public float speed = 10f;

    [Header("Dash")]
    public float dashTime = 0.2f;
    public float dashPower = 24f;
    public float dashCooldown = 1f;

    [Header("WUMP")]
    public float wumpPower = 20f;
    public float tempoWump = 0.8f;
    public float tempoRecargaWump = 0.5f;

    [Header("Gravidade")]
    public float gravidadeNormal = 3f;

    [Header("Verificação do chão")]
    [SerializeField] private Transform verificadorDeChao;
    [SerializeField] private float raioChao = 0.15f;
    [SerializeField] private LayerMask camadasDeChao;

    [Header("Animação")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private bool spriteOlhaParaDireita = true;

    [Header("Efeito do Dash")]
    [SerializeField] private TrailRenderer tr;

    private Rigidbody2D physicsPlayer;

    private float movimentoHorizontal;
    private bool isGrounded;
    private bool isDashing;
    private bool canDash = true;
    private bool isWumping;
    private bool canWump = true;
    private bool controleAtivo = true;

    private void Awake()
    {
        physicsPlayer = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (sprite == null)
            sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        physicsPlayer.gravityScale = gravidadeNormal;

        if (tr != null)
            tr.emitting = false;

        AtualizarAnimacao();
    }

    private void Update()
    {
        VerificarChao();

        if (!controleAtivo)
        {
            movimentoHorizontal = 0f;
            AtualizarAnimacao();
            return;
        }

        movimentoHorizontal = Input.GetAxisRaw("Horizontal");

        AtualizarDirecao();
        AtualizarAnimacao();

        if (Input.GetKeyDown(KeyCode.Space))
            JumpPlayer();

        if (Input.GetKeyDown(KeyCode.LeftShift)
            && canDash && !isDashing && !isWumping)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.S)
            && ProgressoWump.Desbloqueado
            && canWump && !isGrounded && !isDashing)
        {
            StartCoroutine(WumpAttack());
        }
    }

    private void FixedUpdate()
    {
        if (!controleAtivo || isDashing || isWumping)
            return;

        Vector2 velocidade = physicsPlayer.linearVelocity;
        velocidade.x = movimentoHorizontal * speed;
        physicsPlayer.linearVelocity = velocidade;
    }

    private void AtualizarDirecao()
    {
        if (sprite == null || Mathf.Abs(movimentoHorizontal) < 0.01f)
            return;

        bool indoParaDireita = movimentoHorizontal < 0;

        // Ajusta o FlipX considerando a direção original do sprite.
        sprite.flipX = spriteOlhaParaDireita
            ? !indoParaDireita
            : indoParaDireita;
    }

    private void AtualizarAnimacao()
    {
        if (animator == null)
            return;

        bool andando =
            controleAtivo &&
            Mathf.Abs(movimentoHorizontal) > 0.01f &&
            isGrounded &&
            !isDashing &&
            !isWumping;

        animator.SetBool("isWalking", andando);
    }

    private void VerificarChao()
    {
        if (verificadorDeChao == null)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics2D.OverlapCircle(
            verificadorDeChao.position,
            raioChao,
            camadasDeChao
        ) != null;
    }

    public void DefinirControle(bool ativo)
    {
        controleAtivo = ativo;
        movimentoHorizontal = 0f;

        if (physicsPlayer != null)
            physicsPlayer.linearVelocity = Vector2.zero;

        AtualizarAnimacao();
    }

    private void JumpPlayer()
    {
        if (!isGrounded || isDashing || isWumping)
            return;

        physicsPlayer.linearVelocity =
            new Vector2(physicsPlayer.linearVelocity.x, jump);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float gravidadeAnterior = physicsPlayer.gravityScale;
        physicsPlayer.gravityScale = 0f;

        float direcao = movimentoHorizontal;

        if (Mathf.Abs(direcao) < 0.01f)
        {
            direcao = sprite != null && sprite.flipX
                ? (spriteOlhaParaDireita ? -1f : 1f)
                : (spriteOlhaParaDireita ? 1f : -1f);
        }

        if (tr != null)
            tr.emitting = true;

        physicsPlayer.linearVelocity =
            new Vector2(direcao * dashPower, 0f);

        AtualizarAnimacao();

        yield return new WaitForSeconds(dashTime);

        if (tr != null)
            tr.emitting = false;

        physicsPlayer.gravityScale = gravidadeAnterior;
        physicsPlayer.linearVelocity = Vector2.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator WumpAttack()
    {
        canWump = false;
        isWumping = true;

        float gravidadeAnterior = physicsPlayer.gravityScale;
        physicsPlayer.gravityScale = 0f;
        physicsPlayer.linearVelocity = Vector2.down * wumpPower;

        AtualizarAnimacao();

        yield return new WaitForSeconds(tempoWump);

        if (isWumping)
        {
            physicsPlayer.gravityScale = gravidadeAnterior;
            isWumping = false;
        }

        yield return new WaitForSeconds(tempoRecargaWump);
        canWump = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isWumping)
            return;

        foreach (ContactPoint2D contato in collision.contacts)
        {
            if (contato.normal.y > 0.5f)
            {
                isWumping = false;
                physicsPlayer.gravityScale = gravidadeNormal;
                physicsPlayer.linearVelocity = Vector2.zero;

                WumpReceiver receptor =
                    collision.gameObject.GetComponent<WumpReceiver>();

                if (receptor != null)
                    receptor.ReceberWump();

                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (verificadorDeChao == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            verificadorDeChao.position,
            raioChao
        );
    }
}
