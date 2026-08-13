using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoad : MonoBehaviour
{

    void Start()
    {
        Invoke ("UseScene", 2f);
    }
    public void UseScene()
    {
        int loadedScene = PlayerPrefs.GetInt("faseSalva");
        SceneManager.LoadScene(loadedScene);
    }
}
