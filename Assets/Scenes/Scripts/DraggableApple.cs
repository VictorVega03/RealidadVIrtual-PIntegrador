using UnityEngine;

public class DraggableApple : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    private Vector3 originalPosition;

    void Start()
    {
        mainCamera = Camera.main;
        originalPosition = transform.position;
    }

    void OnMouseDown()
    {
        // Solo permitir arrastrar manzanas malas en Nivel 2
        if (GameConfig.currentLevel == GameConfig.Level.Level2 && gameObject.CompareTag("WormApple"))
        {
            isDragging = true;
            offset = transform.position - GetMouseWorldPosition();
        }
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            // La colisión con el bote de basura se maneja en TrashCan.cs
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // Distancia de la cámara
        return mainCamera.ScreenToWorldPoint(mousePos);
    }
}