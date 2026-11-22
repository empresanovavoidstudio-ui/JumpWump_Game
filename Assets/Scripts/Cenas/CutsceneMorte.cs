using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneMorte : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Animator animatorMorte;
    [SerializeField] private GameObject luzMorte;
    [SerializeField] private Camera cameraPrincipal;

    [Header("Configurações")]
    [SerializeField] private float tempoAteCutscene = 1.5f;
    [SerializeField] private float tempoReinicio = 3f;

    private bool iniciada;
    private Transform jogador;

    public void IniciarMorte(Transform player)
    {
        if (iniciada) return;

        iniciada = true;
        CameraFollow cameraFollow = cameraPrincipal.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        cameraFollow.PararDeSeguir();
        jogador = player;

        Wump controle = jogador.GetComponent<Wump>();

        if (controle != null)
            controle.DefinirControle(false);

        // O jogador continua caindo pela física
        Invoke(nameof(IniciarEfeito), tempoAteCutscene);
    }

    private void IniciarEfeito()
    {
        Rigidbody2D rb = jogador.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        CameraFollow cameraFollow = cameraPrincipal.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        cameraFollow.FocarNaPosicao(jogador.position);

        // Ativa o efeito de iluminação
        luzMorte.SetActive(true);

        animatorMorte.Play("AnimacaoMorte", 0, 0f);

        Invoke(nameof(ReiniciarFase), tempoReinicio);
    }

    private void ReiniciarFase()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}