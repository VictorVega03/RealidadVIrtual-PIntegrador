using UnityEngine;
using System.Collections;

public class TrashCan : MonoBehaviour
{
    [Header("Indicador de Resta")]
    public GameObject minusIndicatorPrefab;
    public float indicatorOffset = 1.5f;

    [Header("Audio")]
    public AudioSource trashAudio; // Sonido al tirar manzana
    public AudioSource victoryAudio; // Sonido de victoria

    private static bool levelCompleteSoundPlayed = false;

    void Start()
    {
        levelCompleteSoundPlayed = false; // Resetear al inicio
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WormApple"))
        {
            Debug.Log("¡Manzana mala eliminada!");
            
            // Mostrar indicador de resta
            ShowMinusIndicator(transform.position);
            
            // Reproducir sonido de tirar basura
            if (trashAudio != null)
            {
                trashAudio.Play();
            }
            
            // Reducir contador
            GameConfig.badApplesRemaining--;
            
            // Destruir la manzana
            Destroy(other.gameObject);
            
            // Verificar si terminó el nivel
            if (GameConfig.badApplesRemaining <= 0 && !levelCompleteSoundPlayed)
            {
                levelCompleteSoundPlayed = true;
                Debug.Log("¡Nivel 2 completado!");
                
                // Detener música de fondo y reproducir victoria
                StartCoroutine(PlayVictorySequence());
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
    
    // Secuencia de victoria
    IEnumerator PlayVictorySequence()
    {
        // Detener música de fondo con fade out
        yield return StartCoroutine(FadeOutBackgroundMusic(1.0f));
        
        // Pequeña pausa
        yield return new WaitForSeconds(0.2f);
        
        // Reproducir sonido de victoria
        if (victoryAudio != null && victoryAudio.clip != null)
        {
            victoryAudio.Play();
            Debug.Log("¡Victoria en Nivel 2!");
        }
    }
    
    // Fade out de música
    IEnumerator FadeOutBackgroundMusic(float duration)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        
        if (gameManager != null && gameManager.backgroundMusic != null)
        {
            AudioSource bgMusic = gameManager.backgroundMusic;
            float startVolume = bgMusic.volume;
            float elapsedTime = 0f;
            
            Debug.Log("Haciendo fade out de música de fondo...");
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                bgMusic.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / duration);
                yield return null;
            }
            
            bgMusic.volume = 0f;
            bgMusic.Stop();
            
            Debug.Log("Música de fondo detenida");
        }
    }
}