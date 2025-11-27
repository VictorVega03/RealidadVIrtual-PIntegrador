using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MostrarResultados : MonoBehaviour
{
    public TMP_Text textoResultados;

    void Start()
    {
        int aciertos = PlayerPrefs.GetInt("aciertosTotal", 0);
        textoResultados.text = "Aciertos: " + aciertos + " / 4";
    }

    public void Reiniciar()
    {
        PlayerPrefs.SetInt("aciertosTotal", 0);
        SceneManager.LoadScene("ExamSuma");
    }
}
