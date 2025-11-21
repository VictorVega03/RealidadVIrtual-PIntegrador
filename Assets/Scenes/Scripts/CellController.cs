using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CellController : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private int row;
    private int col;
    private int columnValue;
    private GridManager gridManager;
    private bool hasTree = false;
    
    public GameObject treePrefab;
    private GameObject currentTree;

    [Header("Visual Feedback")]
    public Image highlight;
    public Color hoverColor = new Color(0, 1, 0, 0.3f);
    public Color normalColor = new Color(1, 1, 1, 0);

    public void Initialize(int r, int c, int value, GridManager manager)
    {
        row = r;
        col = c;
        columnValue = value;
        gridManager = manager;
        
        Debug.Log($"✓ Cell_{r}_{c} UI inicializada (valor: {value})");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"\n========== OnDrop en Cell_{row}_{col} ==========");
        
        if (hasTree)
        {
            Debug.Log("⚠ Ya hay árbol en esta celda");
            return;
        }
        
        GameObject droppedObject = eventData.pointerDrag;
        
        if (droppedObject == null)
        {
            Debug.Log("⚠ No hay objeto arrastrado");
            return;
        }
        
        Debug.Log($"Objeto soltado: {droppedObject.name}");
        Debug.Log($"Tag del objeto: {droppedObject.tag}");
        
        if (droppedObject.CompareTag("Tree"))
        {
            Debug.Log($"✓ Tag correcto - Plantando árbol");
            PlaceTree();
        }
        else
        {
            Debug.Log($"⚠ Tag incorrecto. Se esperaba 'Tree', se recibió '{droppedObject.tag}'");
        }
    }

    void PlaceTree()
    {
        Debug.Log($"→ PlaceTree en Cell_{row}_{col}");
        hasTree = true;
        
        if (treePrefab == null)
        {
            Debug.LogError("ERROR: treePrefab es NULL!");
            return;
        }
        
        // Instanciar árbol visual en esta celda UI
        currentTree = Instantiate(treePrefab, transform);
        
        // Configurar como hijo UI - MANTENER ESCALA DEL PREFAB
        RectTransform treeRect = currentTree.GetComponent<RectTransform>();
        if (treeRect != null)
        {
            treeRect.anchoredPosition = Vector2.zero;
            // NO cambiar localScale - usar la del prefab (0.12)
            Debug.Log($"✓ Árbol posicionado en celda con escala: {currentTree.transform.localScale}");
        }
        else
        {
            Debug.LogError("ERROR: TreeUI no tiene RectTransform!");
        }
        
        // Notificar al GridManager
        if (gridManager != null)
        {
            gridManager.OnTreePlaced(row, col, true);
        }
        else
        {
            Debug.LogError("ERROR: gridManager es NULL!");
        }
        
        Debug.Log($"✓ Árbol plantado exitosamente en Cell_{row}_{col}");
        
        // Animación
        PlayPlantAnimation();
    }

    void PlayPlantAnimation()
    {
        if (currentTree != null)
        {
            StartCoroutine(ScaleAnimation(currentTree));
        }
    }

    System.Collections.IEnumerator ScaleAnimation(GameObject tree)
    {
        float duration = 0.3f;
        float elapsed = 0f;
        
        // Guardar escala objetivo (la que tiene el prefab)
        Vector3 targetScale = tree.transform.localScale;
        
        // Empezar desde 0
        tree.transform.localScale = Vector3.zero;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            tree.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
            yield return null;
        }
        
        tree.transform.localScale = targetScale;
        Debug.Log($"✓ Animación completada con escala: {targetScale}");
    }

    public bool HasTree()
    {
        return hasTree;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hasTree && highlight != null)
        {
            highlight.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlight != null)
        {
            highlight.color = normalColor;
        }
    }

    public void TryPlaceTree()
    {
    Debug.Log($"\n========== TryPlaceTree en Cell_{row}_{col} ==========");
    
    if (hasTree)
    {
        Debug.Log("⚠ Ya hay árbol en esta celda");
        return;
    }
    
    Debug.Log($"✓ Plantando árbol");
    PlaceTree();
}
}