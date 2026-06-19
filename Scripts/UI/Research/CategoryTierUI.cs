using UnityEngine;

public class CategoryTierUI : MonoBehaviour
{
    [SerializeField] private ResearchNodeData[] NodesInTier;

    [SerializeField] private bool allNodesUnlocked;
    [SerializeField] private GameObject nodeUIPrefab;
    [SerializeField] private Transform nodesParent;
    public void Initialize(CategoryTier tier)
    {
        NodesInTier = tier.nodesInTier;
        allNodesUnlocked = tier.allNodesUnlocked;
        PopulateUI();
    }
    public void PopulateUI()
    {
        foreach (Transform child in nodesParent)
        {
            Destroy(child.gameObject);
        }
        foreach (var node in NodesInTier)
        {
            GameObject nodeUI = Instantiate(nodeUIPrefab, nodesParent);
            ResearchNode researchNode = nodeUI.GetComponent<ResearchNode>();
            researchNode.Initialize(node, FindAnyObjectByType<ResearchManager>());
        }
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
