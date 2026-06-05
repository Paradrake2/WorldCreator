using UnityEngine;

public enum ResourceType
{
    Standard,
    /// <summary>Shortage (amount == 0) causes population to decrease each tick.</summary>
    Essential,
    /// <summary>Surplus (amount > 0) raises productivity; deficit (amount == 0) lowers it.</summary>
    Productivity
}

[CreateAssetMenu(fileName = "NewResource", menuName = "World Creator/Resource Definition")]
public class ResourceDefinition : ScriptableObject
{
    public string resourceName;
    public ResourceType resourceType = ResourceType.Standard;
    public float startingAmount = 0f;
    [Tooltip("Maximum amount that can be stored. Set to -1 for no cap.")]
    public float cap = -1f;

    public bool HasCap => cap >= 0f;
}
