using UnityEngine;
using System;
using System.Collections.Generic;

// Rastgele üretilen siparişleri tutacak saf C# sınıfımız
[System.Serializable]
public class DynamicOrder
{
    public ItemData item;
    public int amount;
    public int rewardGold;
}

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;
    public static event Action<int> OnGoldChanged; 

    [Header("Order Settings")]
    public int maxActiveOrders = 3; // Aynı anda ekranda olacak maksimum sipariş
    public ItemData[] possibleItemsToOrder; // Jeneratörün sipariş isteyebileceği eşyalar havuzu
    
    [Header("UI References")]
    public GameObject orderCardPrefab; // Kartın kopyası
    public Transform orderContentParent; // Scroll View'in içindeki taşıyıcı

    [Header("Player Data")]
    public int playerGold = 0;     

    // Aktif siparişlerin listesi
    private List<DynamicOrder> activeOrders = new List<DynamicOrder>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Oyun başlarken maksimum sayıya kadar sipariş üret
        for (int i = 0; i < maxActiveOrders; i++)
        {
            GenerateRandomOrder();
        }
        OnGoldChanged?.Invoke(playerGold);
    }

    public void GenerateRandomOrder()
    {
        if (possibleItemsToOrder.Length == 0) return;

        // 1. Rastgele Eşya Seç (Havuzdan)
        ItemData randomItem = possibleItemsToOrder[UnityEngine.Random.Range(0, possibleItemsToOrder.Length)];
        
        // 2. Rastgele Miktar Belirle (1 ile 4 arası)
        int randomAmount = UnityEngine.Random.Range(1, 5);

        // 3. Ödülü Hesapla (Tier * Miktar * 10 altın gibi basit bir formül)
        int calculatedReward = randomItem.tier * randomAmount * 10;

        // 4. Yeni siparişi oluştur ve listeye ekle
        DynamicOrder newOrder = new DynamicOrder
        {
            item = randomItem,
            amount = randomAmount,
            rewardGold = calculatedReward
        };
        
        activeOrders.Add(newOrder);

        // 5. Arayüzde (UI) Kartını Üret
        GameObject newCardObj = Instantiate(orderCardPrefab, orderContentParent);
        OrderCardUI cardUI = newCardObj.GetComponent<OrderCardUI>();
        cardUI.SetupCard(newOrder);
    }

    // Kartın üzerindeki butona basılınca bu çalışacak
    public void CompleteOrder(DynamicOrder completedOrder, GameObject cardGameObject)
    {
        // BoardManager'a git ve eşyaları tahtadan sil
        BoardManager.Instance.RemoveItemsFromBoard(completedOrder.item.id, completedOrder.amount);
        
        // Altını ver
        playerGold += completedOrder.rewardGold;
        OnGoldChanged?.Invoke(playerGold);

        // Siparişi listeden çıkar ve ekrandaki UI kartını yok et
        activeOrders.Remove(completedOrder);
        Destroy(cardGameObject);

        Debug.Log($"Sipariş teslim edildi! {completedOrder.rewardGold} altın kazanıldı.");

        // Hemen yerine yeni bir rastgele sipariş üret! (Sonsuz Döngü)
        GenerateRandomOrder();
    }
}