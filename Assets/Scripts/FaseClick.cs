using UnityEngine;
using UnityEngine.SceneManagement;

public class FaseClick : MonoBehaviour
{
    public int numeroDaFase;
    public bool eTutorial;

    private void OnMouseDown()
    {
        
        if (eTutorial)
        {
            SceneManager.LoadScene("FaseTutorial");
            return;
        }

        
        int faseDesbloqueada = PlayerPrefs.GetInt("FaseDesbloqueada", 1);

        
        if (numeroDaFase <= faseDesbloqueada)
        {
            SceneManager.LoadScene("Level " + numeroDaFase);
        }
        else
        {
            Debug.Log("Essa fase está bloqueada!");
        }
    }
}