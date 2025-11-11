using UnityEngine;

public class ScorePopUp : MonoBehaviour
{
    public float lifetime = 0.8f; // Tiempo que permanece visible (en segundos)
    public float moveSpeed = 1f;  // Opcional: Para que el + suba un poco

    void Start()
    {
        // Destruye el objeto después del tiempo de vida
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Opcional: Hace que el objeto se mueva ligeramente hacia arriba
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }
}