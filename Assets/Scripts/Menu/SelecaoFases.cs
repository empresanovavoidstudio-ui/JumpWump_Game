using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelecaoFases : MonoBehaviour
{
    public Button[] botoesFases;

    void Start()
    {
        int faseDesbloqueada = PlayerPrefs.GetInt("FaseDesbloqueada", 1);

        for (int i = 0; i < botoesFases.Length; i++)
        {
            int numeroDaFase = i + 1;

            if (numeroDaFase <= faseDesbloqueada)
            {
                botoesFases[i].interactable = true;
            }
            else
            {
                botoesFases[i].interactable = false;
            }
        }
    }

    public void AbrirFase(int numeroDaFase)
    {
        SceneManager.LoadScene("Level " + numeroDaFase);
    }
}