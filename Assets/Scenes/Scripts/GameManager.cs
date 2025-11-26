using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // VARIABLES EXISTENTES
    public Button playButton;
    public AppleSpawner appleSpawner; // Para nivel 1
    public AppleSpawnerLevel2 appleSpawnerLevel2; // Para nivel 2
    public AppleSpawnerLevel4 appleSpawnerLevel4; // Para nivel 4
    public DivisionUIManager divisionUIManager; // Para mostrar UI del nivel 4
    public SubtractionUIManager subtractionUIManager; // Para mostrar UI del nivel 2

    // ✨ NUEVAS VARIABLES PARA EL TEMPORIZADOR
    [Header("Configuración del Temporizador")]
    public float timeLimitSeconds = 60f; // 1 minuto = 60 segundos
    public TextMeshProUGUI timerText; // Arrastra aquí el objeto 'TimerText' de la UI
    private float currentTime;
    
    [Header("Referencias UI y Spawner")]
    public GameObject characterObject;
    public GameObject dialogueScroll;
    public TextMeshProUGUI dialogueText;

    [Header("Configuración del Personaje")]
    public float characterSlideSpeed = 5f;
    public float characterVisibleX = -6f;
    public float characterHiddenX = -12f;
    public string characterTag = "Character";
    public float characterSpinSpeed = 720f;

    [Header("Referencias de Audio")]
    public AudioSource characterAudio;
    public AudioSource backgroundMusic;

    [Header("Configuración de Nivel")]
    public GameConfig.Level levelToPlay = GameConfig.Level.Level1; // Selecciona el nivel en el Inspector

    private enum GameState { Initial, CharacterSlidingIn, DialogueReady, CharacterSlidingOut, Playing, GameEnded }
    private GameState currentGameState = GameState.Initial;

    void Start()
    {
        // Establecer el nivel actual
        GameConfig.currentLevel = levelToPlay;
        
        // Inicializar tiempo y texto
        currentTime = timeLimitSeconds;
        if (timerText != null)
        {
            // Ocultar el contador al inicio
            timerText.gameObject.SetActive(false); 
            UpdateTimerDisplay();
        }

        if (playButton != null)
        {
            playButton.gameObject.SetActive(true);
            playButton.onClick.AddListener(OnPlayButtonClick);
        }

        if (characterObject != null)
        {
            characterObject.transform.position = new Vector3(characterHiddenX, characterObject.transform.position.y, characterObject.transform.position.z);
        }

        if (dialogueScroll != null)
        {
            dialogueScroll.SetActive(false);
        }

        if (dialogueText != null)
        {
            dialogueText.text = "";
        }

        currentGameState = GameState.Initial;
    }

    void Update()
    {
        // Lógica de click existente para avanzar diálogo
        if (currentGameState == GameState.DialogueReady && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag(characterTag))
            {
                Debug.Log("Clic en el personaje durante el diálogo.");
                StartCoroutine(HideCharacterAndStartGame());
            }
        }
        
        // ✨ NUEVA LÓGICA DE TIEMPO
        if (currentGameState == GameState.Playing)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();

            if (currentTime <= 0f)
            {
                currentTime = 0f; // Asegurar que el tiempo no sea negativo
                currentGameState = GameState.GameEnded;
                HandleGameEnd();
            }
        }
    }
    
    // ✨ FUNCIÓN CORREGIDA: Actualiza el texto del contador
    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            // ⭐️ CORRECCIÓN: Usar Mathf.Max para asegurar que el valor sea al menos 0
            float displayTime = Mathf.Max(0f, currentTime);

            int minutes = Mathf.FloorToInt(displayTime / 60f);
            int seconds = Mathf.FloorToInt(displayTime % 60f);
            
            // Formato MM:SS
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // ✨ NUEVA FUNCIÓN: Maneja el fin del juego por tiempo
    public void HandleGameEnd()
    {
        Debug.Log("¡Tiempo terminado! Fin del juego.");
        
        // 1. Detener el juego (spawners, música, etc.)
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }
        
        // Detener spawners según el nivel
        if (GameConfig.currentLevel == GameConfig.Level.Level1 && appleSpawner != null)
        {
            appleSpawner.StopSpawning();
        }
        // ... (Agrega lógica para detener spawners de otros niveles si es necesario) ...

        // Ocultar la UI de juego específica del nivel si existe
        if (subtractionUIManager != null) subtractionUIManager.HideUI();
        if (divisionUIManager != null) divisionUIManager.HideUI();

        // 2. Mostrar el diálogo de fin
        StartCoroutine(ShowCharacterEndDialogue("Se acabó el tiempo. ¡Buen intento! \nPresiona para jugar de nuevo o finalizar."));
    }
    
    // ✨ NUEVA FUNCIÓN: Muestra el diálogo de fin (similar a ShowCharacterAndDialogue)
    IEnumerator ShowCharacterEndDialogue(string message)
    {
        // 1. Deslizar al personaje de vuelta
        currentGameState = GameState.CharacterSlidingIn;
        if (characterObject != null)
        {
            while (characterObject.transform.position.x < characterVisibleX)
            {
                characterObject.transform.position += Vector3.right * characterSlideSpeed * Time.deltaTime;
                yield return null;
            }
            characterObject.transform.position = new Vector3(characterVisibleX, characterObject.transform.position.y, characterObject.transform.position.z);
        }

        // 2. Mostrar scroll y mensaje
        if (dialogueScroll != null) dialogueScroll.SetActive(true);
        if (dialogueText != null) dialogueText.text = message;

        // 3. El juego ahora está en estado finalizado (o puedes añadir un estado "DialogueEnd")
        // Aquí podrías añadir lógica para reiniciar o ir al menú principal
    }

    void OnPlayButtonClick()
    {
        Debug.Log("Botón Jugar presionado. Iniciando secuencia de diálogo.");
        if (playButton != null)
        {
            playButton.gameObject.SetActive(false);
        }
        StartCoroutine(ShowCharacterAndDialogue());
    }

    IEnumerator ShowCharacterAndDialogue()
    {
        currentGameState = GameState.CharacterSlidingIn;

        if (characterObject != null)
        {
            while (characterObject.transform.position.x < characterVisibleX)
            {
                characterObject.transform.position += Vector3.right * characterSlideSpeed * Time.deltaTime;
                yield return null;
            }
            characterObject.transform.position = new Vector3(characterVisibleX, characterObject.transform.position.y, characterObject.transform.position.z);
        }

        if (characterAudio != null)
        {
            characterAudio.Play();
        }

        if (dialogueScroll != null)
        {
            dialogueScroll.SetActive(true);
        }

        if (dialogueText != null)
        {
            // Diálogo existente según el nivel
            if (GameConfig.currentLevel == GameConfig.Level.Level1)
            {
                dialogueText.text = "Hola! Estas listo para jugar?\nIntenta atrapar todas las manzanas que puedas :D, puedes usar las flechas para mover la canasta, pero Ojo!, debes estar atento a las manzanas y contar todas las que puedas!!";
            }
            else if (GameConfig.currentLevel == GameConfig.Level.Level2)
            {
                dialogueText.text = "Muy bien! Ahora ayúdame a separar las manzanas malas.\nArrastra las manzanas con gusano al bote de basura. Cuenta cuántas manzanas malas eliminas:D!";
            }
            else if (GameConfig.currentLevel == GameConfig.Level.Level4)
            {
                dialogueText.text = "Ahora Ayúdame a repartir estas pizzas entre mis amigos.\nCada uno debe recibir la misma cantidad. ¡Arrastra las pizzas hacia ellos!";
            }
        }

        currentGameState = GameState.DialogueReady;
    }

    IEnumerator HideCharacterAndStartGame()
    {
        currentGameState = GameState.CharacterSlidingOut;

        dialogueScroll.SetActive(false);
        if (dialogueText != null) dialogueText.text = "";

        while (characterObject.transform.position.x > characterHiddenX)
        {
            characterObject.transform.position += Vector3.left * characterSlideSpeed * Time.deltaTime;
            characterObject.transform.Rotate(0, 0, +characterSpinSpeed * Time.deltaTime);
            yield return null;
        }

        characterObject.transform.position = new Vector3(characterHiddenX, characterObject.transform.position.y, characterObject.transform.position.z);
        characterObject.transform.rotation = Quaternion.identity;

        Debug.Log("Personaje oculto. ¡Juego iniciado!");
        currentGameState = GameState.Playing;
        
        // ✨ INICIA EL CONTADOR Y LO MUESTRA
        currentTime = timeLimitSeconds;
        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
            UpdateTimerDisplay();
        }

        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }

        // Iniciar el spawner según el nivel
        if (GameConfig.currentLevel == GameConfig.Level.Level1)
        {
            if (appleSpawner != null)
            {
                appleSpawner.StartSpawning();
            }
        }
        else if (GameConfig.currentLevel == GameConfig.Level.Level2)
        {
            if (appleSpawnerLevel2 != null)
            {
                appleSpawnerLevel2.SpawnAllApples();
            }
            
            // Mostrar UI de resta
            if (subtractionUIManager != null)
            {
                subtractionUIManager.ShowUI();
            }
        }
        else if (GameConfig.currentLevel == GameConfig.Level.Level4)
        {
            if (appleSpawnerLevel4 != null)
            {
                appleSpawnerLevel4.SpawnAllApples();
            }
            
            // Mostrar UI de división
            if (divisionUIManager != null)
            {
                divisionUIManager.ShowUI();
            }
        }
    }
}