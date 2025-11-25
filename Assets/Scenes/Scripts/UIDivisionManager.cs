using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DivisionUIManager : MonoBehaviour
{
    [Header("Referencias de UI")]
    public TextMeshProUGUI operationText; // "12 ÷ 4 = ?"
    public TextMeshProUGUI progressText; // "Pizzas restantes: 8"
    
    [Header("Configuración")]
    public bool showProgress = true; // Mostrar progreso en tiempo real
    public bool autoCalculateFromScene = true;

    private int totalPizzas;
    private int numberOfFriends;
    private int pizzasPerFriend;
    private int remainingPizzas;
    private bool isGameStarted = false; // ✨ NUEVO
    
    void Start()
    {
        InitializeValues();
        
        // ✨ NUEVO: Ocultar UI al inicio
        HideUI();
        
        // Ajustar tamaño del texto de progreso automáticamente
        if (progressText != null)
        {
            progressText.fontSize = 24; // Tamaño más pequeño
        }
    }
    
    void Update()
    {
        // Solo actualizar si el juego ya empezó
        if (showProgress && isGameStarted)
        {
            // Contar pizzas restantes en tiempo real
            GameObject[] pizzas = GameObject.FindGameObjectsWithTag("Apple");
            remainingPizzas = pizzas.Length;
            UpdateProgressDisplay();
        }
    }
    
    void InitializeValues()
    {
        if (autoCalculateFromScene)
        {
            FriendController[] friends = FindObjectsOfType<FriendController>();
            
            if (friends.Length > 0)
            {
                numberOfFriends = friends.Length;
                pizzasPerFriend = friends[0].maxApples;
                totalPizzas = numberOfFriends * pizzasPerFriend;
                remainingPizzas = totalPizzas;
                
                // Actualizar GameConfig
                GameConfig.totalApples = totalPizzas;
                GameConfig.numberOfFriends = numberOfFriends;
            }
        }
        else
        {
            totalPizzas = GameConfig.totalApples;
            numberOfFriends = GameConfig.numberOfFriends;
            pizzasPerFriend = totalPizzas / numberOfFriends;
            remainingPizzas = totalPizzas;
        }
    }
    
    void UpdateDisplay()
    {
        // Operación principal - TODO EN NEGRO
        if (operationText != null)
        {
            // Usar directamente negro sin variables, más simple
            operationText.text = $"<color=#000000><size=50>{totalPizzas}</size></color> " +
                               $"<color=#000000><b>÷</b></color> " +
                               $"<color=#000000><size=50>{numberOfFriends}</size></color> " +
                               $"<color=#000000><b>=</b></color> " +
                               $"<color=#000000><size=50>{pizzasPerFriend}</size></color>";
        }
        
        UpdateProgressDisplay();
    }
    
    void UpdateProgressDisplay()
    {
        if (progressText != null && showProgress)
        {
            // Siempre en negro
            progressText.color = Color.black;
            
            if (remainingPizzas == 0)
            {
                progressText.text = "¡Todas las pizzas repartidas!";
            }
            else
            {
                progressText.text = $"Pizzas restantes: {remainingPizzas}/{totalPizzas}";
            }
        }
    }
    
    // ✨ NUEVO: Método público para mostrar la UI
    public void ShowUI()
    {
        isGameStarted = true;
        
        if (operationText != null)
        {
            operationText.gameObject.SetActive(true);
        }
        
        if (progressText != null)
        {
            progressText.gameObject.SetActive(true);
        }
        
        UpdateDisplay(); // Actualizar contenido
        
        Debug.Log("📊 Division UI mostrada");
    }
    
    // ✨ NUEVO: Método para ocultar la UI
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
        
        Debug.Log("📊 Division UI ocultada");
    }
}