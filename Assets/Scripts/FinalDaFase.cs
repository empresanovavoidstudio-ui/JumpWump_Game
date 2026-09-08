using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDaFase : MonoBehaviour
{
    public int numeroDaFase;

    public void CompletarFase()
    {
        int faseDesbloqueada = PlayerPrefs.GetInt("FaseDesbloqueada", 1);

        
        if (numeroDaFase >= faseDesbloqueada)
        {
            PlayerPrefs.SetInt("FaseDesbloqueada", numeroDaFase + 1);
            PlayerPrefs.Save();
        }

        
        SceneManager.LoadScene("SelecaoFases");
    }
}