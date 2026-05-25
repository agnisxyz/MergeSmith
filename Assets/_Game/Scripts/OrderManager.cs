using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    [Header("Current Active Order")]
    public OrderData currentOrder; // Şu an ekranda oyuncudan istenen sipariş
    
    // Şimdilik UI olmadığı için altını burada bir değişkende tutalım
    public int playerGold = 0;     

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Oyuncu "Teslim Et" butonuna basınca (veya testi tetiklediğimizde) bu çalışacak
    public void TryCompleteOrder()
    {
        if (currentOrder == null)
        {
            Debug.Log("Aktif bir sipariş yok!");
            return;
        }

        // Adım 1: Tahtada bu siparişi karşılayacak eşyalar var mı kontrol et
        if (CheckIfOrderCanBeCompleted())
        {
            // Adım 2: Varsa, tahtadaki o eşyaları bul ve sil
            ConsumeOrderItems();
            
            // Adım 3: Ödülü ver
            playerGold += currentOrder.goldReward;
            Debug.Log($"Sipariş Tamamlandı! {currentOrder.goldReward} Altın kazanıldı. Toplam Altın: {playerGold}");
            
            // (İleride burada currentOrder'ı silip yeni rastgele bir sipariş getireceğiz)
        }
        else
        {
            Debug.Log("Siparişi tamamlamak için tahtada yeterli eşya yok!");
        }
    }

    private bool CheckIfOrderCanBeCompleted()
    {
        // Siparişteki her bir gereksinimi (Örn: 2 tane Külçe, 1 tane Kılıç) tek tek kontrol et
        foreach (OrderRequirement req in currentOrder.requirements)
        {
            // Tahtada bu eşyadan kaç tane olduğunu BoardManager'a sor (Bunu bir sonraki adımda yazacağız)
            int countOnBoard = BoardManager.Instance.CountItemOnBoard(req.requiredItem.id);
            
            if (countOnBoard < req.amount)
            {
                return false; // Yeterli eşya yok, direkt iptal et
            }
        }
        return true; // Bütün döngüyü hatasız geçerse eşyalar tam demektir!
    }

    private void ConsumeOrderItems()
    {
        // Eşyaları tahtadan silmek için yine BoardManager'dan yardım isteyeceğiz
        foreach (OrderRequirement req in currentOrder.requirements)
        {
            BoardManager.Instance.RemoveItemsFromBoard(req.requiredItem.id, req.amount);
        }
    }
}