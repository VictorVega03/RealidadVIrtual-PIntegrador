using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Button playButton;
    public AppleSpawner appleSpawner; // Para nivel 1
    public AppleSpawnerLevel2 appleSpawnerLevel2; // NUEVO: Para nivel 2

    [Header("Referencias UI y Spawner")]
    public GameObject characterObject;
    public GameObject dialogueScroll;
    public TextMeshProUGUI dialogueText;

    [Header("Configuración del Personaje")]
    public float characterSlideSpeed = 5f;
    public float characterVisibleX = -6f;
    public float characterHiddenX = -12f;
    public string characterTag = "DialogueTrigger";
    public float characterSpinSpeed = 720f;

    [Header("Referencias de Audio")]
    public AudioSource characterAudio;
    public AudioSource backgroundMusic;

    [Header("Configuración de Nivel")]
    public GameConfig.Level levelToPlay = GameConfig.Level.Level1; // Selecciona el nivel en el Inspector

    private enum GameState { Initial, CharacterSlidingIn, DialogueReady, CharacterSlidingOut, Playing }
    private GameState currentGameState = GameState.Initial;

    void Start()
    {
        // Establecer el nivel actual
        GameConfig.currentLevel = levelToPlay;

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
            // NUEVO: Diálogo diferente según el nivel
            if (GameConfig.currentLevel == GameConfig.Level.Level1)
            {
                dialogueText.text = "Hola! Estas listo para jugar?\nIntenta atrapar todas las manzanas que puedas :D, puedes usar las flechas para mover la canasta, pero Ojo!, debes estar atento a las manzanas y contar todas las que puedas!!";
            }
            else if (GameConfig.currentLevel == GameConfig.Level.Level2)
            {
                dialogueText.text = "Muy bien! Ahora ayúdame a separar las manzanas malas.\nArrastra las manzanas con gusano al bote de basura. Cuenta cuántas manzanas malas eliminas:D!";
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

        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }

        // NUEVO: Iniciar el spawner según el nivel
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
        }
    }
}