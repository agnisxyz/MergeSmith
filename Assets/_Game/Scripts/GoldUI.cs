using UnityEngine;
using TMPro; // TMPro kullandığımız için bunu unutmuyoruz

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    // Obje aktifleştiğinde haberi dinlemeye (abone olmaya) başla
    private void OnEnable()
    {
        OrderManager.OnGoldChanged += UpdateGoldText;
    }

    // Obje kapanırsa (veya silinirse) dinlemeyi bırak ki hafıza sızıntısı (memory leak) olmasın
    private void OnDisable()
    {
        OrderManager.OnGoldChanged -= UpdateGoldText;
    }

    private void Start()
    {
        // Oyun ilk başladığında mevcut altını ekrana yaz
        if (OrderManager.Instance != null)
        {
            UpdateGoldText(OrderManager.Instance.playerGold);
        }
    }

    // Olay (Event) tetiklendiğinde otomatik olarak çalışacak fonksiyon
    private void UpdateGoldText(int newGoldAmount)
    {
        goldText.text = newGoldAmount.ToString();
    }
}