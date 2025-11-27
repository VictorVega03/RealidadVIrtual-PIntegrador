using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonSalir : MonoBehaviour
{
    public void SalirAlMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
