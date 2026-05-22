using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Board Dimensions")]
    public int columns = 8; // X ekseni (Sütunlar)
    public int rows = 8;    // Y ekseni (Satırlar)

    [Header("References")]
    public GameObject cellPrefab;    // Çoğaltacağımız tekil hücrenin (Cell) şablonu
    public Transform boardContainer; // Hücrelerin içine dizileceği ana UI objesi

    public ItemData testItem;

    // Tüm hücreleri hafızada tutacağımız 2 boyutlu array
    private Cell[,] gridMap;

    private void Start()
    {
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        // Hafızada 8x8'lik bir alan açıyoruz
        gridMap = new Cell[columns, rows];

        // İç içe for döngüsü ile (X,Y) koordinatlarında geziyoruz
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                // Prefab'dan bir kopya (clone) oluştur ve boardContainer'ın içine koy
                GameObject newCellObj = Instantiate(cellPrefab, boardContainer);

                // YENİ EKLENEN SATIR: UI objelerinin üretilirken boyutunun bozulmasını kesin olarak engeller
                newCellObj.transform.localScale = Vector3.one;
                
                // Oyun motorunda düzenli görünmesi için adını değiştir (Örn: Cell_3_4)
                newCellObj.name = $"Cell_{x}_{y}";

                // Objenin üzerindeki Cell scriptini al
                Cell cellComponent = newCellObj.GetComponent<Cell>();
                
                // Hücreye kendi koordinatını öğret ve içini boşalt
                cellComponent.gridPosition = new Vector2Int(x, y);
                cellComponent.ClearCell();

                // Son olarak bu hücreyi komutanın (BoardManager) hafızasına kaydet
                gridMap[x, y] = cellComponent;
            }
        }

        // (Bu satırı test için ekliyoruz, BoardManager'ın en üstüne "public ItemData testItem;" tanımlamayı unutma)
gridMap[0, 0].SetItem(testItem);
        
        Debug.Log("8x8 Board basariyla olusturuldu!");
    }
}