using UnityEngine;

[System.Serializable]
public class NodeResourceCost
{
    public ResourceDefinition resource;
    public float amount;
}
[System.Serializable]
public class NodeCost
{
    public int matterCost;
    public NodeResourceCost[] resourceCosts;
}
public interface IResearchNodeUnlockable
{
    public void OnUnlock();
}
// this is the base node data class. It can be extended for specific types of research nodes
//[CreateAssetMenu(fileName = "ResearchNodeData", menuName = "Research/ResearchNodeData")]
public abstract class ResearchNodeData : ScriptableObject, IResearchNodeUnlockable
{
    [Tooltip("Name of the research node.")]
    [SerializeField] private string nodeName;
    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private ResearchNodeData[] prerequisites;
    [Tooltip("Buildings that must be unlocked before this node can be researched.")]
    [SerializeField] private BuildingDefinition[] buildingPrerequisites;
    [SerializeField] private NodeCost cost;
    [SerializeField] private bool isBaseNode = false; // when algorithm is generating category, this will be the starting node
    public string NodeName => nodeName;
    public bool IsUnlocked => isUnlocked;
    public ResearchNodeData[] Prerequisites => prerequisites;
    public BuildingDefinition[] BuildingPrerequisites => buildingPrerequisites;
    public NodeCost Cost => cost;
    public bool IsBaseNode => isBaseNode;
    public abstract void OnUnlock();

    public void Unlock()
    {
        isUnlocked = true;
    }

}
