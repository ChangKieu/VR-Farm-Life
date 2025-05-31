using UnityEngine;

[CreateAssetMenu(fileName = "SeedItem", menuName = "Shop/Seed Item")]
public class SeedItem : ScriptableObject
{
    public string seedName;       
    public Sprite icon;          
    public int price;
}
