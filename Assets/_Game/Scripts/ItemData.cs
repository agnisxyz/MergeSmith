using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "MergeGame/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("General Settings")]
    public string id;
    public string itemName;
    public Sprite icon;
    // YENİ EKLENEN SATIR: Eşyanın geçici rengi
    public Color itemColor = Color.white; 
    public int tier;

    [Header("Merge Settings")]
    public ItemData nextTierItem;

    [Header("Generator Settings")]
    public bool isGenerator;
    public ItemData[] possibleDrops;
}