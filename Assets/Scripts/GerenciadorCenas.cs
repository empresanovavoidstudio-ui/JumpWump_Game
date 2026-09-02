using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorCenas : MonoBehaviour
{

    public void CarregarCena(string nomeDaCena)
    {
        SceneManager.LoadScene(nomeDaCena);
    }

    public void IrParaMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void IrParaFase1()
    {
        SceneManager.LoadScene("Stage 1");
    }

    public void IrParaFase2()
    {
        SceneManager.LoadScene("Stage 2");
    }

    public void IrParaFase3()
    {
        SceneManager.LoadScene("Stage 3");
    }

    public void IrParaSelecaoFases()
    {
        SceneManager.LoadScene("Stages");
    }

    
    public void ProximaCena()
    {
        int indiceAtual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(indiceAtual + 1);
    }

    
    public void ReiniciarCena()
    {
        int indiceAtual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(indiceAtual);
    }

    
    public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }
}