
using UnityEngine;

public static class ProgressoWump
{
    private const string CHAVE = "WumpDesbloqueado";

    public static bool Desbloqueado
    {
        get { return PlayerPrefs.GetInt(CHAVE, 0) == 1; }
    }

    public static void Desbloquear()
    {
        PlayerPrefs.SetInt(CHAVE, 1);
        PlayerPrefs.Save();
    }

    public static void BloquearParaNovoJogo()
    {
        PlayerPrefs.DeleteKey(CHAVE);
        PlayerPrefs.Save();
    }
}
