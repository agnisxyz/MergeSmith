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
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = Vector2.zero; 
    }

    // YENİ FONKSİYON: Eşyaya tıklandığında çalışır
    public void OnPointerClick(PointerEventData eventData)
    {
        // Eğer bu bir sürükleme işlemiyse (yanlışlıkla tıklama sayılmasın diye) iptal et
        if (eventData.dragging) return;

        // İçinde bulunduğumuz hücreyi bul
        Cell currentCell = GetComponentInParent<Cell>();
        
        if (currentCell != null && currentCell.currentItem != null)
        {
            // Eğer bu eşya bir "Generator" ise
            if (currentCell.currentItem.isGenerator)
            {
                GenerateNewItem(currentCell.currentItem);
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