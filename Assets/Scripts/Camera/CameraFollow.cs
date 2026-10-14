using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Movimento")]
    public float smoothSpeed = 5f;
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

    [Header("Shake")]
    public float shakeIntensity = 0.15f;
    public float shakeDuration = 0.1f;

    private Rigidbody2D playerRb;
    private Vector3 velocity = Vector3.zero;

    private bool finalDaFase = false;
    private Transform objetivoFinal;

    void Start()
    {
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
        }
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        Vector3 posicaoDesejada = transform.position;

        
        // FINAL DA FASE
        

        if (finalDaFase && objetivoFinal != null)
        {
            Vector3 meio = (player.position + objetivoFinal.position) / 2f;

            posicaoDesejada.x = meio.x;
            posicaoDesejada.y = meio.y;
        }
        else
        {
            
            // MOVIMENTO HORIZONTAL
            

            float diferencaX = player.position.x - transform.position.x;

            if (Mathf.Abs(diferencaX) > deadZoneX)
            {
                float direcao = 0;

                if (playerRb != null)
                {
                    direcao = Mathf.Sign(playerRb.linearVelocity.x);
                }

                posicaoDesejada.x = player.position.x + (direcao * lookAhead);
            }

            
            // MOVIMENTO VERTICAL
            

            float diferencaY = player.position.y - transform.position.y;

            if (Mathf.Abs(diferencaY) > deadZoneY)
            {
                posicaoDesejada.y = player.position.y;
            }

            
            // QUEDA
           

            if (playerRb != null && playerRb.linearVelocity.y < 0)
            {
                posicaoDesejada.y += fallOffset;
            }
        }

        
        // LIMITES
        

        Camera cam = GetComponent<Camera>();

        float altura = cam.orthographicSize;
        float largura = altura * cam.aspect;

        posicaoDesejada.x = Mathf.Clamp(
            posicaoDesejada.x,
            limiteEsquerdo + largura,
            limiteDireito - largura
        );

        posicaoDesejada.y = Mathf.Clamp(
            posicaoDesejada.y,
            limiteInferior + altura,
            limiteSuperior - altura
        );

       
        // FOLLOW SUAVE
        

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

   
    // SHAKE
    

    public void Shake()
    {
        StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera()
    {
        Vector3 posicaoOriginal = transform.localPosition;

        float tempo = 0;

        while (tempo < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;

            transform.localPosition = new Vector3(
                posicaoOriginal.x + x,
                posicaoOriginal.y + y,
                posicaoOriginal.z
            );

            tempo += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = posicaoOriginal;
    }

    
    // FREEZE FRAME
    

    public IEnumerator FreezeFrame(float tempo)
    {
        Time.timeScale = 0.05f;

        yield return new WaitForSecondsRealtime(tempo);

        Time.timeScale = 1f;
    }

    
    // FINAL DA FASE
    

    public void MostrarFinal(Transform objetivo)
    {
        objetivoFinal = objetivo;
        finalDaFase = true;
    }
}