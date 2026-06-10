using UnityEngine;
using UnityEngine.EventSystems;

public class BuildButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private BuildingDefinition buildingDefinition;
    [SerializeField] private BuildingManager buildingManager;
    public int amountToBuild = 1;
    [SerializeField] private bool isMax = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize(BuildingDefinition definition, BuildingManager manager)
    {
        buildingDefinition = definition;
        buildingManager = manager;
    }

    // Update is called once per frame
    private float MaxCanBuild()
    {
        float maxByMatter = buildingManager.ResourceManager.GetMatter() / buildingDefinition.matterCost;
        int maxByLand = buildingManager.LandManager.AvailableLand / buildingDefinition.landCost;
        float maxByResources = float.MaxValue;

        if (buildingDefinition.buildCosts != null)
        {
            foreach (var cost in buildingDefinition.buildCosts)
            {
                if (cost.resource == null) continue;
                float maxForThisResource = buildingManager.ProductionManager.GetResource(cost.resource) / cost.amount;
                if (maxForThisResource < maxByResources)
                    maxByResources = maxForThisResource;
            }
        }

        return Mathf.Min(maxByMatter, maxByLand, maxByResources);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isMax)
            amountToBuild = Mathf.FloorToInt(MaxCanBuild());
        buildingManager.costPanel.UpdateCostText(buildingDefinition, amountToBuild);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buildingManager.costPanel.UpdateCostText(null);
    }
}
