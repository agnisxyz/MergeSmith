using UnityEngine;

// Siparişte hangi eşyadan KAÇ TANE istendiğini tutan küçük bir sınıf
[System.Serializable]
public class OrderRequirement
{
    public ItemData requiredItem; // Hangi eşya isteniyor? (Örn: IronBar_Tier2)
    public int amount;            // Kaç tane isteniyor? (Örn: 2)
}

[CreateAssetMenu(fileName = "NewOrder", menuName = "MergeGame/Order Data")]
public class OrderData : ScriptableObject
{
    [Header("Order Settings")]
    public string orderName;      // Siparişin adı (Örn: "Şövalye Siparişi")
    
    // YENİ BİR TASARIM DESENİ KULLANMA ZAMANI! 
    public OrderRequirement[] requirements; // İstenen eşyaların listesi
    
    public int goldReward;        // Tamamlanınca verilecek altın
}