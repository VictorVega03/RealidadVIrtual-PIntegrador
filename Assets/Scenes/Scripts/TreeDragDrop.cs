using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TreeDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Transform originalParent;
    private Canvas canvas;
    private int originalSiblingIndex;
    private bool wasPlaced = false; // ← NUEVO: Si el árbol fue plantado

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        Debug.Log($"TreeDragDrop Awake en {gameObject.name}");
    }

    void Start()
    {
        if (!CompareTag("Tree"))
        {
            Debug.LogWarning($"{gameObject.name} no tiene tag 'Tree'!");
        }
        else
        {
            Debug.Log($"✓ {gameObject.name} inicializado correctamente");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"\n========== OnBeginDrag en {gameObject.name} ==========");
        
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
        wasPlaced = false;
        
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        
        transform.SetAsLastSibling();
        
        Debug.Log("✓ Alpha reducido, raycasts desactivados");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

public void OnEndDrag(PointerEventData eventData)
{
    Debug.Log($"\n========== OnEndDrag en {gameObject.name} ==========");
    
    GridManager gridManager = FindObjectOfType<GridManager>();
    if (gridManager != null)
    {
        // Obtener posición en espacio mundial
        Vector3 worldPosition = transform.position;
        Vector2 dropPosition = new Vector2(worldPosition.x, worldPosition.y);
        
        Debug.Log($"Posicion mundial: {dropPosition}");
        
        CellController closestCell = gridManager.FindClosestCell(dropPosition);
        
        if (closestCell != null)
        {
            RectTransform cellRect = closestCell.GetComponent<RectTransform>();
            
            // Calcular distancia ya convertida
            Vector2 localDropPos = gridManager.gridParent.InverseTransformPoint(dropPosition);
            float distance = Vector2.Distance(localDropPos, cellRect.anchoredPosition);
            
            Debug.Log($"Distancia real: {distance}");
            
            float maxDistance = 100f;
            
            if (distance <= maxDistance && !closestCell.HasTree())
            {
                Debug.Log($"✓ Celda valida (distancia: {distance})");
                closestCell.TryPlaceTree();
                wasPlaced = true;
                gameObject.SetActive(false);
            }
            else if (distance > maxDistance)
            {
                Debug.Log($"⚠ Demasiado lejos (distancia: {distance}, max: {maxDistance})");
            }
            else
            {
                Debug.Log("⚠ Celda ya ocupada");
            }
        }
    }
    
    if (!wasPlaced)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        rectTransform.anchoredPosition = originalPosition;
        transform.SetSiblingIndex(originalSiblingIndex);
    }
}
}