using UnityEngine;

[CreateAssetMenu(fileName = "TradeMechanicResearchNode", menuName = "ResearchNodes/TradeMechanicResearchNode")]
public class TradeMechanicResearchNode : ResearchNodeData
{
    public override void OnUnlock()
    {
        TradeMechanic.Activate();
    }
}
