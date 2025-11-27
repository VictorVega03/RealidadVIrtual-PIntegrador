using UnityEngine;
using UnityEngine.SceneManagement;

public class VolverMenu : MonoBehaviour
{
    // Cambia a una escena específica (si la necesitas)
    public void IrAEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    // Regresar a la escena Niveles
    public void IrANiveles()
    {
        SceneManager.LoadScene("Niveles");
    }

    // Ir desde Niveles al Menu
    public void IrAMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
