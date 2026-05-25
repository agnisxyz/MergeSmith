using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IDropHandler
{
    [Header("UI Elements")]
    public Image itemIcon;

    [Header("Cell Data")]
    public Vector2Int gridPosition;
    public ItemData currentItem;

    public void SetItem(ItemData newItemData)
    {
        currentItem = newItemData;

        if (currentItem != null)
        {
            itemIcon.sprite = currentItem.icon;
            // Rengini ve tam görünürlüğünü (Alpha 1) geri ver
            itemIcon.color = new Color(currentItem.itemColor.r, currentItem.itemColor.g, currentItem.itemColor.b, 1f);
            
            // Görsel tıklamaları tekrar algılasın
            itemIcon.raycastTarget = true; 
        }
        else
        {
            ClearCell();
        }
    }

    public void ClearCell()
    {
        currentItem = null;
        itemIcon.sprite = null;
        
        // DİKKAT: Obje asla kapatılmaz (SetActive false YAPILMAZ).
        // Sadece şeffaf (Alpha 0) yapılır ve tıklanması (raycast) engellenir.
        itemIcon.color = new Color(1f, 1f, 1f, 0f); 
        itemIcon.raycastTarget = false; 
    }

    public bool IsEmpty()
    {
        // Artık sadece veriye bakması %100 güvenli, çünkü ikonlar yollarını kaybetmeyecek.
        return currentItem == null; 
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObj = eventData.pointerDrag;

        if (droppedObj != null)
        {
            DraggableItem draggedItemScript = droppedObj.GetComponent<DraggableItem>();
            Cell sourceCell = draggedItemScript.originalParent.GetComponentInParent<Cell>();

            if (sourceCell == null || sourceCell == this) return;

            if (IsEmpty())
            {
                SetItem(sourceCell.currentItem);
                sourceCell.ClearCell();
            }
            else
            {
                if (currentItem.id == sourceCell.currentItem.id &&
                    currentItem.tier == sourceCell.currentItem.tier &&
                    currentItem.nextTierItem != null)
                {
                    SetItem(currentItem.nextTierItem);
                    sourceCell.ClearCell();
                }
                else
                {
                    ItemData tempItem = currentItem;
                    SetItem(sourceCell.currentItem);
                    sourceCell.SetItem(tempItem);
                }
            }
        }
    }
}