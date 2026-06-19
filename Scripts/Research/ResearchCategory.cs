using UnityEngine;

[System.Serializable]
public class CategoryTier
{
    public ResearchNodeData[] nodesInTier;
    public bool allNodesUnlocked => CheckAllNodesUnlocked();

    private bool CheckAllNodesUnlocked()
    {
        foreach (var node in nodesInTier)
        {
            if (!node.IsUnlocked)
                return false;
        }
        return true;
    }
}


[CreateAssetMenu(fileName = "ResearchCategory", menuName = "Research/ResearchCategory")]
public class ResearchCategory : ScriptableObject
{
    [SerializeField] private string categoryName;
    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private CategoryTier[] categoryTiers;
    public string CategoryName => categoryName;
    public bool IsUnlocked => isUnlocked;
    public CategoryTier[] CategoryTiers => categoryTiers;
    public void Unlock() => isUnlocked = true;
}
