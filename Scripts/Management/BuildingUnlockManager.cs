using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUnlockManager : MonoBehaviour
{
    public event Action<BuildingDefinition> OnBuildingUnlocked;

    [SerializeField] private ProductionManager productionManager;
    [SerializeField] private ResourceManager resourceManager;
    [Tooltip("All buildings in the game. Drag every BuildingDefinition SO here.")]
    [SerializeField] private BuildingDefinition[] allBuildings;
    [Tooltip("Assign the Matter ResourceDefinition SO here to support matter-gated unlocks.")]
    [SerializeField] private ResourceDefinition matterResource;

    private readonly HashSet<BuildingDefinition> _unlocked = new();
    private readonly HashSet<BuildingDefinition> _locked = new();

    public bool IsUnlocked(BuildingDefinition building) => _unlocked.Contains(building);

    void Start()
    {
        if (productionManager == null) productionManager = FindAnyObjectByType<ProductionManager>();
        if (resourceManager == null) resourceManager = FindAnyObjectByType<ResourceManager>();

        foreach (var building in allBuildings)
        {
            if (building.IsUnlockedFromStart)
                Unlock(building);
            else
                _locked.Add(building);
        }

        productionManager.OnResourceChanged += OnResourceChanged;
        ResourceManager.OnMatterChanged += OnMatterChanged;
    }

    void OnDestroy()
    {
        productionManager.OnResourceChanged -= OnResourceChanged;
        ResourceManager.OnMatterChanged -= OnMatterChanged;
    }

    private void OnResourceChanged(ResourceDefinition resource, float amount)
    {
        CheckAllLocked();
    }

    private void OnMatterChanged(float amount)
    {
        CheckAllLocked();
    }

    private void CheckAllLocked()
    {
        var toUnlock = new List<BuildingDefinition>();
        foreach (var building in _locked)
        {
            if (AreAllConditionsMet(building))
                toUnlock.Add(building);
        }
        foreach (var building in toUnlock)
            Unlock(building);
    }

    private bool AreAllConditionsMet(BuildingDefinition building)
    {
        foreach (var condition in building.unlockConditions)
        {
            if (condition.resource == null) continue;
            float have = condition.resource == matterResource
                ? resourceManager.GetMatter()
                : productionManager.GetResource(condition.resource);
            if (have < condition.threshold) return false;
        }
        return true;
    }

    private void Unlock(BuildingDefinition building)
    {
        _locked.Remove(building);
        _unlocked.Add(building);
        OnBuildingUnlocked?.Invoke(building);
    }
}
