using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Alvo")]
    public Transform player;

    [Header("Follow")]
    public float smoothSpeed = 5f;

    [Header("Antecipação")]
    public float lookAhead = 2f;

    [Header("Dead Zone")]
    public float deadZoneX = 2f;
    public float deadZoneY = 1f;

    [Header("Queda")]
    public float fallOffset = -1.5f;

    [Header("Limites da fase")]
    public float limiteEsquerdo;
    public float limiteDireito;
    public float limiteInferior;
    public float limiteSuperior;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (player == null)
            return;

        Vector3 posicaoDesejada = transform.position;

        
        // MOVIMENTO HORIZONTAL
        

        float diferencaX = player.position.x - transform.position.x;

        if (Mathf.Abs(diferencaX) > deadZoneX)
        {
            posicaoDesejada.x = player.position.x + lookAhead;
        }

        
        // MOVIMENTO VERTICAL
        

        float diferencaY = player.position.y - transform.position.y;

        if (Mathf.Abs(diferencaY) > deadZoneY)
        {
            posicaoDesejada.y = player.position.y;
        }

        // Se estiver caindo, mostra um pouco mais abaixo
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb != null && rb.linearVelocity.y < 0)
        {
            posicaoDesejada.y += fallOffset;
        }

        
        // LIMITES
        

        posicaoDesejada.x = Mathf.Clamp(
            posicaoDesejada.x,
            limiteEsquerdo,
            limiteDireito
        );

        posicaoDesejada.y = Mathf.Clamp(
            posicaoDesejada.y,
            limiteInferior,
            limiteSuperior
        );

        
        // MOVIMENTO SUAVE
        

        Vector3 novaPosicao = Vector3.SmoothDamp(
            transform.position,
            posicaoDesejada,
            ref velocity,
            1f / smoothSpeed
        );

        transform.position = new Vector3(
            novaPosicao.x,
            novaPosicao.y,
            transform.position.z
        );
    }
}