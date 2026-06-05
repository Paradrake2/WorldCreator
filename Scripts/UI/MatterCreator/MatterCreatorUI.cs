using UnityEngine;
using TMPro;
public class MatterCreatorUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI matterAmountText;
    [SerializeField] private TextMeshProUGUI creatorsAmountText;
    [SerializeField] private MatterCreator matterCreator;
    
    void OnEnable()
    {
        if (matterCreator == null) return;
        matterCreator.OnMatterCreatedChanged += UpdateMatterAmount;
        matterCreator.OnCreatorsChanged += UpdateCreatorsAmount;
    }
    void OnDisable()
    {
        if (matterCreator == null) return;
        matterCreator.OnMatterCreatedChanged -= UpdateMatterAmount;
        matterCreator.OnCreatorsChanged -= UpdateCreatorsAmount;
    }
    private void UpdateMatterAmount(float amount)
    {
        matterAmountText.text = "Matter per Interval: " + amount.ToString("F2") + " (Cost: " + matterCreator.GetCostForNextMatterAmount() + " Matter)";
    }
    private void UpdateCreatorsAmount(int amount)
    {
        creatorsAmountText.text = "Number of Creators: " + amount.ToString() + " (Cost: " + matterCreator.GetCostForNextCreator() + " Matter)";
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateMatterAmount(1);
        UpdateCreatorsAmount(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
