using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogClickDebug : MonoBehaviour
{
    public GameObject DialogPanel;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("\n========================================");
            Debug.Log("CLICK DETECTADO EN PANTALLA");
            Debug.Log($"Posicion: {Input.mousePosition}");
            Debug.Log("========================================");

            // Verificar EventSystem
            if (EventSystem.current == null)
            {
                Debug.LogError("❌ NO HAY EVENTSYSTEM!");
                return;
            }

            // Hacer raycast para ver QUÉ detecta Unity
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            Debug.Log($"Objetos detectados: {results.Count}");

            if (results.Count == 0)
            {
                Debug.LogError("❌ NO SE DETECTÓ NINGÚN OBJETO UI!");
                Debug.LogError("Posibles causas:");
                Debug.LogError("- Canvas sin Graphic Raycaster");
                Debug.LogError("- Todos los objetos tienen Raycast Target desmarcado");
                Debug.LogError("- Camera mal configurada");
            }
            else
            {
                for (int i = 0; i < results.Count; i++)
                {
                    RaycastResult result = results[i];
                    Debug.Log($"{i}. {result.gameObject.name}");
                    
                    if (result.gameObject == DialogPanel)
                    {
                        Debug.Log("   ← ✓ ESTE ES EL DIALOGPANEL!");
                        
                        // Verificar componentes
                        Image img = DialogPanel.GetComponent<Image>();
                        if (img != null)
                        {
                            Debug.Log($"   - Tiene Image, Raycast Target: {img.raycastTarget}");
                        }
                        else
                        {
                            Debug.LogError("   - ❌ NO TIENE IMAGE!");
                        }

                        DialogueManager dm = DialogPanel.GetComponent<DialogueManager>();
                        if (dm != null)
                        {
                            Debug.Log("   - ✓ Tiene DialogueManager");
                        }
                        else
                        {
                            Debug.LogError("   - ❌ NO TIENE DialogueManager!");
                        }
                    }
                }
            }

            // Verificar Canvas
            if (DialogPanel != null)
            {
                Canvas canvas = DialogPanel.GetComponentInParent<Canvas>();
                if (canvas != null)
                {
                    Debug.Log($"\nCanvas encontrado: {canvas.name}");
                    Debug.Log($"  Render Mode: {canvas.renderMode}");
                    
                    GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
                    if (gr != null)
                    {
                        Debug.Log($"  ✓ Tiene Graphic Raycaster (enabled: {gr.enabled})");
                    }
                    else
                    {
                        Debug.LogError("  ❌ NO TIENE GRAPHIC RAYCASTER!");
                    }
                }
                else
                {
                    Debug.LogError("❌ DialogPanel NO ESTÁ EN UN CANVAS!");
                }
            }

            Debug.Log("========================================\n");
        }
    }
}