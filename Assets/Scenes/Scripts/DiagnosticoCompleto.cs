using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DiagnosticoCompleto : MonoBehaviour
{
    public Button botonAProbar;
    
    void Start()
    {
        Debug.Log("========================================");
        Debug.Log("INICIANDO DIAGNÓSTICO COMPLETO");
        Debug.Log("========================================");
        
        // 1. Verificar EventSystem
        EventSystem eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            Debug.LogError("❌ NO HAY EVENTSYSTEM EN LA ESCENA!");
        }
        else
        {
            Debug.Log("✓ EventSystem encontrado: " + eventSystem.gameObject.name);
            Debug.Log("  - Enabled: " + eventSystem.enabled);
        }
        
        // 2. Verificar el botón
        if (botonAProbar == null)
        {
            Debug.LogError("❌ Botón no asignado en el inspector!");
            return;
        }
        
        Debug.Log("\n--- INFORMACIÓN DEL BOTÓN ---");
        Debug.Log("Nombre: " + botonAProbar.gameObject.name);
        Debug.Log("Activo: " + botonAProbar.gameObject.activeInHierarchy);
        Debug.Log("Interactable: " + botonAProbar.interactable);
        Debug.Log("Raycast Target: " + botonAProbar.GetComponent<Image>().raycastTarget);
        
        // Mostrar jerarquía completa del botón
        Debug.Log("\n--- JERARQUÍA DEL BOTÓN ---");
        Transform current = botonAProbar.transform;
        string path = current.name;
        while (current.parent != null)
        {
            current = current.parent;
            path = current.name + " → " + path;
            
            Canvas canvasComponent = current.GetComponent<Canvas>();
            if (canvasComponent != null)
            {
                Debug.Log($"Canvas encontrado: {current.name}");
                GraphicRaycaster gr = current.GetComponent<GraphicRaycaster>();
                if (gr != null)
                {
                    Debug.Log($"  ✓ Tiene Graphic Raycaster");
                }
                else
                {
                    Debug.LogError($"  ❌ NO tiene Graphic Raycaster!");
                }
            }
        }
        Debug.Log($"Path completo: {path}");
        
        // 3. Verificar Canvas
        Canvas canvas = botonAProbar.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            Debug.Log("\n--- INFORMACIÓN DEL CANVAS ---");
            Debug.Log("Render Mode: " + canvas.renderMode);
            
            GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
            if (raycaster != null)
            {
                Debug.Log("✓ Graphic Raycaster presente");
                Debug.Log("  - Enabled: " + raycaster.enabled);
            }
            else
            {
                Debug.LogError("❌ NO HAY GRAPHIC RAYCASTER EN EL CANVAS!");
            }
        }
        
        // 4. Contar listeners del botón
        int listenerCount = botonAProbar.onClick.GetPersistentEventCount();
        Debug.Log("\n--- EVENTOS DEL BOTÓN ---");
        Debug.Log("Listeners persistentes: " + listenerCount);
        
        for (int i = 0; i < listenerCount; i++)
        {
            Debug.Log($"  Listener {i}: {botonAProbar.onClick.GetPersistentMethodName(i)}");
        }
        
        // 5. Agregar listener de prueba
        botonAProbar.onClick.AddListener(TestClick);
        Debug.Log("✓ Listener de prueba agregado");
        
        Debug.Log("\n========================================");
        Debug.Log("FIN DIAGNÓSTICO - Presiona el botón ahora");
        Debug.Log("========================================\n");
    }
    
    void TestClick()
    {
        Debug.Log("╔════════════════════════════════════════╗");
        Debug.Log("║   ¡¡¡EL BOTÓN FUNCIONA!!!              ║");
        Debug.Log("╚════════════════════════════════════════╝");
    }
    
    void Update()
    {
        // Detectar cualquier click
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log($"\n→ CLICK detectado en posición: {Input.mousePosition}");
            
            // Ver qué objetos detecta el raycast
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            
            Debug.Log($"Objetos bajo el cursor: {results.Count}");
            
            if (results.Count == 0)
            {
                Debug.LogWarning("⚠ NO SE DETECTÓ NINGÚN OBJETO UI!");
            }
            else
            {
                foreach (RaycastResult result in results)
                {
                    Debug.Log($"  • {result.gameObject.name}");
                    
                    if (result.gameObject == botonAProbar.gameObject)
                    {
                        Debug.Log("    ← ¡Este es el botón!");
                    }
                }
            }
        }
    }
}