using UnityEngine;

public class AppleSpawnerLevel2 : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject goodApplePrefab; // Manzana buena
    public GameObject badApplePrefab;  // Manzana con gusano

    [Header("Área de Generación (Mantel)")]
    public float minX = -6f;
    public float maxX = 6f;
    public float minY = -2f;
    public float maxY = 2f;
    public float spacing = 1.2f; // Espacio entre manzanas

    [Header("Configuración de Prueba")]
    public int defaultAppleCount = 10; // NUEVO: Cantidad por defecto para pruebas
    public bool useDefaultCount = true; // NUEVO: Usar cantidad por defecto si no hay puntaje previo

    public void SpawnAllApples()
    {
        int totalApples = GameConfig.appleCountFromLevel1;

        if (totalApples <= 0 || useDefaultCount)
        {
            totalApples = defaultAppleCount;
            Debug.LogWarning($"No hay puntaje del nivel anterior. Usando valor por defecto: {totalApples} manzanas");
        }
        
        // Validar que el total sea al menos 2 (para tener al menos 1 buena y 1 mala)
        totalApples = Mathf.Max(2, totalApples);
        
        // Calcular cuántas manzanas malas (30-40% del total)
        int badAppleCount = Mathf.RoundToInt(totalApples * Random.Range(0.3f, 0.4f));
        badAppleCount = Mathf.Clamp(badAppleCount, 1, totalApples - 1); // Al menos 1 mala, pero no todas
        
        int goodAppleCount = totalApples - badAppleCount;
        
        GameConfig.badApplesRemaining = badAppleCount;

        Debug.Log($"Generando {totalApples} manzanas: {goodAppleCount} buenas, {badAppleCount} malas");

        // Crear una lista mezclada de tipos de manzanas
        bool[] appleTypes = new bool[totalApples]; // true = buena, false = mala
        for (int i = 0; i < badAppleCount; i++)
        {
            appleTypes[i] = false; // Manzanas malas
        }
        for (int i = badAppleCount; i < totalApples; i++)
        {
            appleTypes[i] = true; // Manzanas buenas
        }

        // Mezclar el array (Algoritmo Fisher-Yates)
        for (int i = totalApples - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            bool temp = appleTypes[i];
            appleTypes[i] = appleTypes[randomIndex];
            appleTypes[randomIndex] = temp;
        }

        // Generar las manzanas en una cuadrícula
        int columns = Mathf.CeilToInt(Mathf.Sqrt(totalApples));
        int rows = Mathf.CeilToInt((float)totalApples / columns);

        int appleIndex = 0;
        for (int row = 0; row < rows && appleIndex < totalApples; row++)
        {
            for (int col = 0; col < columns && appleIndex < totalApples; col++)
            {
                float x = minX + (col * spacing);
                float y = maxY - (row * spacing);
                
                Vector3 position = new Vector3(x, y, 0);
                
                // Instanciar manzana buena o mala según el array mezclado
                GameObject prefabToUse = appleTypes[appleIndex] ? goodApplePrefab : badApplePrefab;
                GameObject apple = Instantiate(prefabToUse, position, Quaternion.identity);
                
                // Agregar componente de arrastre
                apple.AddComponent<DraggableApple>();
                
                appleIndex++;
            }
        }
    }
}