using System;
using System.Collections.Generic;
using UnityEngine;

public class ProductionManager : MonoBehaviour
{
    public event Action<ResourceDefinition, float> OnResourceChanged;
    public event Action<float> OnProductivityChanged;

    [SerializeField] private BuildingManager buildingManager;
    [SerializeField] private float tickInterval = 5f;

    [Header("People")]
    [Tooltip("The ResourceDefinition SO that represents people.")]
    [SerializeField] private ResourceDefinition peopleResource;
    [Tooltip("How many people are lost per tick for each essential resource at zero.")]
    [SerializeField] private float populationLossPerEssentialDeficit = 1f;

    [Header("Productivity")]
    [Tooltip("How much productivity shifts per tick when a Productivity-type resource is in surplus or deficit.")]
    [SerializeField] private float productivityShiftPerTick = 0.05f;
    private const float ProductivityMin = 0.5f;
    private const float ProductivityMax = 1.5f;
    private float _productivity = 1f;

    private readonly Dictionary<ResourceDefinition, float> _resources = new();
    private float _timer = 0f;
    private int _populationCap = 0;

    public float Productivity => _productivity;
    public int PopulationCap => _populationCap;

    void Start()
    {
        if (buildingManager == null) buildingManager = FindAnyObjectByType<BuildingManager>();
        buildingManager.OnBuildingsChanged += RecalculatePopulationCap;
        RecalculatePopulationCap();
    }

    void OnDestroy()
    {
        if (buildingManager != null)
            buildingManager.OnBuildingsChanged -= RecalculatePopulationCap;
    }

    private void RecalculatePopulationCap()
    {
        int newCap = 0;
        foreach (var (building, count) in buildingManager.Buildings)
            newCap += building.housingCapacity * count;
        _populationCap = newCap;

        // If population exceeds the new cap, clamp it down immediately.
        if (peopleResource != null)
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

        // Run each building.
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

        // Essential resource deficit → population loss.
        if (peopleResource != null)
        {
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
