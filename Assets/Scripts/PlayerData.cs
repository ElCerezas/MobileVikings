using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;
    public HeroData[] deck;
    int Level = 1;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            return;
        }
        else
        {
            instance = this;
        }
    }
    public int GetLevel()
    {
        return Level;
    }
}
