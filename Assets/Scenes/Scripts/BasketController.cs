using UnityEngine;

public class BasketController : MonoBehaviour
{
    public float moveSpeed = 7f; // Ajusta la velocidad

    [Header("Límites de Movimiento")]
    public float minX = -8f; // Límite izquierdo
    public float maxX = 8f;  // Límite derecho

    [Header("Indicador de Puntuación")]
    // ✨ NUEVO: Arrastra el Prefab del '+' aquí en el Inspector
    public GameObject scoreIndicatorPrefab;
    public float indicatorOffset = 1.0f; // Distancia vertical por encima de la canasta

    [Header("Audio")]
    // ✨ NUEVO: Asigna el Audio Source de la canasta aquí en el Inspector.
    public AudioSource basketAudio;

    void Update()
    {
        // Obtiene la entrada horizontal (teclas A/D o flechas izquierda/derecha)
        float horizontalInput = Input.GetAxis("Horizontal");

       if (horizontalInput != 0)
        {
            // Calcula el movimiento
            Vector3 movement = new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0, 0);

            // Aplica el movimiento a la posición
            transform.position += movement;

            // Limita la posición X
            Vector3 currentPosition = transform.position;
            currentPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);
            transform.position = currentPosition;
        }
    }

    // Este método se llama cuando otro objeto con Collider2D (y Rigidbody2D)
    // entra en el Collider2D de la cesta, si el de la cesta es un 'Trigger'
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Apple")) // Manzana normal
        {
            Debug.Log("¡Manzana buena atrapada! Sumar puntos.");

            // ✨ LLAMAR A LA FUNCIÓN DE INDICADOR DE PUNTUACIÓN
            ShowScoreIndicator(transform.position);

            // ✨ REPRODUCIR SONIDO
            if (basketAudio != null && basketAudio.clip != null)
            {
                basketAudio.Play();
            }

            Destroy(other.gameObject);
            // Aquí agregarás la lógica para sumar puntos (e.g., incrementar contador/puntuación)
        }
        else if (other.CompareTag("WormApple")) // Manzana con gusano
        {
            Debug.Log("¡Manzana con gusano atrapada! No sumar puntos o restar.");
            Destroy(other.gameObject);
        }
    }
    
    // ✨ FUNCIÓN NUEVA: Genera el indicador de puntuación
    void ShowScoreIndicator(Vector3 spawnPosition)
    {
        if (scoreIndicatorPrefab != null)
        {
            // 1. Calcular la posición de aparición (encima de la canasta)
            Vector3 indicatorPosition = spawnPosition;
            indicatorPosition.y += indicatorOffset; // Subir el indicador el valor del Offset
            
            // 2. Generar el prefab en esa posición
            Instantiate(scoreIndicatorPrefab, indicatorPosition, Quaternion.identity);
        }
    }
}