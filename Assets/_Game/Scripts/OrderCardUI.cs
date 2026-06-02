using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderCardUI : MonoBehaviour
{
    [Header("UI References")]
    public Image reqItemIcon;      
    public TextMeshProUGUI reqAmountText;     
    public Button deliverButton;   
    public TextMeshProUGUI rewardText; // Altın miktarını gösterecek yazı

    // Bu kartın takip ettiği dinamik sipariş verisi
    [HideInInspector] public DynamicOrder myOrder; 

    private void Start()
    {
        deliverButton.onClick.AddListener(OnDeliverButtonClicked);
    }

    // Kart ilk oluşturulduğunda OrderManager buraya veriyi gönderecek
    public void SetupCard(DynamicOrder orderData)
    {
        myOrder = orderData;
        reqItemIcon.sprite = myOrder.item.icon;
        reqItemIcon.color = myOrder.item.itemColor;
        rewardText.text = myOrder.rewardGold.ToString();
    }

    private void Update()
    {
        if (myOrder == null) return;

        // Tahtada istenen eşyadan kaç tane var kontrol et
        int currentAmount = BoardManager.Instance.CountItemOnBoard(myOrder.item.id);

        reqAmountText.text = $"{currentAmount} / {myOrder.amount}";

        // Yeterli eşya varsa butonu aktif et
        deliverButton.interactable = currentAmount >= myOrder.amount;
    }

    private void OnDeliverButtonClicked()
    {
        // Teslim et butonuna basıldığında OrderManager'a "Benim siparişimi tamamla" diye haber ver
        OrderManager.Instance.CompleteOrder(myOrder, this.gameObject);
    }
}