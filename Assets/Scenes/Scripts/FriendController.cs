using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FriendController : MonoBehaviour
{
    [Header("Configuración")]
    public int maxApples = 4;
    private int currentApples = 0;
    
    [Header("UI")]
    public Text appleCountText;
    
    [Header("Audio")]
    public AudioSource friendAudio;
    public AudioSource victoryAudio; // Sonido de victoria
    
    [Header("Indicador")]
    public GameObject divisionIndicatorPrefab;
    public float indicatorOffset = 1.5f;
    
    [Header("Animación de Desaparición")]
    public float fadeOutDuration = 1f;
    public float delayBeforeFade = 0.5f;
    
    private SpriteRenderer spriteRenderer;
    private bool isFull = false;
    private static bool levelCompleteSoundPlayed = false; // reproducir solo una vez

    void Start()
    {
        UpdateUI();
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelCompleteSoundPlayed = false; // Resetear al inicio
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Apple") && currentApples < maxApples)
        {
            currentApples++;
            UpdateUI();
            
            ShowDivisionIndicator();
            
            if (friendAudio != null)
            {
                friendAudio.Play();
            }
            
            Destroy(other.gameObject);
            
            if (currentApples >= maxApples && !isFull)
            {
                isFull = true;
                StartCoroutine(FadeOutAndDisappear());
            }
            
            CheckLevelCompletion();
        }
    }

    void UpdateUI()
    {
        if (appleCountText != null)
        {
            appleCountText.text = currentApples + "/" + maxApples;
        }
    }

    void ShowDivisionIndicator()
    {
        if (divisionIndicatorPrefab != null)
        {
            Vector3 indicatorPosition = transform.position;
            indicatorPosition.y += indicatorOffset;
            Instantiate(divisionIndicatorPrefab, indicatorPosition, Quaternion.identity);
        }
    }

    IEnumerator FadeOutAndDisappear()
    {
        yield return new WaitForSeconds(delayBeforeFade);
        
        if (spriteRenderer != null)
        {
            float elapsedTime = 0f;
            Color originalColor = spriteRenderer.color;
            
            while (elapsedTime < fadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
            
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        }
        
        if (appleCountText != null)
        {
            appleCountText.gameObject.SetActive(false);
        }
        
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        
        Debug.Log($"{gameObject.name} ha desaparecido - está lleno!");
    }

    void CheckLevelCompletion()
    {
        FriendController[] allFriends = FindObjectsOfType<FriendController>();
        
        bool allComplete = true;
        foreach (FriendController friend in allFriends)
        {
            if (friend.gameObject.activeInHierarchy && friend.currentApples < friend.maxApples)
            {
                allComplete = false;
                break;
            }
        }

        if (allComplete && !levelCompleteSoundPlayed)
        {
            levelCompleteSoundPlayed = true;
            Debug.Log("¡Nivel 4 completado! Todas las pizzas repartidas equitativamente");
            
            // Fade out de música de fondo
            StartCoroutine(FadeOutBackgroundMusic(1.0f)); // 1 segundo de fade
            
            // Reproducir sonido de victoria
            PlayVictorySound();
        }
    }

    // Fade out gradual de la música
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
    
    // Método para reproducir el sonido de victoria
    void PlayVictorySound()
    {
        if (victoryAudio != null && victoryAudio.clip != null)
        {
            victoryAudio.Play();
            Debug.Log("Reproduciendo sonido de victoria!");
        }
        else
        {
            Debug.LogWarning("Victory Audio no está configurado");
        }
    }
}