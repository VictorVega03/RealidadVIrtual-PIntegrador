using UnityEngine;
using UnityEngine.EventSystems;

public class TreeDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;

    void Awake()
    {
        Debug.Log($"TreeDragDrop Awake en {gameObject.name}");
        
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        
        if (rectTransform == null)
            Debug.LogError($"ERROR: {gameObject.name} no tiene RectTransform!");
        
        if (canvasGroup == null)
            Debug.LogError($"ERROR: {gameObject.name} no tiene CanvasGroup!");
        
        if (canvas == null)
            Debug.LogError($"ERROR: {gameObject.name} no encuentra Canvas padre!");
        
        originalPosition = rectTransform.anchoredPosition;
        
        Debug.Log($"✓ {gameObject.name} inicializado correctamente");
    }

    void Start()
    {
        // Verificar tag
        if (!gameObject.CompareTag("Tree"))
        {
            Debug.LogError($"ERROR: {gameObject.name} NO tiene tag 'Tree'!");
        }
        else
        {
            Debug.Log($"✓ {gameObject.name} tiene tag 'Tree' correcto");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"========== OnBeginDrag en {gameObject.name} ==========");
        
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
            Debug.Log("✓ Alpha reducido, raycasts desactivados");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (rectTransform != null && canvas != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"========== OnEndDrag en {gameObject.name} ==========");
        Debug.Log($"Posición final: {rectTransform.anchoredPosition}");
        
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
        
        // Regresa a posición original
        rectTransform.anchoredPosition = originalPosition;
        Debug.Log($"✓ Regresado a posición original: {originalPosition}");
    }

    // Método para testear desde Inspector
    void OnMouseDown()
    {
        Debug.Log($"CLICK detectado en {gameObject.name}!");
    }
}
