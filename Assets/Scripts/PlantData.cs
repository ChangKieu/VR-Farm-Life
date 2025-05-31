using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantData", menuName = "Scriptable Objects/PlantData")]
public class PlantData : ScriptableObject
{
    public string plantName; 
    public float growthTime; 
    public List<GameObject> growthStages; 
    public List<float> waterTimes; 
    public float pestTime; 
    public int harvestValue;
}
