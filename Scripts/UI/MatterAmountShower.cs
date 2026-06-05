using UnityEngine;
using TMPro;

public class MatterAmountShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI matterAmountText;

    void OnEnable()
    {
        ResourceManager.OnMatterChanged += UpdateMatterAmount;
    }
    void OnDisable()
    {
        ResourceManager.OnMatterChanged -= UpdateMatterAmount;
    }

    private void UpdateMatterAmount(float amount)
    {
        matterAmountText.text = "Matter: " + amount.ToString("F2");
    }
}
