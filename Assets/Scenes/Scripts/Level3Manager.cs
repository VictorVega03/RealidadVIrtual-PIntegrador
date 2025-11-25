using UnityEngine;
using UnityEngine.UI;

public class Level3Manager : MonoBehaviour
{
    [Header("Referencias UI")]
    public DialogueManager dialogueManager;
    public Button playButton;
    
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