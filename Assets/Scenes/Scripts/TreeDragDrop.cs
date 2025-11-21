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
        Debug.Log($"Posición final: {rectTransform.anchoredPosition}");
        
        // DETECCIÓN MANUAL: Buscar celda más cercana
        GridManager gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null)
        {
            Vector2 dropPosition = rectTransform.anchoredPosition;
            CellController closestCell = gridManager.FindClosestCell(dropPosition);
            
            if (closestCell != null)
            {
                Debug.Log($"✓ Celda más cercana encontrada");
                closestCell.TryPlaceTree();
            }
            else
            {
                Debug.Log("⚠ No se encontró celda cercana");
            }
        }
        
        // Restaurar transparencia y raycasts
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        
        // Volver a posición y orden original
        rectTransform.anchoredPosition = originalPosition;
        transform.SetSiblingIndex(originalSiblingIndex);
        
        Debug.Log($"✓ Regresado a posición original: {originalPosition}");
    }
}