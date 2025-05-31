using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public int currentGold;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        LoadGold();
    }

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            SaveGold();
            return true;
        }
        return false;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        SaveGold();
    }

    public void SaveGold()
    {
        PlayerPrefs.SetInt("Gold", currentGold);
        PlayerPrefs.Save();
    }

    public void LoadGold()
    {
        currentGold = PlayerPrefs.GetInt("Gold", 100); 
    }
}
