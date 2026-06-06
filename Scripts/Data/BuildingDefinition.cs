using System;
using UnityEngine;

[Serializable]
public struct ResourceRate
{
    public ResourceDefinition resource;
    [Tooltip("Amount per tick per building. Positive = produced, negative = consumed.")]
    public float amountPerTick;
}

[Serializable]
public struct UnlockCondition
{
    public ResourceDefinition resource;
    [Tooltip("Amount of this resource required to unlock the building.")]
    public float threshold;
}

[Serializable]
public struct ResourceCost
{
    public ResourceDefinition resource;
    public float amount;
    [Tooltip("If true, this amount is refunded when the building is demolished.")]
    public bool refundOnDemolish;
}

[CreateAssetMenu(fileName = "NewBuilding", menuName = "World Creator/Building Definition")]
public class BuildingDefinition : ScriptableObject
{
    public string buildingName;
    [Tooltip("How many land slots this building occupies.")]
    public int landCost = 1;
    [Tooltip("Matter cost to construct one of this building.")]
    public int matterCost = 100;
    [Tooltip("Additional resource costs paid once on construction.")]
    public ResourceCost[] buildCosts;
    [Tooltip("Number of people required to staff one of this building. Building will not run without enough people.")]
    public int peopleCost = 0;
    [Tooltip("How many people can live in one of this building. 0 for non-housing buildings.")]
    public int housingCapacity = 0;
    public ResourceRate[] productionRates;

    [Header("Unlock Conditions")]
    [Tooltip("All conditions must be met to unlock this building. Leave empty to unlock from the start.")]
    public UnlockCondition[] unlockConditions;

    public bool IsUnlockedFromStart => unlockConditions == null || unlockConditions.Length == 0;
    public Color textColor = Color.white;
    public int sortOrder = 0; // For UI sorting. Sorting within orders is alphabetical
}
