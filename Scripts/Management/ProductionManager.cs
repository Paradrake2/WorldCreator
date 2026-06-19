using System;
using System.Collections.Generic;
using UnityEngine;

public class ProductionManager : MonoBehaviour
{
    public event Action<ResourceDefinition, float> OnResourceChanged;
    public event Action<float> OnProductivityChanged;
    public event Action<bool> OnProductionHaltedChanged;

    [SerializeField] private BuildingManager buildingManager;
    [SerializeField] private float tickInterval = 5f;

    [Header("People")]
    [Tooltip("The ResourceDefinition SO that represents people.")]
    [SerializeField] private ResourceDefinition peopleResource;
    [Tooltip("How many people are lost per tick for each essential resource at zero.")]
    [SerializeField] private float populationLossPerEssentialDeficit = 1f;

    [Header("Per-Person Consumption")]
    [Tooltip("Food consumed per person per tick.")]
    [SerializeField] private ResourceDefinition foodResource;
    [SerializeField] private float foodPerPersonPerTick = 0.1f;
    [Tooltip("Water consumed per person per tick.")]
    [SerializeField] private ResourceDefinition waterResource;
    [SerializeField] private float waterPerPersonPerTick = 0.1f;

    [Header("Productivity")]
    [Tooltip("How much productivity shifts per tick when a Productivity-type resource is in surplus or deficit.")]
    [SerializeField] private float productivityShiftPerTick = 0.05f;
    private const float ProductivityMin = 0.5f;
    private const float ProductivityMax = 1.5f;
    private float _productivity = 1f;
    private bool _productivityEnabled = false;
    private bool _productionHalted = false;
    private bool _consumptionEnabled = false;

    [Header("Resources")]
    [Tooltip("Every ResourceDefinition SO in the game. Used to initialize starting amounts.")]
    [SerializeField] private ResourceDefinition[] allResources;

#if UNITY_EDITOR
    [ContextMenu("Find All Resources")]
    private void FindAllResources()
    {
        var guids = UnityEditor.AssetDatabase.FindAssets("t:ResourceDefinition");
        var results = new ResourceDefinition[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
            results[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<ResourceDefinition>(path);
        }
        allResources = results;
        UnityEditor.EditorUtility.SetDirty(this);
        Debug.Log($"[ProductionManager] Found {results.Length} ResourceDefinition assets.");
    }
#endif

    private readonly Dictionary<ResourceDefinition, float> _resources = new();
    private float _timer = 0f;
    private int _populationCap = 0;

    public float Productivity => _productivity;
    public int PopulationCap => _populationCap;
    public bool ProductivityEnabled => _productivityEnabled;
    public bool IsProductionHalted => _productionHalted;

    public void EnableProductivity()
    {
        _productivityEnabled = true;
    }

    public void EnableConsumption()
    {
        _consumptionEnabled = true;
    }

    void Start()
    {
        if (buildingManager == null) buildingManager = FindAnyObjectByType<BuildingManager>();
        buildingManager.OnBuildingsChanged += RecalculatePopulationCap;

        // Seed resources from their startingAmount values.
        if (allResources != null)
        {
            foreach (var res in allResources)
            {
                if (res != null && res.startingAmount > 0f)
                {
                    _resources[res] = res.startingAmount;
                    OnResourceChanged?.Invoke(res, res.startingAmount);
                }
            }
        }

        RecalculatePopulationCap();
    }

    void OnDestroy()
    {
        if (buildingManager != null)
            buildingManager.OnBuildingsChanged -= RecalculatePopulationCap;
    }
    public Dictionary<ResourceDefinition, float> GetAllResources()
    {
        return new Dictionary<ResourceDefinition, float>(_resources);
    }
    public int GetResourceCount()
    {
        return _resources.Count;
    }
    public int GetSpecificResourceCount(ResourceDefinition resource)
    {
        _resources.TryGetValue(resource, out float value);
        return Mathf.RoundToInt(value);
    }
    private void RecalculatePopulationCap()
    {
        int newCap = 0;
        foreach (var (building, count) in buildingManager.Buildings)
            newCap += building.housingCapacity * count;
        _populationCap = newCap;

        // Only clamp population down if there is at least some housing built.
        // A cap of 0 means no housing exists yet, not that nobody is allowed to live.
        if (peopleResource != null && _populationCap > 0)
        {
            float currentPeople = GetResource(peopleResource);
            if (currentPeople > _populationCap)
                ModifyResource(peopleResource, _populationCap - currentPeople);
        }
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= tickInterval)
        {
            Tick();
            _timer = 0f;
        }
    }

    public float GetResource(ResourceDefinition resource)
    {
        _resources.TryGetValue(resource, out float value);
        return value;
    }

    public void ModifyResource(ResourceDefinition resource, float delta)
    {
        if (resource == null) return;
        _resources.TryGetValue(resource, out float current);
        float newValue = Mathf.Max(0f, current + delta);
        if (resource.HasCap) newValue = Mathf.Min(newValue, resource.cap);
        // People can never exceed the housing cap.
        if (resource == peopleResource && _populationCap > 0)
            newValue = Mathf.Min(newValue, _populationCap);
        _resources[resource] = Mathf.Round(newValue * 100f) / 100f;
        OnResourceChanged?.Invoke(resource, _resources[resource]);
    }

    private void Tick()
    {
        // Staffing ratio: if not enough people to staff all buildings, production scales down.
        float totalPeopleRequired = 0f;
        foreach (var (building, count) in buildingManager.Buildings)
            totalPeopleRequired += building.peopleCost * count;

        float currentPeople = peopleResource != null ? GetResource(peopleResource) : float.MaxValue;
        float staffingRatio = totalPeopleRequired > 0f
            ? Mathf.Clamp01(currentPeople / totalPeopleRequired)
            : 1f;

        // If staffing drops to 50%, halt all building production until fully staffed.
        bool nowHalted = staffingRatio <= ProductivityMin;
        if (nowHalted != _productionHalted)
        {
            _productionHalted = nowHalted;
            OnProductionHaltedChanged?.Invoke(_productionHalted);
        }

        // Run each building only when production is not halted.
        if (!_productionHalted)
        {
            foreach (var (building, count) in buildingManager.Buildings)
            {
                if (count <= 0) continue;

                // Check consumption can be met (consumption is not affected by productivity or staffing).
                bool canRun = true;
                foreach (var rate in building.productionRates)
                {
                    if (rate.amountPerTick >= 0f) continue;
                    _resources.TryGetValue(rate.resource, out float have);
                    if (have < -rate.amountPerTick * count)
                    {
                        canRun = false;
                        break;
                    }
                }
                if (!canRun) continue;

                foreach (var rate in building.productionRates)
                {
                    if (rate.resource == null) continue;
                    _resources.TryGetValue(rate.resource, out float current);

                    float delta;
                    if (rate.amountPerTick > 0f)
                        // Production is scaled by both staffing and productivity.
                        delta = rate.amountPerTick * count * staffingRatio * _productivity;
                    else
                        // Consumption is always full — buildings still consume even when understaffed.
                        delta = rate.amountPerTick * count;

                    float newValue = current + delta;
                    if (rate.resource.HasCap) newValue = Mathf.Min(newValue, rate.resource.cap);
                    newValue = Mathf.Max(0f, newValue);

                    _resources[rate.resource] = Mathf.Round(newValue * 100f) / 100f;
                    OnResourceChanged?.Invoke(rate.resource, _resources[rate.resource]);
                }
            }
        }

        // Per-person consumption and essential deficit handling.
        if (_consumptionEnabled && peopleResource != null)
        {
            float population = GetResource(peopleResource);

            if (foodResource != null && population > 0f)
                ModifyResource(foodResource, -foodPerPersonPerTick * population);
            if (waterResource != null && population > 0f)
                ModifyResource(waterResource, -waterPerPersonPerTick * population);

            int essentialDeficits = 0;
            foreach (var (resource, amount) in _resources)
            {
                if (resource.resourceType == ResourceType.Essential && amount <= 0f)
                    essentialDeficits++;
            }
            if (essentialDeficits > 0)
                ModifyResource(peopleResource, -populationLossPerEssentialDeficit * essentialDeficits);
        }

        // Productivity-type resource surplus/deficit → shift productivity.
        if (_productivityEnabled)
        {
            float productivityDelta = 0f;
            foreach (var (resource, amount) in _resources)
            {
                if (resource.resourceType != ResourceType.Productivity) continue;
                productivityDelta += amount > 0f ? productivityShiftPerTick : -productivityShiftPerTick;
            }

            if (productivityDelta != 0f)
            {
                _productivity = Mathf.Clamp(_productivity + productivityDelta, ProductivityMin, ProductivityMax);
                _productivity = Mathf.Round(_productivity * 100f) / 100f;
                OnProductivityChanged?.Invoke(_productivity);
            }
        }
    }
}
