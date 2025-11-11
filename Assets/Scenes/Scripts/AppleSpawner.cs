using UnityEngine;
using System.Collections;

public class AppleSpawner : MonoBehaviour
{
    [Header("Configuración del Prefab")]
    public GameObject applePrefab; // Arrastra tu Prefab de Manzana aquí

    [Header("Área General de Generación")]
    public float minX = -5.5f; // Límite X izquierdo de la zona del árbol
    public float maxX = 5.5f;  // Límite X derecho de la zona del árbol
    public float spawnY = 3.5f; // Altura máxima de spawn (dentro de la copa)
    public float minY = 2.0f;  // Altura mínima de spawn (dentro de la copa)

    [Header("Frecuencia y Cantidad")]
    public float spawnInterval = 1.5f; // Tiempo entre cada intento de aparición

    [Header("Configuración del Árbol")]
    public string treeTag = "TreeArea"; // Tag que debe tener el objeto del árbol
    private Collider2D treeCollider;

    [Header("Configuración de Caída")]
    public float staticDelay = 0.5f; // Pausa antes de la caída (0.5 segundos)
    public float maxSpinForce = 500f; // Fuerza máxima de giro para la caída

    private bool isSpawning = false;

    void Start()
    {
        // Encontrar el collider del árbol al inicio del juego
        GameObject tree = GameObject.FindGameObjectWithTag(treeTag);
        if (tree != null)
        {
            treeCollider = tree.GetComponent<Collider2D>();
        }
        
        if (treeCollider == null)
        {
            Debug.LogError("Objeto con el Tag '" + treeTag + "' NO ENCONTRADO o le falta un Collider2D. Las manzanas no se generarán correctamente dentro del área.");
        }
    }

    // Función llamada por el GameManager para iniciar la generación
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnApplesRoutine());
        }
    }

    // Coroutine principal para generar manzanas de forma continua
    IEnumerator SpawnApplesRoutine()
    {
        while (isSpawning)
        {
            // Intentar encontrar una posición válida dentro del árbol
            Vector3 spawnPosition = FindValidSpawnPosition();

            if (spawnPosition != Vector3.zero) // Si encontramos una posición válida
            {
                // Iniciar la coroutine para manejar la pausa y el giro
                StartCoroutine(SpawnAppleWithDelayAndSpin(spawnPosition));
            }

            // Esperar el tiempo de intervalo antes del siguiente intento
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Busca una posición aleatoria dentro de los límites X/Y que también esté dentro del Collider del árbol
    Vector3 FindValidSpawnPosition()
    {
        // Intentamos encontrar una posición válida hasta 30 veces para evitar un bucle infinito
        for (int i = 0; i < 30; i++) 
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, spawnY); 
            Vector2 testPosition = new Vector2(randomX, randomY);

            // Verificar si la posición está DENTRO del Collider del árbol
            if (treeCollider != null && treeCollider.OverlapPoint(testPosition))
            {
                return testPosition; // ¡Posición válida encontrada!
            }
        }
        
        // Si no encontramos una posición válida después de 30 intentos, devolvemos Vector3.zero
        return Vector3.zero; 
    }

    // Coroutine para manejar la pausa, el inicio de la caída y el giro
    IEnumerator SpawnAppleWithDelayAndSpin(Vector3 spawnPosition)
    {
        // 1. Crear la manzana
        GameObject appleObject = Instantiate(applePrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D appleRb = appleObject.GetComponent<Rigidbody2D>();

        if (appleRb != null)
        {
            // 2. Congelar la manzana al generarse (isKinematic = true ignora las fuerzas)
            appleRb.isKinematic = true; 
            appleRb.linearVelocity = Vector2.zero;
            appleRb.angularVelocity = 0f;

            // 3. Esperar el tiempo de pausa (0.5 segundos)
            yield return new WaitForSeconds(staticDelay);

            // 4. Iniciar la caída y el giro
            appleRb.isKinematic = false; // Dejar que la gravedad actúe
            
            // Añadir una fuerza de giro aleatoria para una caída más dinámica
            float randomSpin = Random.Range(-maxSpinForce, maxSpinForce);
            appleRb.AddTorque(randomSpin);
        }
    }

    // Opcional: Para detener la generación de manzanas
    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }
}