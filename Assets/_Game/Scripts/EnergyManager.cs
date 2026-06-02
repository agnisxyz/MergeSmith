using UnityEngine;
using System; // Action için

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager Instance;
    
    // UI'ı tetikleyecek olay (Mevcut Enerji ve Maksimum Enerji yollayacağız)
    public static event Action<int, int> OnEnergyChanged; 

    [Header("Energy Settings")]
    public int maxEnergy = 100;
    public int currentEnergy;
    
    [Tooltip("Saniye cinsinden 1 enerjinin dolma süresi")]
    public float timeToRegenOneEnergy = 120f; // 2 dakikada 1 dolsun
    private float regenTimer;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Başlangıçta enerjiyi full yapalım (Test için)
        currentEnergy = maxEnergy;
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }

    private void Update()
    {
        // Enerji full değilse zamanlayıcıyı çalıştır
        if (currentEnergy < maxEnergy)
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= timeToRegenOneEnergy)
            {
                AddEnergy(1);
                regenTimer = 0f;
            }
        }
    }

    // Enerji harcamak istediğimizde bu fonksiyonu çağıracağız (True/False döner)
    public bool TryConsumeEnergy(int amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
            return true; // Enerji harcandı, işleme izin ver
        }
        
        Debug.Log("Yeterli Enerji Yok!");
        return false; // Enerji yetmedi, işlemi iptal et
    }

    public void AddEnergy(int amount)
    {
        currentEnergy += amount;
        if (currentEnergy > maxEnergy) currentEnergy = maxEnergy; // Sınırı aşmasın
        
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }
}