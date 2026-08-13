using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSave : MonoBehaviour
{
    private void Start()
    {
        int numeroCena = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Número da cena atual: " + numeroCena);
        PlayerPrefs.SetInt("faseSalva", numeroCena);
        PlayerPrefs.Save();
    }
}
