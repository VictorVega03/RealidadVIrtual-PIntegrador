using UnityEngine;

public class AppleSpawnerLevel4 : MonoBehaviour
{
    public GameObject applePrefab;
    public Vector2 centerPosition = new Vector2(0, 0);
    public float spacing = 0.8f;

    public void SpawnAllApples()
    {
        // Siempre usar valores de GameConfig 
        int apples = GameConfig.totalApples;
        
        Debug.Log($"Generando {apples} pizzas");
        
        // Genera en cuadrícula compacta
        int columns = Mathf.CeilToInt(Mathf.Sqrt(apples));
        
        int appleIndex = 0;
        for (int row = 0; row < columns && appleIndex < apples; row++)
        {
            for (int col = 0; col < columns && appleIndex < apples; col++)
            {
                float x = centerPosition.x + (col - columns/2f) * spacing;
                float y = centerPosition.y + (row - columns/2f) * spacing;
                
                GameObject apple = Instantiate(applePrefab, new Vector3(x, y, 0), Quaternion.identity);
                apple.AddComponent<DraggableAppleLevel4>();
                
                appleIndex++;
            }
        }
    }
}