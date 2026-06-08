using UnityEngine;
using TMPro;
public class LandDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI landAmountText;
    [SerializeField] private ProductionManager productionManager;
    [SerializeField] private LandManager landManager;

    public void OnEnable()
    {
        if (landManager != null)
            landManager.OnLandChanged += HandleLandChanged;
    }
    public void OnDisable()
    {
        if (landManager != null)
            landManager.OnLandChanged -= HandleLandChanged;
    }
    private void HandleLandChanged(int total, int used)
    {
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        if (landAmountText != null && landManager != null)
            landAmountText.text = $"{landManager.UsedLand}/{landManager.TotalLand}";
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
