using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManagement : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("Stages");
    }
}