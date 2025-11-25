using UnityEngine;

public class DivisionGenerator : MonoBehaviour
{
    [Header("Rango de Dificultad")]
    [Tooltip("Número mínimo de amigos")]
    public int minFriends = 2;
    [Tooltip("Número máximo de amigos")]
    public int maxFriends = 4;
    
    [Tooltip("Mínimo de pizzas por amigo")]
    public int minPizzasPerFriend = 2;
    [Tooltip("Máximo de pizzas por amigo")]
    public int maxPizzasPerFriend = 5;
    
    [Header("Debug")]
    public bool showDebugInfo = true;

    void Awake()
    {
        GenerateRandomDivision();
    }

    public void GenerateRandomDivision()
    {
        // Generar número aleatorio de amigos
        int friends = Random.Range(minFriends, maxFriends + 1);
        
        // Generar número aleatorio de pizzas por amigo
        int pizzasPerFriend = Random.Range(minPizzasPerFriend, maxPizzasPerFriend + 1);
        
        // Calcular total de pizzas (siempre será divisible exactamente)
        int totalPizzas = friends * pizzasPerFriend;
        
        // Guardar en GameConfig
        GameConfig.numberOfFriends = friends;
        GameConfig.totalApples = totalPizzas;
        
        if (showDebugInfo)
        {
            Debug.Log($"🎲 División generada: {totalPizzas} ÷ {friends} = {pizzasPerFriend}");
        }
        
        // Actualizar los FriendControllers con el nuevo maxApples
        UpdateFriendControllers(pizzasPerFriend);
    }
    
    void UpdateFriendControllers(int pizzasPerFriend)
    {
        FriendController[] allFriends = FindObjectsOfType<FriendController>();
        
        foreach (FriendController friend in allFriends)
        {
            friend.maxApples = pizzasPerFriend;
            
            if (showDebugInfo)
            {
                Debug.Log($"📝 {friend.gameObject.name} ahora debe recibir {pizzasPerFriend} pizzas");
            }
        }
    }
}