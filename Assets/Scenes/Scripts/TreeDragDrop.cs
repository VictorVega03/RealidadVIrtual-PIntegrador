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
        Debug.Log($"Posicion final: {rectTransform.anchoredPosition}");
        
        // Detección manual: Buscar celda más cercana
        GridManager gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null)
        {
            Vector2 dropPosition = rectTransform.anchoredPosition;
            CellController closestCell = gridManager.FindClosestCell(dropPosition);
            
            if (closestCell != null && !closestCell.HasTree())
            {
                Debug.Log($"✓ Celda valida encontrada - Plantando");
                closestCell.TryPlaceTree();
                wasPlaced = true;
                
                // Ocultar este árbol arrastrable
                gameObject.SetActive(false);
                Debug.Log("✓ Arbol arrastrable ocultado");
            }
            else
            {
                Debug.Log("⚠ Celda no valida o ya ocupada");
            }
        }
        
        // Solo regresar si NO fue plantado
        if (!wasPlaced)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            rectTransform.anchoredPosition = originalPosition;
            transform.SetSiblingIndex(originalSiblingIndex);
            Debug.Log($"✓ Regresado a posicion original: {originalPosition}");
        }
    }
}