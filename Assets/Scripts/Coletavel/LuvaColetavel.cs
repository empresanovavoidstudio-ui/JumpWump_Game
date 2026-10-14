
using UnityEngine;

public class LuvaColetavel : MonoBehaviour
{
    private const string CHAVE_WUMP = "WumpDesbloqueado";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerPrefs.SetInt(CHAVE_WUMP, 1);
        PlayerPrefs.Save();
        Destroy(gameObject);
    }
}
