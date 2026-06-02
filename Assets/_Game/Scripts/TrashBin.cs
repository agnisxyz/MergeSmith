using UnityEngine;
using UnityEngine.EventSystems;

// Çöp kutusu da bir UI elemanı olacak ve üzerine bırakılan eşyaları algılayacak
public class TrashBin : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Üzerimize bırakılan objeyi alıyoruz
        GameObject droppedObj = eventData.pointerDrag;

        if (droppedObj != null)
        {
            DraggableItem draggedItemScript = droppedObj.GetComponent<DraggableItem>();
            
            if (draggedItemScript != null)
            {
                // Eşyanın geldiği hücreyi (Cell) bulalım
                Cell sourceCell = draggedItemScript.originalParent.GetComponentInParent<Cell>();

                if (sourceCell != null && !sourceCell.IsEmpty())
                {
                    // Jeneratörlerin yanlışlıkla silinmesini engelleyelim!
                    if (sourceCell.currentItem.isGenerator)
                    {
                        Debug.Log("Jeneratörleri çöpe atamazsın!");
                        return;
                    }

                    // Oyuncuya teselli ödülü olarak 1-2 altın verebiliriz (Opsiyonel)
                    // Veya sadece yok ederiz. Şimdilik düz silelim.
                    Debug.Log($"{sourceCell.currentItem.itemName} çöpe atıldı ve yok edildi.");

                    // Hücreyi tamamen temizle
                    sourceCell.ClearCell();
                    
                    // Sürüklenen görsel objeyi de sahneden tamamen yok et ki havada kalmasın
                    Destroy(droppedObj);
                }
            }
        }
    }
}