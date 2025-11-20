using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Settings")]
    public bool isTutorialMode = true;
    public int tutorialTreesPlaced = 0;
    public int tutorialTreesRequired = 3;
    private bool hasStartedTutorial = false;

    [Header("UI References")]
    public Button playButton;
    public DialogueManager dialogueManager;
    
    [Header("Button Sprites (Solo 2)")]
    public Sprite jugarSprite;      // Sprite "JUGAR"
    public Sprite verificarSprite;  // Sprite "VERIFICAR"
    private Image buttonImage;

    [Header("Tutorial Messages")]
    public string welcomeMessage = "¡Hola! ¿Estás listo para jugar?\n" +
        "Arrastra los árboles a la grid.\n" +
        "Cada posición tiene un valor diferente!\n" +
        "Intenta plantar 3 árboles.";

    void Start()
    {
        Debug.Log("\n=== TutorialManager Start() ===");
        
        if (playButton == null)
        {
            Debug.LogError("ERROR: playButton es NULL!");
            return;
        }
        
        if (dialogueManager == null)
        {
            Debug.LogError("ERROR: dialogueManager es NULL!");
            return;
        }
        
        buttonImage = playButton.GetComponent<Image>();
        
        if (buttonImage == null)
        {
            Debug.LogError("ERROR: El botón no tiene Image component!");
            return;
        }
        
        // Mostrar botón JUGAR al inicio
        if (jugarSprite != null)
        {
            buttonImage.sprite = jugarSprite;
            Debug.Log("✓ Botón inicial: JUGAR");
        }
        else
        {
            Debug.LogWarning("⚠ jugarSprite es NULL!");
        }

        //playButton.onClick.AddListener(OnPlayButtonClicked);
        Debug.Log("✓ Listener del botón agregado");
        Debug.Log("=== Fin TutorialManager Start() ===\n");
    }

    public void OnPlayButtonClicked()
    {
        Debug.Log("\n========== BOTÓN PRESIONADO ==========");
        Debug.Log($"Has Started Tutorial: {hasStartedTutorial}");
        Debug.Log($"Trees Placed: {tutorialTreesPlaced}/{tutorialTreesRequired}");
        
        if (isTutorialMode)
        {
            if (!hasStartedTutorial)
            {
                // Primera vez: Presionó JUGAR
                Debug.Log("→ Usuario presionó JUGAR - Mostrando instrucciones");
                StartTutorial();
            }
            else if (tutorialTreesPlaced >= tutorialTreesRequired)
            {
                // Ya completó el tutorial: Presionó VERIFICAR
                Debug.Log("→ Usuario presionó VERIFICAR - Validando");
                ValidateTutorial();
            }
            else
            {
                // Presionó VERIFICAR pero no ha plantado suficientes árboles
                Debug.Log($"⚠ Faltan árboles! ({tutorialTreesPlaced}/{tutorialTreesRequired})");
                string msg = $"¡Espera! Necesitas plantar {tutorialTreesRequired - tutorialTreesPlaced} árbol(es) más.";
                dialogueManager.ShowDialogue(msg);
            }
        }
        else
        {
            Debug.Log("→ Modo juego - Iniciando");
            StartGame();
        }
    }

    void StartTutorial()
    {
        Debug.Log("→ StartTutorial() - Iniciando tutorial");
        
        hasStartedTutorial = true;
        
        // Mostrar instrucciones
        dialogueManager.ShowDialogue(welcomeMessage);
        
        // Cambiar botón a VERIFICAR (ya puede verificar cuando quiera)
        if (verificarSprite != null)
        {
            buttonImage.sprite = verificarSprite;
            Debug.Log("✓ Botón cambiado a: VERIFICAR");
        }
        
        Debug.Log("✓ Tutorial iniciado - Usuario puede arrastrar árboles");
    }

    public void OnTreePlacedInTutorial()
    {
        tutorialTreesPlaced++;
        Debug.Log($"\n→ ¡Árbol plantado! Total: {tutorialTreesPlaced}/{tutorialTreesRequired}");
        
        if (tutorialTreesPlaced >= tutorialTreesRequired)
        {
            CompleteTutorial();
        }
    }

    void CompleteTutorial()
    {
        Debug.Log("→ CompleteTutorial() - ¡Tutorial completado!");
        
        string completeMessage = "¡Excelente! Has plantado 3 árboles.\n" +
            "Ahora presiona VERIFICAR para ver tu puntuación.";
        
        dialogueManager.ShowDialogue(completeMessage);
        Debug.Log("✓ Mensaje de completado mostrado");
    }

    void ValidateTutorial()
    {
        Debug.Log("→ ValidateTutorial() - Calculando puntuación");
        
        GridManager gridManager = FindObjectOfType<GridManager>();
        
        if (gridManager == null)
        {
            Debug.LogError("ERROR: No se encontró GridManager!");
            return;
        }
        
        int score = gridManager.CalculateTotalScore();
        Debug.Log($"✓ Puntuación calculada: {score} puntos");
        
        string resultMessage = $"¡Muy bien!\n" +
            $"Tu puntuación total es: {score} puntos\n" +
            $"¡Has completado el tutorial!";
        
        dialogueManager.ShowDialogue(resultMessage);
        Debug.Log("✓ Resultado mostrado");
        
        // Cambiar a modo juego después de 5 segundos
        Invoke("SwitchToGameMode", 5f);
    }

    void SwitchToGameMode()
    {
        Debug.Log("\n→ SwitchToGameMode() - Cambiando a modo juego");
        isTutorialMode = false;
        
        // Aquí puedes cargar otra escena o reiniciar
        Debug.Log("=== Modo juego activado ===");
        Debug.Log("(Aquí podrías cargar la siguiente escena)\n");
    }

    public void StartGame()
    {
        Debug.Log("→ StartGame() - Iniciando juego normal");
        // Lógica del juego normal
    }

    public void TestButton()
{
    Debug.Log("========== ¡BOTÓN FUNCIONA! ==========");
}
}
