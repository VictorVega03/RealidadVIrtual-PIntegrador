using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject DialogPanel;
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;
    
    [Header("Character Animation")]
    public Animator characterAnimator;

    private Coroutine typingCoroutine;

    void Start()
    {
        Debug.Log("=== DialogueManager Start() ===");
        
        if (DialogPanel == null)
        {
            Debug.LogError("ERROR: DialogPanel es NULL!");
            return;
        }
        
        DialogPanel.SetActive(false);
        Debug.Log("✓ DialogPanel desactivado en Start()");
    }

    public void ShowDialogue(string message)
    {
        Debug.Log($"\n========== ShowDialogue() ==========");
        Debug.Log($"Mensaje: {message}");
        
        if (DialogPanel == null)
        {
            Debug.LogError("ERROR: DialogPanel es NULL!");
            return;
        }
        
        if (dialogueText == null)
        {
            Debug.LogError("ERROR: dialogueText es NULL!");
            return;
        }
        
        Debug.Log($"DialogPanel estado antes de activar: {DialogPanel.activeSelf}");
        
        DialogPanel.SetActive(true);
        
        Debug.Log($"DialogPanel estado después de activar: {DialogPanel.activeSelf}");
        
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            Debug.Log("Coroutine anterior detenida");
        }
        
        typingCoroutine = StartCoroutine(TypeText(message));
        Debug.Log("✓ Coroutine TypeText iniciada");
        
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("Talk");
            Debug.Log("✓ Animación 'Talk' activada");
        }
    }

    IEnumerator TypeText(string message)
    {
        Debug.Log("→ TypeText coroutine iniciada");
        dialogueText.text = "";
        
        int charCount = 0;
        foreach (char letter in message)
        {
            dialogueText.text += letter;
            charCount++;
            yield return new WaitForSeconds(typingSpeed);
        }
        
        Debug.Log($"✓ TypeText completado ({charCount} caracteres)");
        Debug.Log("→ Esperando 3 segundos antes de cerrar...");
        
        yield return new WaitForSeconds(3f);
        
        Debug.Log("→ Llamando a HideDialogue()");
        HideDialogue();
    }

    public void HideDialogue()
    {
        Debug.Log("\n========== HideDialogue() ==========");
        
        if (DialogPanel != null)
        {
            DialogPanel.SetActive(false);
            Debug.Log("✓ DialogPanel desactivado");
        }
        
        if (characterAnimator != null)
        {
            characterAnimator.SetTrigger("Idle");
            Debug.Log("✓ Animación 'Idle' activada");
        }
    }
}