using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidade = 5f;

    private Rigidbody2D rb;
    private float movimento;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // move para as teclas A/D ou Setas
        movimento = Input.GetAxisRaw("Horizontal");
       
        Vector3 escala = transform.localScale;
        if (movimento > 0)
        {
            escala.x = Mathf.Abs(escala.x);
        }
        else if (movimento < 0)
        {
            escala.x = -Mathf.Abs(escala.x);
        }
        transform.localScale = escala;
    }

    void FixedUpdate()
    {
        // Move na horizontal
        rb.linearVelocity = new Vector2(movimento * velocidade, rb.linearVelocity.y);
    }
}