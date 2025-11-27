using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonSalirExamen : MonoBehaviour
{
    public void IrANiveles()
    {
        // Limpia los aciertos, por si quieres reiniciar
        PlayerPrefs.SetInt("aciertosTotal", 0);

        // Cargar la escena "Niveles"
        SceneManager.LoadScene("Niveles");
    }
}
