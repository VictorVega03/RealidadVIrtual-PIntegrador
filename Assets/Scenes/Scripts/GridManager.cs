using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public GameObject cellPrefab;
    public Transform gridParent;
    public int rows = 5;
    public int cols = 6;
    public Vector2 gridStartPosition = new Vector2(-230f, 80f);
    
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
            // TAMAÑOS MÁS PEQUEÑOS
            float cellWidth = 15f;   // ← CAMBIAR de 65 a 42
            float cellHeight = 15f;  // ← CAMBIAR de 65 a 42
            float spacingX = 6f;     // ← CAMBIAR de 5 a 3
            float spacingY = 6f;     // ← CAMBIAR de 5 a 3
            
            // Calcular posición
            Vector2 position = new Vector2(
                gridStartPosition.x + (col * (cellWidth + spacingX)),
                gridStartPosition.y - (row * (cellHeight + spacingY))
            );
            
            // Instanciar celda UI
            GameObject cellObj = Instantiate(cellPrefab, gridParent);
            cellObj.name = $"Cell_{row}_{col}";
            
            // Configurar RectTransform
            RectTransform rectTransform = cellObj.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = position;
                rectTransform.sizeDelta = new Vector2(cellWidth, cellHeight);
                Debug.Log($"Cell_{row}_{col} creada en {position}, tamaño: {cellWidth}×{cellHeight}");
            }
            else
            {
                Debug.LogError($"ERROR: Cell_{row}_{col} no tiene RectTransform!");
            }
            
            // Inicializar controlador
            CellController cell = cellObj.GetComponent<CellController>();
            if (cell != null)
            {
                cell.Initialize(row, col, columnValues[col], this);
            }
            else
            {
                Debug.LogError($"ERROR: Cell_{row}_{col} no tiene CellController!");
            }
            
            cells[row, col] = cell;
        }
    }
    
    Debug.Log($"✓ Grid UI creada: {rows}×{cols} = {rows * cols} celdas");
    Debug.Log("========================================\n");
}

    public void OnTreePlaced(int row, int col, bool isOccupied)
    {
        Debug.Log($"\n========== OnTreePlaced en GridManager ==========");
        Debug.Log($"Fila: {row}, Columna: {col}, Ocupada: {isOccupied}");
        
        if (isOccupied && row >= 0 && row < rows && col >= 0 && col < cols)
        {
            int rowValue = row + 1;
            int colValue = columnValues[col];
            int result = rowValue * colValue;
            
            string message = $"Fila {rowValue} × Columna {colValue} = {result}";
            Debug.Log($"Mensaje: {message}");
            
            if (dialogueManager != null)
            {
                dialogueManager.ShowDialogue(message);
            }
            else
            {
                Debug.LogError("DialogueManager es NULL!");
            }
            
            // Notificar al TutorialManager si existe
            TutorialManager tutorial = FindObjectOfType<TutorialManager>();
            if (tutorial != null && tutorial.isTutorialMode)
            {
                tutorial.OnTreePlacedInTutorial();
            }
        }
        else
        {
            Debug.LogWarning($"Posición inválida o no ocupada: ({row}, {col})");
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
        
        Debug.Log($"Puntuación total calculada: {totalScore}");
        return totalScore;
    }

    public bool IsGridFull()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (cells[row, col] != null && !cells[row, col].HasTree())
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void ResetGrid()
    {
        Debug.Log("Reseteando grid...");
        
        if (gridParent != null)
        {
            foreach (Transform child in gridParent)
            {
                Destroy(child.gameObject);
            }
        }
        
        totalScore = 0;
        CreateGrid();
    }


    public CellController FindClosestCell(Vector2 dropPosition)
    {
        Debug.Log($"\n→ FindClosestCell en posición: {dropPosition}");
        
        CellController closestCell = null;
        float closestDistance = float.MaxValue;
        
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
                        float distance = Vector2.Distance(dropPosition, cellPosition);
                        
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
            Debug.Log($"✓ Celda más cercana: {closestCell.name}, distancia: {closestDistance}");
        }
        else
        {
            Debug.Log("⚠ No se encontró ninguna celda");
        }
        
        return closestCell;
    }
}