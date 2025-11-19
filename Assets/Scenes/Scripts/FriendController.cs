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
    
    [Header("Indicador")]
    public GameObject divisionIndicatorPrefab;
    public float indicatorOffset = 1.5f;
    
    [Header("Animación de Desaparición")]
    public float fadeOutDuration = 1f; // Duración del fade out
    public float delayBeforeFade = 0.5f; // Espera antes de empezar a desaparecer
    
    private SpriteRenderer spriteRenderer;
    private bool isFull = false;

    void Start()
    {
        UpdateUI();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            
            // ✨ NUEVO: Si ya está lleno, hacer fade out
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

    //fade out gradual cuando el niño está lleno
    IEnumerator FadeOutAndDisappear()
    {
        // Esperar un poco antes de empezar a desaparecer
        yield return new WaitForSeconds(delayBeforeFade);
        
        if (spriteRenderer != null)
        {
            float elapsedTime = 0f;
            Color originalColor = spriteRenderer.color;
            
            // Fade out gradual
            while (elapsedTime < fadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
            
            // Asegurar que el alpha sea 0
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        }
        
        // También hacer fade out del texto si existe
        if (appleCountText != null)
        {
            appleCountText.gameObject.SetActive(false);
        }
        
        // Desactivar el collider para que no reciba más pizzas
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
            if (friend.currentApples < friend.maxApples)
            {
                allComplete = false;
                break;
            }
        }

        if (allComplete)
        {
            Debug.Log("¡Nivel 4 completado! Todas las pizzas repartidas equitativamente");
        }
    }
}