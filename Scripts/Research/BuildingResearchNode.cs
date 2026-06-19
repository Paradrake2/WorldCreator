using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "BuildingResearchNode", menuName = "ResearchNodes/BuildingResearchNode")]
public class BuildingResearchNode : ResearchNodeData
{
    [SerializeField] private BuildingDefinition buildingToUnlock;
    public override void OnUnlock()
    {
        BuildingUnlockManager bm = FindAnyObjectByType<BuildingUnlockManager>();
        bm.Unlock(buildingToUnlock);
    }

}
