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
    [SerializeField] private NodeCost cost;
    public string NodeName => nodeName;
    public bool IsUnlocked => isUnlocked;
    public ResearchNodeData[] Prerequisites => prerequisites;
    public NodeCost Cost => cost;

    public abstract void OnUnlock();

    public void Unlock()
    {
        isUnlocked = true;
    }

}
