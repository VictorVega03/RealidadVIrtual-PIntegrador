using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
public class Level3Manager : MonoBehaviour
{
    [Header("Referencias UI")]
    public DialogueManager dialogueManager;
    public Button playButton;
    // ✨ NUEVA REFERENCIA: Botón para salir al menú
    public Button exitButton; 
    
    [Header("Audio")]
    public AudioSource voiceAudio;
    public AudioSource backgroundMusic;
    
    [Header("Tutorial State")]
    public bool isTutorialMode = true;
    public bool gameStarted = false;
    private int totalTreesAvailable = 5;
    private int treesPlaced = 0;

    void Start()
    {
        if (playButton != null)
        {
            playButton.gameObject.SetActive(true);
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        
        // ✨ CONEXIÓN DEL BOTÓN DE SALIR
        if (exitButton != null)
        {
            // Nota: Asumo que el botón ya está activo en la escena
            exitButton.onClick.AddListener(LoadMenuScene); 
        }
        
        if (dialogueManager == null)
        {
            dialogueManager = FindObjectOfType<DialogueManager>();
        }
        
        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }
        
        SetTreesDraggable(false);
    }

    // ✨ NUEVA FUNCIÓN: Carga la escena "menu"
    public void LoadMenuScene()
    {
        // 🚨 Importante: Asegúrate que la escena "menu" esté añadida en Build Settings
        Debug.Log("Cargando escena: menu");
        SceneManager.LoadScene("menu");
    }
    
    public void OnPlayButtonClicked()
    {
        if (playButton != null)
        {
            playButton.gameObject.SetActive(false);
        }
        
        if (voiceAudio != null)
        {
            voiceAudio.Play();
        }
        
        ShowWelcomeMessage();
    }

    void ShowWelcomeMessage()
    {
        if (dialogueManager != null)
        {
            string welcomeMessage = "¡Bienvenido! Arrastra 5 arboles a la cuadricula para aprender las tablas de multiplicar.";
            
            // Mostrar por 5 segundos (mensaje largo)
            dialogueManager.ShowDialogue(welcomeMessage, true, 5f);
        }
    }

    void SetTreesDraggable(bool draggable)
    {
        TreeDragDrop[] trees = FindObjectsOfType<TreeDragDrop>();
        
        foreach (TreeDragDrop tree in trees)
        {
            tree.enabled = draggable;
        }
    }

    public void OnCharacterClicked()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            dialogueManager.HideDialogue();
            SetTreesDraggable(true);
            Debug.Log("✓ Juego comenzado - Arboles habilitados");
        }
    }

    public void OnTreePlacedInTutorial()
    {
        treesPlaced++;
        Debug.Log($"Arboles plantados: {treesPlaced}/{totalTreesAvailable}");
        
        if (treesPlaced >= totalTreesAvailable)
        {
            ShowFinalMessage();
        }
    }

    void ShowFinalMessage()
    {
        GridManager gridManager = FindObjectOfType<GridManager>();
        
        if (gridManager != null)
        {
            int totalScore = gridManager.CalculateTotalScore();
            
            if (voiceAudio != null)
            {
                voiceAudio.Play();
            }
            
            if (dialogueManager != null)
            {
                string finalMessage = $"¡Felicidades!\n\nHas completado el nivel.\nTu puntuacion: {totalScore} puntos\n\n¡Excelente trabajo aprendiendo a multiplicar!";
                
                // Mostrar por 6 segundos (mensaje final)
                dialogueManager.ShowDialogue(finalMessage, false, 6f);
            }
            
            isTutorialMode = false;
        }
    }
}