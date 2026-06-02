using UnityEngine;
using TMPro;

public class EnergyUI : MonoBehaviour
{
    public TextMeshProUGUI energyText;

    private void OnEnable()
    {
        EnergyManager.OnEnergyChanged += UpdateEnergyUI;
    }

    private void OnDisable()
    {
        EnergyManager.OnEnergyChanged -= UpdateEnergyUI;
    }

    private void Start()
    {
        // Başlangıç değerini al
        if (EnergyManager.Instance != null)
        {
            UpdateEnergyUI(EnergyManager.Instance.currentEnergy, EnergyManager.Instance.maxEnergy);
        }
    }

    private void UpdateEnergyUI(int current, int max)
    {
        // Örn: "85 / 100" şeklinde ekrana yazar
        energyText.text = $"{current} / {max}"; 
    }
}