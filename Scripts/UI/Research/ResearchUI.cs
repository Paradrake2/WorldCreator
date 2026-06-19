using UnityEngine;

public class ResearchUI : MonoBehaviour
{
    [SerializeField] private GameObject categoryObjectPrefab;
    [SerializeField] private Transform transformToPopulate;
    public void PopulateUI(ArchCategory archCategory)
    {
        foreach (Transform child in transformToPopulate)
        {
            Destroy(child.gameObject);
        }
        foreach(ResearchCategory category in archCategory.ResearchCategories)
        {
            if (category.IsUnlocked)
            {
                GameObject categoryObj = Instantiate(categoryObjectPrefab, transformToPopulate);
                ResearchCategoryUI categoryUI = categoryObj.GetComponent<ResearchCategoryUI>();
                categoryUI.Initialize(category);
            }
            
        }
    }
}

