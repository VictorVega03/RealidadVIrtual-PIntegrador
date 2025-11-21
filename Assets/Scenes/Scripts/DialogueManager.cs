using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject DialogPanel;
    public TextMeshProUGUI DialogueText;
    
    [Header("Settings")]
    public float typingSpeed = 0.05f;
    public float displayTime = 3f; // Tiempo que se muestra el dialogo antes de cerrarse
    
    private Coroutine typingCoroutine;
    private Coroutine closeCoroutine;
    private bool isWelcomeMessage = false;

    void Awake()
    {
        if (DialogPanel != null && DialogPanel.activeSelf)
        {
            DialogPanel.SetActive(false);
        }
        
        if (DialogueText != null)
        {
            DialogueText.richText = true;
        }
    }

    public void ShowDialogue(string message, bool isWelcome = false, float customDisplayTime = -1)
    {
        if (DialogPanel == null || DialogueText == null) return;
        
        isWelcomeMessage = isWelcome;
        
        // Detener coroutines anteriores
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        if (closeCoroutine != null)
        {
            StopCoroutine(closeCoroutine);
        }
        
        DialogPanel.SetActive(true);
        
        float timeToShow = customDisplayTime > 0 ? customDisplayTime : displayTime;
        typingCoroutine = StartCoroutine(TypeTextAndClose(message, timeToShow, isWelcome));
    }

    public void HideDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        if (closeCoroutine != null)
        {
            StopCoroutine(closeCoroutine);
        }
        
        if (DialogPanel != null)
        {
            DialogPanel.SetActive(false);
        }
        
        isWelcomeMessage = false;
    }

    IEnumerator TypeTextAndClose(string message, float timeToShow, bool isWelcome)
    {
        // Escribir el texto
        DialogueText.text = "";
        
        foreach (char letter in message)
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        
        Debug.Log($"✓ Texto completado, esperando {timeToShow} segundos");
        
        // Si es mensaje de bienvenida, esperar más tiempo
        if (isWelcome)
        {
            yield return new WaitForSeconds(timeToShow);
            
            // Notificar al Level3Manager para iniciar juego
            Level3Manager levelManager = FindObjectOfType<Level3Manager>();
            if (levelManager != null)
            {
                levelManager.OnCharacterClicked();
            }
        }
        else
        {
            // Mensaje normal - esperar y cerrar
            yield return new WaitForSeconds(timeToShow);
            HideDialogue();
        }
    }
}