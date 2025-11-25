using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Configuraciones de la cuadrícula 
    [Header("Grid Settings")]
    public GameObject cellPrefab;
    public Transform gridParent;
    public int rows = 5;
    public int cols = 6;
    public Vector2 gridStartPosition = new Vector2(-170f, -55f);
    
    [Header("Column Values")]
    public int[] columnValues = { 1, 2, 3, 4, 5, 6 };
    
    [Header("References")]
    public DialogueManager dialogueManager;
    
    private CellController[,] cells;
    private int totalScore = 0;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
{
    Debug.Log("========== CREANDO GRID UI ==========");
    cells = new CellController[rows, cols];
    
    for (int row = 0; row < rows; row++)
    {
        for (int col = 0; col < cols; col++)
        {
            float cellWidth = 15f;
            float cellHeight = 15f;
            float spacingX = 5f;
            float spacingY = 5f;
            
            // CORREGIDO: Ahora SÍ usa el spacing
            Vector2 position = new Vector2(
                gridStartPosition.x + (col * (cellWidth + spacingX)),
                gridStartPosition.y - (row * (cellHeight + spacingY))
            );
            
            GameObject cellObj = Instantiate(cellPrefab, gridParent);
            cellObj.name = $"Cell_{row}_{col}";
            
            RectTransform rectTransform = cellObj.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = position;
                rectTransform.sizeDelta = new Vector2(cellWidth, cellHeight);
            }
            
            CellController cell = cellObj.GetComponent<CellController>();
            if (cell != null)
            {
                cell.Initialize(row, col, columnValues[col], this);
            }
            
            cells[row, col] = cell;
        }
    }
    
    Debug.Log($"✓ Grid UI creada: {rows}×{cols} = {rows * cols} celdas");
}

   public void OnTreePlaced(int row, int col, bool isOccupied)
{
    if (isOccupied && row >= 0 && row < rows && col >= 0 && col < cols)
    {
        int rowValue = row + 1;
        int colValue = columnValues[col];
        int result = rowValue * colValue;
        
        string message = $"Fila {rowValue} × Columna {colValue} = {result}";
        
        if (dialogueManager != null)
        {
            // Mostrar por 2 segundos (mensaje corto)
            dialogueManager.ShowDialogue(message, false, 2f);
        }
        
        Level3Manager levelManager = FindObjectOfType<Level3Manager>();
        if (levelManager != null && levelManager.isTutorialMode)
        {
            levelManager.OnTreePlacedInTutorial();
        }
    }
}


    public int CalculateTotalScore()
    {
        totalScore = 0;
        
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (cells[row, col] != null && cells[row, col].HasTree())
                {
                    int rowValue = row + 1;
                    int colValue = columnValues[col];
                    totalScore += (rowValue * colValue);
                }
            }
        }
        
        Debug.Log($"Puntuacion total calculada: {totalScore}");
        return totalScore;
    }

  public CellController FindClosestCell(Vector2 dropPosition)
{
    CellController closestCell = null;
    float closestDistance = float.MaxValue;
    
    // Convertir la posición del árbol al espacio local del GridParent
    Vector2 localDropPosition = gridParent.InverseTransformPoint(dropPosition);
    
    Debug.Log($"\n→ FindClosestCell");
    Debug.Log($"  Posicion original: {dropPosition}");
    Debug.Log($"  Posicion local (GridParent): {localDropPosition}");
    
    for (int row = 0; row < rows; row++)
    {
        for (int col = 0; col < cols; col++)
        {
            if (cells[row, col] != null)
            {
                RectTransform cellRect = cells[row, col].GetComponent<RectTransform>();
                if (cellRect != null)
                {
                    Vector2 cellPosition = cellRect.anchoredPosition;
                    float distance = Vector2.Distance(localDropPosition, cellPosition);
                    
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestCell = cells[row, col];
                    }
                }
            }
        }
    }
    
    if (closestCell != null)
    {
        Debug.Log($"✓ Celda mas cercana: {closestCell.name}, distancia: {closestDistance}");
    }
    else
    {
        Debug.Log("⚠ No se encontro ninguna celda");
    }
    
    return closestCell;
}
}