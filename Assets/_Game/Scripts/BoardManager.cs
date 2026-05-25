using UnityEngine;
using System.Collections.Generic;

// YENİ: Başlangıçta tahtaya konacak eşyaları belirleyen küçük bir veri sınıfı
[System.Serializable]
public class StartingItem
{
    public Vector2Int position; // Eşyanın nerede olacağı (Örn: X:0, Y:0)
    public ItemData item;       // Hangi eşya olacağı (Örn: Mine_Cart_Gen)
}

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    [Header("Board Dimensions")]
    public int columns = 8; 
    public int rows = 8;    

    [Header("References")]
    public GameObject cellPrefab;    
    public Transform boardContainer; 

    [Header("Level Settings")]
    // YENİ: Başlangıçta tahtada kesin olarak duracak eşyaların listesi
    public StartingItem[] startingItems; 

    private Cell[,] gridMap;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        gridMap = new Cell[columns, rows];

        // 1. Önce tahtayı (boş olarak) kuruyoruz
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject newCellObj = Instantiate(cellPrefab, boardContainer);
                newCellObj.transform.localScale = Vector3.one; 
                newCellObj.name = $"Cell_{x}_{y}";

                Cell cellComponent = newCellObj.GetComponent<Cell>();
                cellComponent.gridPosition = new Vector2Int(x, y);
                cellComponent.ClearCell();

                gridMap[x, y] = cellComponent;
            }
        }
        
        // 2. Tahta kurulduktan sonra başlangıç eşyalarını (Generator vb.) yerlerine koyuyoruz
        PlaceStartingItems();
    }

    private void PlaceStartingItems()
    {
        // Döngüyle startingItems listesindeki her elemanı geziyoruz
        foreach (StartingItem sItem in startingItems)
        {
            int x = sItem.position.x;
            int y = sItem.position.y;

            // X ve Y değerleri tahtanın sınırları içindeyse eşyayı yerleştir
            if (x >= 0 && x < columns && y >= 0 && y < rows)
            {
                gridMap[x, y].SetItem(sItem.item);
            }
            else
            {
                Debug.LogWarning($"Taşma Hatası: {sItem.item.itemName} eşyasını tahta dışına ({x},{y}) koymaya çalışıyorsun!");
            }
        }
    }

    public Cell GetRandomEmptyCell()
    {
        List<Cell> emptyCells = new List<Cell>();

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (gridMap[x, y].IsEmpty())
                {
                    emptyCells.Add(gridMap[x, y]);
                }
            }
        }

        if (emptyCells.Count > 0)
        {
            int randomIndex = Random.Range(0, emptyCells.Count);
            return emptyCells[randomIndex];
        }

        return null;
    }

    // Tahtayı tarayıp, istenilen ID'ye sahip eşyadan kaç tane olduğunu sayar
    public int CountItemOnBoard(string itemID)
    {
        int count = 0;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (!gridMap[x, y].IsEmpty() && gridMap[x, y].currentItem.id == itemID)
                {
                    count++;
                }
            }
        }
        return count;
    }

    // İstenilen ID'ye sahip eşyaları tahtadan bulur ve belirtilen sayı kadar siler (Consume)
    public void RemoveItemsFromBoard(string itemID, int amountToRemove)
    {
        int removedCount = 0;
        
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                // Yeteri kadar sildiysek çık
                if (removedCount >= amountToRemove) return;

                if (!gridMap[x, y].IsEmpty() && gridMap[x, y].currentItem.id == itemID)
                {
                    gridMap[x, y].ClearCell(); // Hücreyi temizle (Eşyayı teslim ettik)
                    removedCount++;
                }
            }
        }
    }
}