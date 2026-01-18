using UnityEngine;

public class CondicionDeVictoria : MonoBehaviour
{
    public static CondicionDeVictoria Instance;

    public int collectedItems = 0;
    public int itemsToWin = 5;

    public bool HasAllItems()
    {
        return collectedItems >= itemsToWin;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
