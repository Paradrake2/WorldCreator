using UnityEngine;

public class ResearchCategoryUI : MonoBehaviour
{
    [SerializeField] private ResearchCategory category;
    [SerializeField] private Transform nodeUIParent;
    [SerializeField] private GameObject nodeTierUIPrefab;
    public void Initialize(ResearchCategory category)
    {
        this.category = category;
        GenerateUI();
    }
    public void GenerateUI()
    {
        // clear existing UI
        foreach (Transform child in nodeUIParent)
        {
            Destroy(child.gameObject);
        }

        // generate new UI based on category data
        foreach (var tier in category.CategoryTiers)
        {
            GameObject tierUI = Instantiate(nodeTierUIPrefab, nodeUIParent);
            CategoryTierUI tierUIComponent = tierUI.GetComponent<CategoryTierUI>();
            tierUIComponent.Initialize(tier);
            // Initialize the tier UI with the tier data
        }
    }
}
