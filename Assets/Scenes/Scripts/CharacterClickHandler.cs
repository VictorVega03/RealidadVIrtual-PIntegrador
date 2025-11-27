using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterClickHandler : MonoBehaviour, IPointerClickHandler
{
    private DialogueManager dialogueManager;
    private Level3Manager levelManager;
    private Image image;

    void Awake()
    {
        // Asegurar que tiene Image con Raycast Target
        image = GetComponent<Image>();
        if (image == null)
        {
            image = gameObject.AddComponent<Image>();
            image.color = new Color(1, 1, 1, 0); // Transparente
            Debug.Log("✓ Image agregado automaticamente al personaje");
        }
        
        image.raycastTarget = true;
        Debug.Log("✓ Raycast Target habilitado en personaje");
    }

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        levelManager = FindObjectOfType<Level3Manager>();
        
        if (dialogueManager == null)
        {
            Debug.LogError("ERROR: No se encontro DialogueManager!");
        }
        
        if (levelManager == null)
        {
            Debug.LogError("ERROR: No se encontro Level3Manager!");
        }
        
        Debug.Log($"✓ CharacterClickHandler inicializado en {gameObject.name}");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("========================================");
        Debug.Log("→ CLICK EN PERSONAJE DETECTADO");
        Debug.Log("========================================");
        
        if (dialogueManager != null && dialogueManager.DialogPanel != null)
        {
            bool dialogoAbierto = dialogueManager.DialogPanel.activeSelf;
            Debug.Log($"Estado dialogo: {(dialogoAbierto ? "ABIERTO" : "CERRADO")}");
            
            if (dialogoAbierto)
            {
                // Si dialogo esta abierto, cerrarlo
                dialogueManager.HideDialogue();
                Debug.Log("✓ Dialogo cerrado por click en personaje");
                
                // Si es el primer click (inicio del juego), habilitar arboles
                if (levelManager != null && !levelManager.gameStarted)
                {
                    levelManager.OnCharacterClicked();
                }
            }
            else
            {
                Debug.Log("⚠ Dialogo ya estaba cerrado");
            }
        }
        else
        {
            Debug.LogError("ERROR: DialogueManager o DialogPanel es NULL!");
        }
    }
}