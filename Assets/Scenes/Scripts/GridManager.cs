using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int rows = 5;
    public int cols = 6;
    public GameObject cellPrefab;
    public float cellSize = 1f;
    public Vector2 gridStartPosition;

    [Header("Column Values")]
    public int[] columnValues = { 1, 2, 3, 4, 5, 6 }; // Valores de cada columna
    
    [Header("References")]
    public DialogueManager dialogueManager;
    public Transform gridParent;

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
                // Calcular posición UI
                // Ajusta estos valores según tu escala
                float cellSizeUI = cellSize * 110; // Tamaño en píxeles
                Vector2 position = new Vector2(
                    gridStartPosition.x + (col * cellSizeUI),
                    gridStartPosition.y - (row * cellSizeUI)
                );
                
                // Instanciar celda UI
                GameObject cellObj = Instantiate(cellPrefab, gridParent);
                cellObj.name = $"Cell_{row}_{col}";
                
                // Configurar RectTransform
                RectTransform rectTransform = cellObj.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.anchoredPosition = position;
                    rectTransform.sizeDelta = new Vector2(100, 100); // Tamaño de la celda
                    Debug.Log($"Cell_{row}_{col} creada en posición: {position}");
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
    if (isOccupied)
    {
        int value = columnValues[col];
        ShowTutorialMessage(row, col, value);
        
        // Notificar al TutorialManager
        TutorialManager tutorial = FindObjectOfType<TutorialManager>();
        if (tutorial != null && tutorial.isTutorialMode)
        {
            tutorial.OnTreePlacedInTutorial();
        }
    }
}

    void ShowTutorialMessage(int row, int col, int value)
    {
        string message = $"¡Muy bien! Pusiste un árbol en:\n" +
                        $"Fila {row + 1} × Columna con valor {value}\n" +
                        $"Ese árbol vale {value} puntos!";
        
        dialogueManager.ShowDialogue(message);
    }

    public int CalculateTotalScore()
    {
        totalScore = 0;
        
        for (int col = 0; col < cols; col++)
        {
            int treesInColumn = 0;
            for (int row = 0; row < rows; row++)
            {
                if (cells[row, col].HasTree())
                {
                    treesInColumn++;
                }
            }
            totalScore += treesInColumn * columnValues[col];
        }
        
        return totalScore;
    }
}