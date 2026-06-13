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
    public Color textColor = Color.white;
    public int tier = 0; // For UI sorting. Sorting within tiers is alphabetical
    [Tooltip("If true, this resource's row is visible in the UI from the start of the game.")]
    public bool showFromStart = false;
    public float tradeInputValue = 1f; // how much 1 unit of this resource is worth when used to convert to another resource
    public float tradeOutputValue = 5f; // how many units of trade value are required to make one unit of this resource
}
