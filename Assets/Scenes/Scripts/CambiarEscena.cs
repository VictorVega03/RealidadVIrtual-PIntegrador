using UnityEngine;
using UnityEngine.SceneManagement; // Manejar escenas

public class CambiarEscena : MonoBehaviour
{
    public string nombreEscena; // Escribe el nombre exacto de la escena en el Inspector

    public void CargarEscena()
    {
        SceneManager.LoadScene(nombreEscena);
    }
}
