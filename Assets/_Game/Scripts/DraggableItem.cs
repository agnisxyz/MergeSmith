using UnityEngine;
using UnityEngine.EventSystems; 

// YENİ: IPointerClickHandler eklendi
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [HideInInspector] public Transform originalParent; 
    
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ÇİFTE GÜVENLİK: Eğer objenin parent'ı gerçekten bir hücreyse (Cell) hafızaya al.
        // Yanlışlıkla Canvas'tayken tekrar tetiklenirse eski evimizi unutmamızı engeller.
        Cell parentCell = GetComponentInParent<Cell>();
        if (parentCell != null)
        {
            originalParent = transform.parent;
        }
        
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling(); 
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // EĞER obje çöp kutusu tarafından yok edildiyse, bu kodun kalanını çalıştırma
        if (this == null || gameObject == null) return;

        canvasGroup.blocksRaycasts = true;
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = Vector2.zero; 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        Cell currentCell = GetComponentInParent<Cell>();
        
        if (currentCell != null && currentCell.currentItem != null)
        {
            if (currentCell.currentItem.isGenerator)
            {
                // YENİ: Jeneratör eşya üretmeden önce EnerjiManager'a soruyor: 1 Enerji harcayabilir miyim?
                if (EnergyManager.Instance.TryConsumeEnergy(1))
                {
                    GenerateNewItem(currentCell.currentItem);
                }
                else
                {
                    // İleride buraya "Enerji Satın Al" paneli açma kodu ekleyebiliriz
                    Debug.Log("Enerji bittiği için üretim yapılamıyor.");
                }
            }
        }
    }
private void GenerateNewItem(ItemData generatorData)
    {
        Cell emptyCell = BoardManager.Instance.GetRandomEmptyCell();

        // Artık sadece null kontrolü yeterli, çünkü hücrelerimiz %100 doğru veriye sahip
        if (emptyCell != null && generatorData.possibleDrops.Length > 0)
        {
            int randomIndex = Random.Range(0, generatorData.possibleDrops.Length);
            ItemData itemToDrop = generatorData.possibleDrops[randomIndex];

            emptyCell.SetItem(itemToDrop);
            
            Debug.Log($"{generatorData.itemName} generated a {itemToDrop.itemName} at {emptyCell.gridPosition}!");
        }
        else
        {
            Debug.Log("Board is full! Cannot generate.");
        }
    }
}