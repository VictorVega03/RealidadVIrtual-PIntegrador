using UnityEngine;

public class GameConfig : MonoBehaviour
{
    public enum Level { Level1, Level2 }
    public static Level currentLevel = Level.Level1;
    
    // Variables para el Nivel 2
    public static int appleCountFromLevel1 = 10; // Manzanas buenas del nivel 1
    public static int badApplesRemaining = 0;   // Manzanas malas a eliminar en nivel 2
}
