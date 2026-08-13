using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoad : MonoBehaviour
{

    public SpriteRenderer[] vectorLevels;
    void Start()
    {
        UseScene();
        ChangeScene();
        Invoke ("UseScene", 2f);
    }
    public void UseScene()
    {
        int StageSelect = PlayerPrefs.GetInt("faseSalva");
        SceneManager.LoadScene(StageSelect);
        vectorLevels[StageSelect -1].color = Color.red;
    }
    public void ChangeScene()
    {
        int StageSelect = PlayerPrefs.GetInt("faseSalva");
        SceneManager.LoadScene("StageSelect");
    }
}
