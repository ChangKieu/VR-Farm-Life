using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Plant : MonoBehaviour
{
    public PlantData plantData;
    [SerializeField] private GameObject currentPlantInstance;
    [SerializeField] private int currentStage;
    [SerializeField] private float elapsedTime;
    [SerializeField] private bool isDead, isDestroy;
    [SerializeField] private bool hasPest;
    [SerializeField] private GameObject insectPrefeb, insectCurr;
    [SerializeField] private int harvestValue;
    private Coroutine wateringCoroutine;
    public Land landScript;

    void Start()
    {
        harvestValue = plantData.harvestValue;
        if (insectCurr)
        {
            Destroy(insectCurr);
        }
        StartCoroutine(GrowPlant());
    }

    IEnumerator GrowPlant()
    {
        if (isDestroy) yield return null;
        while (elapsedTime < plantData.growthTime)
        {

            if (currentStage < plantData.growthStages.Count && elapsedTime >= plantData.growthTime / (plantData.growthStages.Count + 1) * (currentStage + 1))
            {
                Debug.Log(plantData.growthStages.Count);
                ChangeGrowthStage();
            }

            if (plantData.waterTimes.Contains(elapsedTime))
            {
                isDead =true;
                if (wateringCoroutine != null)
                    StopCoroutine(wateringCoroutine);
                wateringCoroutine = StartCoroutine(WateringCountdown(30f));
            }

            if (!hasPest && elapsedTime == plantData.pestTime)
            {
                hasPest = true;
                Debug.Log("Sâu bệnh xuất hiện!");
                insectCurr = Instantiate(insectPrefeb, transform.position, Quaternion.identity, transform);
                insectCurr.GetComponent<XRGrabInteractable>().selectEntered.AddListener((SelectEnterEventArgs args) =>
                {
                    RemovePest(); 
                });
                StartCoroutine(PestCountdown(30f));
            }

            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;
        }

    }

    void ChangeGrowthStage()
    {
        Debug.Log(currentStage);
        if (currentStage < plantData.growthStages.Count)
        {
            if (currentPlantInstance != null)
            {
                Destroy(currentPlantInstance);
            }

            currentPlantInstance = Instantiate(plantData.growthStages[currentStage], transform.position, Quaternion.identity, transform);

            currentStage++;
        }
        ;
    }

    IEnumerator WateringCountdown(float time)
    {
        Debug.Log("Cây cần được tưới nước!");
        landScript.TextOn("Hãy tưới nước!");
        yield return new WaitForSeconds(time);
        if (isDead)
        {
            landScript.TextOn("Cây hỏng vì thiếu nước!");
            Debug.Log("Cây bị chết vì thiếu nước!");
            isDestroy = true;
            Renderer[] renderers = currentPlantInstance.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderers)
            {
                rend.material.color = new Color(0.6f, 0.6f, 0f); 
            }
        }
    }

    IEnumerator PestCountdown(float time)
    {
        yield return new WaitForSeconds(time);
        if (hasPest)
        {
            harvestValue = (int)(plantData.harvestValue * 0.5f);
        }
    }

    public void WaterPlant()
    {
        Debug.Log("Cây đã được tưới nước!");
        landScript.TextOff();
        MissionManager.Instance.AddProgress(MissionType.WaterPlants);
        if (wateringCoroutine != null)
            StopCoroutine(wateringCoroutine);
    }

    public void RemovePest()
    {
        hasPest = false;
        MissionManager.Instance.AddProgress(MissionType.CatchInsects);
        Destroy(insectCurr);
    }

    public void Harvest()
    {
        if (elapsedTime >= plantData.growthTime && !isDestroy)
        {
            Debug.Log("Thu hoạch cây, nhận " + harvestValue + " vàng!");
            CurrencyManager.Instance.AddGold(harvestValue);
            AudioManager.Instance.PlayHarvest();
            Destroy(gameObject);
            landScript.ResetLand();

        }
        else if(isDestroy)
        {
            Debug.Log("Cây đã chết, không thể thu hoạch!");
            Destroy(gameObject);
            AudioManager.Instance.PlayHarvest();
            landScript.ResetLand();
        }
    }
    public void DestroyPlant()
    {
        if (currentPlantInstance != null)
        {
            Destroy(currentPlantInstance);
        }
    }
    public void SetInsectPrefab(GameObject insect)
    {
        insectPrefeb = insect;
    }    
}
