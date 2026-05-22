using UnityEngine;
using UnityEngine.UI; // Arayüz (Image) bileşenlerini kullanabilmek için bu kütüphaneyi ekliyoruz.

public class Cell : MonoBehaviour
{
    [Header("UI Elements")]
    public Image itemIcon;         // Hücrenin içindeki eşyanın resmi

    [Header("Cell Data")]
    public Vector2Int gridPosition; // Hücrenin tahtadaki (X, Y) koordinatı
    public ItemData currentItem;    // Hücrenin içindeki eşyanın verisi (Boşsa null olur)

    // Hücreye yeni bir eşya koyduğumuzda veya güncellediğimizde çalışacak fonksiyon
    public void SetItem(ItemData newItemData)
    {
        currentItem = newItemData;

        // Eğer hücreye bir eşya verildiyse
        if (currentItem != null)
        {
            itemIcon.sprite = currentItem.icon; // UI'daki resmi, eşyanın resmiyle değiştir
            itemIcon.gameObject.SetActive(true); // Resmi görünür yap
        }
        else // Eğer hücreye null geldiyse (Yani hücre boşaltıldıysa)
        {
            itemIcon.sprite = null;
            itemIcon.gameObject.SetActive(false); // Resmi gizle
        }
    }

    // Hücreyi tamamen boşaltmak için bir yardımcı fonksiyon
    public void ClearCell()
    {
        SetItem(null);
    }

    // Hücre boş mu dolu mu diye sormak istediğimizde kullanacağız
    public bool IsEmpty()
    {
        return currentItem == null;
    }
}