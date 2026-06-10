using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public event Action OnBuildingsChanged;

    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private LandManager landManager;
    [SerializeField] private ProductionManager productionManager;

    private readonly Dictionary<BuildingDefinition, int> _buildings = new();

    public IReadOnlyDictionary<BuildingDefinition, int> Buildings => _buildings;
    public CostPanel costPanel;
    public ProductionManager ProductionManager => productionManager;
    public ResourceManager ResourceManager => resourceManager;
    public LandManager LandManager => landManager;

    void Start()
    {
        if (resourceManager == null) resourceManager = FindAnyObjectByType<ResourceManager>();
        if (landManager == null) landManager = FindAnyObjectByType<LandManager>();
        if (productionManager == null) productionManager = FindAnyObjectByType<ProductionManager>();
    }
    public bool TryBuild(BuildingDefinition building)
    {
        if (resourceManager.GetMatter() < building.matterCost) return false;
        if (!landManager.TryUseLand(building.landCost)) return false;

        // Check all additional resource costs.
        if (building.buildCosts != null)
        {
            foreach (var cost in building.buildCosts)
            {
                if (cost.resource == null) continue;
                if (productionManager.GetResource(cost.resource) < cost.amount) return false;
            }
        }

        resourceManager.RemoveMatter(building.matterCost);

        if (building.buildCosts != null)
            foreach (var cost in building.buildCosts)
                if (cost.resource != null)
                    productionManager.ModifyResource(cost.resource, -cost.amount);

        _buildings.TryGetValue(building, out int current);
        _buildings[building] = current + 1;
        OnBuildingsChanged?.Invoke();
        return true;
    }

    public bool TryDemolish(BuildingDefinition building)
    {
        if (!_buildings.TryGetValue(building, out int current) || current <= 0) return false;
        _buildings[building] = current - 1;
        landManager.FreeLand(building.landCost);

        if (building.buildCosts != null)
            foreach (var cost in building.buildCosts)
                if (cost.resource != null && cost.refundOnDemolish)
                    productionManager.ModifyResource(cost.resource, cost.amount);

        OnBuildingsChanged?.Invoke();
        return true;
    }

    public int GetCount(BuildingDefinition building)
    {
        _buildings.TryGetValue(building, out int count);
        return count;
    }
}
