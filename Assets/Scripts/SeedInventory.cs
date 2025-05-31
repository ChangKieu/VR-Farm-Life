using UnityEngine;

public static class SeedInventory
{
    public static void AddSeed(string seedName, int quantity)
    {
        int current = PlayerPrefs.GetInt(GetKey(seedName), 0);
        PlayerPrefs.SetInt(GetKey(seedName), current + quantity);
        PlayerPrefs.Save();
    }

    public static int GetSeedCount(string seedName)
    {
        return PlayerPrefs.GetInt(GetKey(seedName), 0);
    }

    public static bool UseSeed(string seedName)
    {
        int current = GetSeedCount(seedName);
        if (current > 0)
        {
            PlayerPrefs.SetInt(GetKey(seedName), current - 1);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    public static void ClearSeed(string seedName)
    {
        PlayerPrefs.DeleteKey(GetKey(seedName));
        PlayerPrefs.Save();
    }

    private static string GetKey(string seedName)
    {
        return "Seed_" + seedName;
    }
}
