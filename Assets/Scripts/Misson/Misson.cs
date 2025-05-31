using UnityEngine;

public enum MissionType
{
    CatchInsects,
    WaterPlants,
    PlantCorn,
    PlantBeans,
    PlantSquash
}

[System.Serializable]
public class Mission
{
    public MissionType type;
    public int level;
    public int progress;
    public int target;
    public int reward;

    public Mission(MissionType type, int level)
    {
        this.type = type;
        this.level = level;
        this.progress = 0;

        switch (type)
        {
            case MissionType.CatchInsects:
                target = level * 5;
                reward = level * 30;
                break;
            case MissionType.WaterPlants:
                target = level * 5;
                reward = level * 20;
                break;
            case MissionType.PlantCorn:
                target = level * 5;
                reward = level * 25;
                break;
            case MissionType.PlantBeans:
                target = level * 5;
                reward = level * 35;
                break;
            case MissionType.PlantSquash:
                target = level * 5;
                reward = level * 50;
                break;
        }
    }

    public bool IsCompleted() => progress >= target;

    public void IncrementProgress()
    {
        progress++;
        if (IsCompleted())
        {
            CurrencyManager.Instance.AddGold(reward);
            level++;
            PlayerPrefs.SetInt("MissionLevel_" + type.ToString(), level);
            PlayerPrefs.Save();
            Debug.Log($"✅ {type} hoàn thành! Nhận {reward} xu. Lên cấp {level}");

            Reset();
        }
    }

    public void Reset()
    {
        progress = 0;
        switch (type)
        {
            case MissionType.CatchInsects:
                target = level * 5;
                reward = level * 30;
                break;
            case MissionType.WaterPlants:
                target = level * 5;
                reward = level * 20;
                break;
            case MissionType.PlantCorn:
                target = level * 5;
                reward = level * 25;
                break;
            case MissionType.PlantBeans:
                target = level * 5;
                reward = level * 35;
                break;
            case MissionType.PlantSquash:
                target = level * 5;
                reward = level * 50;
                break;
        }
    }
}
