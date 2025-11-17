using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [Header("Indicador de Resta")]
    public GameObject minusIndicatorPrefab; // Prefab del símbolo "-"
    public float indicatorOffset = 1.5f;

    [Header("Audio")]
    public AudioSource trashAudio;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WormApple"))
        {
            Debug.Log("¡Manzana mala eliminada!");
            
            // Mostrar indicador de resta
            ShowMinusIndicator(transform.position);
            
            // Reproducir sonido
            if (trashAudio != null)
            {
                trashAudio.Play();
            }
            
            // Reducir contador
            GameConfig.badApplesRemaining--;
            
            // Destruir la manzana
            Destroy(other.gameObject);
            
            // Verificar si terminó el nivel
            if (GameConfig.badApplesRemaining <= 0)
            {
                Debug.Log("¡Nivel 2 completado!");
                // Aquí puedes cargar el siguiente nivel o mostrar mensaje
            }
        }
    }

    void ShowMinusIndicator(Vector3 spawnPosition)
    {
        if (minusIndicatorPrefab != null)
        {
            Vector3 indicatorPosition = spawnPosition;
            indicatorPosition.y += indicatorOffset;
            Instantiate(minusIndicatorPrefab, indicatorPosition, Quaternion.identity);
        }
    }
}