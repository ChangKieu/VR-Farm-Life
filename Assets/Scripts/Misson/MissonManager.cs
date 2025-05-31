using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    public Transform missionListContainer;
    public GameObject missionPrefab;

    public List<Mission> missions = new List<Mission>();
    private Dictionary<MissionType, GameObject> missionUIMap = new Dictionary<MissionType, GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;

        LoadMissions();
        GenerateMissionUI();
    }

    public void LoadMissions()
    {
        missions.Clear();

        foreach (MissionType type in System.Enum.GetValues(typeof(MissionType)))
        {
            int level = PlayerPrefs.GetInt("MissionLevel_" + type, 1);
            int progress = PlayerPrefs.GetInt("MissionProgress_" + type, 0);

            Mission mission = new Mission(type, level);
            mission.progress = progress;

            missions.Add(mission);
        }
    }

    public void SaveMission(Mission mission)
    {
        PlayerPrefs.SetInt("MissionLevel_" + mission.type, mission.level);
        PlayerPrefs.SetInt("MissionProgress_" + mission.type, mission.progress);
        PlayerPrefs.Save();
    }

    public void AddProgress(MissionType type, int amount = 1)
    {
        Mission mission = missions.Find(m => m.type == type);
        if (mission == null) return;

        mission.progress += amount;
        mission.progress = Mathf.Min(mission.progress, mission.target);

        UpdateSingleMissionUI(mission);

        if (mission.progress >= mission.target)
        {
            ShowCompleteButton(mission);
        }
        else
        {
            SaveMission(mission);
        }
    }

    private void ShowCompleteButton(Mission mission)
    {
        if (!missionUIMap.TryGetValue(mission.type, out GameObject item)) return;

        Button completeBtn = item.transform.Find("ButtonComplete").GetComponent<Button>();
        TextMeshProUGUI btnText = completeBtn.GetComponentInChildren<TextMeshProUGUI>();

        completeBtn.gameObject.SetActive(true);
        btnText.text = $"+{mission.reward}";

        completeBtn.onClick.RemoveAllListeners();

        completeBtn.onClick.AddListener(() =>
        {
            CurrencyManager.Instance.AddGold(mission.reward);

            AudioManager.Instance.PlayButton();
            mission.level++;
            mission.progress = 0;

            SaveMission(mission);

            Mission newMission = new Mission(mission.type, mission.level);
            int idx = missions.IndexOf(mission);
            missions[idx] = newMission;

            SaveMission(newMission);
            UpdateSingleMissionUI(newMission);

            completeBtn.gameObject.SetActive(false);
        });
    }

    public void GenerateMissionUI()
    {
        missionUIMap.Clear();

        foreach (Transform child in missionListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Mission mission in missions)
        {
            GameObject item = Instantiate(missionPrefab, missionListContainer);
            TextMeshProUGUI title = item.GetComponent<TextMeshProUGUI>();
            Transform progressTF = item.transform.Find("ProgressText");
            TextMeshProUGUI progressText = progressTF.GetComponent<TextMeshProUGUI>();


            title.text = GetMissionTitle(mission);
            progressText.text = $"{mission.progress} / {mission.target}";

            Slider progressBar = item.transform.Find("SliderProgressBar").GetComponent<Slider>();

            progressBar.value = (float)mission.progress / mission.target;
            Button completeBtn = item.transform.Find("ButtonComplete").GetComponent<Button>();
            completeBtn.gameObject.SetActive(false);

            missionUIMap[mission.type] = item;
        }
    }

    private void UpdateSingleMissionUI(Mission mission)
    {
        if (!missionUIMap.TryGetValue(mission.type, out GameObject item)) return;

        TextMeshProUGUI title = item.GetComponent<TextMeshProUGUI>();
        Transform progressTF = item.transform.Find("ProgressText");
        TextMeshProUGUI progressText = progressTF.GetComponent<TextMeshProUGUI>();

        title.text = GetMissionTitle(mission);
        progressText.text = $"{mission.progress} / {mission.target}";

        Slider progressBar = item.transform.Find("SliderProgressBar").GetComponent<Slider>();
        progressBar.value = (float)mission.progress / mission.target;

        Button completeBtn = item.transform.Find("ButtonComplete").GetComponent<Button>();
        if (mission.progress < mission.target)
        {
            completeBtn.gameObject.SetActive(false);
        }
        else
        {
            ShowCompleteButton(mission);
        }
    }

    private string GetMissionTitle(Mission mission)
    {
        int target = mission.target;
        return mission.type switch
        {
            MissionType.CatchInsects => $"Bắt {target} sâu",
            MissionType.WaterPlants => $"Tưới {target} lần",
            MissionType.PlantCorn => $"Trồng {target} cây ngô",
            MissionType.PlantBeans => $"Trồng {target} cây đậu",
            MissionType.PlantSquash => $"Trồng {target} cây bí",
            _ => "Nhiệm vụ"
        };
    }

    public void ResetAllMissions()
    {
        foreach (MissionType type in System.Enum.GetValues(typeof(MissionType)))
        {
            PlayerPrefs.DeleteKey("MissionLevel_" + type);
            PlayerPrefs.DeleteKey("MissionProgress_" + type);
        }
        PlayerPrefs.Save();
    }
}
