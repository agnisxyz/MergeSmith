using UnityEngine;

// Bu satır, Unity editöründe sağ tıklayıp yeni bir eşya yaratmamızı sağlayan bir menü ekler.
[CreateAssetMenu(fileName = "NewItemData", menuName = "MergeGame/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("General Settings")]
    public string id;              // Kodu yazarken bu eşyayı bulmak için kullanacağımız eşsiz ID (Örn: "sword_tier1")
    public string itemName;        // Oyuncunun ekranda göreceği isim
    public Sprite icon;            // Eşyanın 2D görseli
    public int tier;               // Eşyanın seviyesi (1, 2, 3...)

    [Header("Merge Settings")]
    // Bu eşya başka bir eşyayla birleşirse (merge), ortaya ne çıkacak? Son seviyeyse boş kalır.
    public ItemData nextTierItem;  

    [Header("Generator Settings")]
    public bool isGenerator;       // Bu eşya tıklandığında yeni eşyalar üreten bir kaynak mı?
    
    // Eğer bu bir kaynaksa (isGenerator = true), tıklandığında hangi eşyaları düşürebilir?
    public ItemData[] possibleDrops; 
}