using UnityEngine;
using UnityEngine.EventSystems; // Sürükle-bırak arayüzleri için bu kütüphane şart

// IBeginDragHandler, IDragHandler, IEndDragHandler arayüzlerini (interface) ekliyoruz.
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Transform originalParent; // Sürüklemeden önceki asıl hücremiz
    
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
        // 1. Sürükleme başladığında eski evimizi (hücreyi) hafızaya al
        originalParent = transform.parent;
        
        // 2. Eşyayı UI'da en üste (Canvas'a) taşı ki diğer hücrelerin altında kalıp gizlenmesin
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling(); // Ekranda her şeyin üstünde çizilmesini sağlar

        // 3. ÇOK ÖNEMLİ: Sürüklerken eşyanın kendisi tıklamaları engellemesin ki, 
        // altındaki hücreyi (bırakacağımız yeri) algılayabilelim.
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Fare/Parmağı takip et
        // (Ekran çözünürlüğüne göre hızı dengelemek için canvas.scaleFactor'e bölüyoruz)
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Sürükleme bittiğinde eşyanın tekrar tıklanabilir olmasını aç
        canvasGroup.blocksRaycasts = true;

        // Şimdilik test için eşyayı eski yerine (originalParent) geri gönderiyoruz.
        // Bir sonraki adımda burayı "eğer başka bir hücreye bırakıldıysa Swap/Merge yap" diye değiştireceğiz.
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = Vector2.zero; // Hücrenin tam ortasına oturt
    }
}