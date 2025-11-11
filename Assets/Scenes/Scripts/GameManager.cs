using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; // Para Coroutines

public class GameManager : MonoBehaviour
{
    // Asigna estos componentes en el Inspector de Unity
    public Button playButton;
    public AppleSpawner appleSpawner; // Referencia al script de generación

    // ✨ Referencias a los nuevos objetos
    [Header("Referencias UI y Spawner")]
    public GameObject characterObject;   // Objeto del personaje (ForestSpirit)
    public GameObject dialogueScroll;    // Objeto del pergamino
    public TextMeshProUGUI dialogueText;        

    [Header("Configuración del Personaje")]
    public float characterSlideSpeed = 5f; // Velocidad de deslizamiento del personaje
    public float characterVisibleX = -6f;  // Posición X donde el personaje se detiene (visible en cámara)
    public float characterHiddenX = -12f;  // Posición X donde el personaje está fuera de cámara
    public string characterTag = "DialogueTrigger"; // Tag del personaje para el clic
    public float characterSpinSpeed = 720f;

    [Header("Referencias de Audio")]    
    public AudioSource characterAudio;
    public AudioSource backgroundMusic; 

    private enum GameState { Initial, CharacterSlidingIn, DialogueReady, CharacterSlidingOut, Playing }
    private GameState currentGameState = GameState.Initial;

    void Start()
    {
        // Asegúrate de que el botón de jugar esté activo al inicio
        if (playButton != null)
        {
            playButton.gameObject.SetActive(true);
            playButton.onClick.AddListener(OnPlayButtonClick);
        }

        // Asegúrate de que el personaje esté escondido al inicio
        if (characterObject != null)
        {
            characterObject.transform.position = new Vector3(characterHiddenX, characterObject.transform.position.y, characterObject.transform.position.z);
        }

        // Asegúrate de que el pergamino esté oculto al inicio
        if (dialogueScroll != null)
        {
            dialogueScroll.SetActive(false);
        }

        // Vaciar texto inicial si existe
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }

        currentGameState = GameState.Initial;
    }

    void Update()
    {
        // Detectar clics en el personaje SOLO cuando el diálogo está listo (DialogueReady)
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
    }

    // Función llamada cuando se presiona el botón "Jugar"
    void OnPlayButtonClick()
    {
        Debug.Log("Botón Jugar presionado. Iniciando secuencia de diálogo.");
        if (playButton != null)
        {
            playButton.gameObject.SetActive(false);
        }
        StartCoroutine(ShowCharacterAndDialogue());
    }

    void StartGame()
    {
        if (appleSpawner != null)
        {
            appleSpawner.StartSpawning(); // Llama a la función de inicio de generación
        }
        else
        {
            Debug.LogError("Error: AppleSpawner no está asignado al GameManager.");
        }
    }
   
    IEnumerator ShowCharacterAndDialogue()
    {
        // 0. Cambiar el estado: El personaje está entrando.
        currentGameState = GameState.CharacterSlidingIn; 

        // 1. Deslizar al personaje hacia la vista (código de movimiento)
        if (characterObject != null)
        {
            while (characterObject.transform.position.x < characterVisibleX)
            {
                characterObject.transform.position += Vector3.right * characterSlideSpeed * Time.deltaTime;
                yield return null; 
            }
            characterObject.transform.position = new Vector3(characterVisibleX, characterObject.transform.position.y, characterObject.transform.position.z);
        }

        // ✨ 1.5 REPRODUCIR EL SONIDO DEL PERSONAJE
        if (characterAudio != null)
        {
            characterAudio.Play();
        }

        // 2. Mostrar el pergamino
        if (dialogueScroll != null)
        {
            dialogueScroll.SetActive(true);
        }

        // 3. Mostrar el diálogo
        if (dialogueText != null)
        {
            dialogueText.text = "Hola! Estas listo para jugar?\nIntenta atrapar todas las manzanas que puedas :D, puedes usar las flechas para mover la canasta, pero Ojo!, debes estar atento a las manzanas y contar todas las que puedas!!";
        }

        // 4. Cambiar el estado: El diálogo está listo.
        currentGameState = GameState.DialogueReady; 
    }

    IEnumerator HideCharacterAndStartGame()
    {
    // 0. Cambiar el estado: El personaje está saliendo. El clic está deshabilitado.
    currentGameState = GameState.CharacterSlidingOut;

    // 1. Ocultar el pergamino
    dialogueScroll.SetActive(false);
    if (dialogueText != null) dialogueText.text = ""; 

    // 2. Deslizar al personaje fuera de la vista Y APLICAR EL GIRO
    while (characterObject.transform.position.x > characterHiddenX)
    {
        // Movimiento a la izquierda
        characterObject.transform.position += Vector3.left * characterSlideSpeed * Time.deltaTime;                
        characterObject.transform.Rotate(0, 0, +characterSpinSpeed * Time.deltaTime);

        yield return null;
    }
    
    // Asegurar la posición final y resetear la rotación (opcional, pero limpio)
    characterObject.transform.position = new Vector3(characterHiddenX, characterObject.transform.position.y, characterObject.transform.position.z);
    characterObject.transform.rotation = Quaternion.identity; // Asegura que el giro se detiene y la rotación vuelve a 0

    // 3. Iniciar el juego
    Debug.Log("Personaje oculto. ¡Juego iniciado!");
    currentGameState = GameState.Playing;
    // REPRODUCIR LA MÚSICA DE FONDO
    if (backgroundMusic != null)
    {
        backgroundMusic.Play();
    }
    if (appleSpawner != null)
    {
        appleSpawner.StartSpawning(); 
    }
    
    }
}