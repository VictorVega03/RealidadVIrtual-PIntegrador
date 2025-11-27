using UnityEngine;
using TMPro;

public class SubtractionUIManager : MonoBehaviour
{
    [Header("Referencias de UI")]
    public TextMeshProUGUI operationText; // "17 - 5 = 12"
    public TextMeshProUGUI progressText; // "Manzanas malas restantes: 3"
    
    [Header("Configuración")]
    public bool showProgress = true;
    
    private int totalApples;
    private int badApples;
    private int goodApples;
    private int remainingBadApples;
    private bool isGameStarted = false;
    
    void Start()
    {
        HideUI();
    }
    
    void Update()
    {
        if (showProgress && isGameStarted)
        {
            // Actualizar progreso en tiempo real
            remainingBadApples = GameConfig.badApplesRemaining;
            UpdateProgressDisplay();
        }
    }
    
    public void ShowUI()
    {
        isGameStarted = true;
        
        // Obtener valores de GameConfig
        totalApples = GameConfig.appleCountFromLevel1;
        badApples = GameConfig.badApplesRemaining;
        goodApples = totalApples - badApples;
        remainingBadApples = badApples;
        
        if (operationText != null)
        {
            operationText.gameObject.SetActive(true);
        }
        
        if (progressText != null)
        {
            progressText.gameObject.SetActive(true);
            progressText.fontSize = 24;
        }
        
        UpdateDisplay();
        
        Debug.Log("Subtraction UI mostrada");
    }
    
    public void HideUI()
    {
        if (operationText != null)
        {
            operationText.gameObject.SetActive(false);
        }
        
        if (progressText != null)
        {
            progressText.gameObject.SetActive(false);
        }
    }
    
    void UpdateDisplay()
    {
        // Operación de resta - TODO EN NEGRO
        if (operationText != null)
        {
            operationText.text = $"<color=#000000><size=50>{totalApples}</size></color> " +
                               $"<color=#000000><b>-</b></color> " +
                               $"<color=#000000><size=50>{badApples}</size></color> " +
                               $"<color=#000000><b>=</b></color> " +
                               $"<color=#000000><size=50>{goodApples}</size></color>";
        }
        
        UpdateProgressDisplay();
    }
    
    void UpdateProgressDisplay()
    {
        if (progressText != null && showProgress)
        {
            progressText.color = Color.black;
            
            if (remainingBadApples == 0)
            {
                progressText.text = "¡Todas las manzanas malas eliminadas!";
            }
            else
            {
                progressText.text = $"Manzanas malas restantes: {remainingBadApples}/{badApples}";
            }
        }
    }
}