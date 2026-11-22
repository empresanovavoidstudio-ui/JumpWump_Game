using UnityEngine;

public class AreaMorte : MonoBehaviour
{
    [SerializeField] private CutsceneMorte cutscene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        cutscene.IniciarMorte(other.transform);
    }
}