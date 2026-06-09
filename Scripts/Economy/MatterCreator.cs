using System;
using UnityEngine;

public class MatterCreator : MonoBehaviour
{
    public event Action<float> OnMatterCreatedChanged;
    public event Action<int> OnCreatorsChanged;

    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private float matterAmount = 1f;
    [SerializeField] private int numberOfCreators = 1;
    [SerializeField] private float creationInterval = 5f;
    [SerializeField] private int baseCost = 10;
    [SerializeField] private float costMultiplier1 = 1.25f;
    [SerializeField] private float costMultiplier2 = 1.5f;

    private int generationId = -1;

    void Start()
    {
        if (resourceManager == null) resourceManager = FindAnyObjectByType<ResourceManager>();

        if (MatterGenerationManager.Instance != null)
            generationId = MatterGenerationManager.Instance.Register(matterAmount, numberOfCreators, creationInterval);

        OnMatterCreatedChanged?.Invoke(matterAmount);
        OnCreatorsChanged?.Invoke(numberOfCreators);
    }

    private void PushToManager()
    {
        if (MatterGenerationManager.Instance != null && generationId >= 0)
            MatterGenerationManager.Instance.UpdateEntry(generationId, matterAmount, numberOfCreators, creationInterval);
    }

    public void IncreaseMatterAmount(float amount)
    {
        int cost = GetCostForNextMatterAmount();
        if (resourceManager.GetMatter() < cost) return;
        matterAmount += amount;
        resourceManager.RemoveMatter(cost);
        PushToManager();
        OnMatterCreatedChanged?.Invoke(matterAmount);
    }

    public void IncreaseNumberOfCreators(int amount)
    {
        int cost = GetCostForNextCreator();
        if (resourceManager.GetMatter() < cost) return;
        numberOfCreators += amount;
        resourceManager.RemoveMatter(cost);
        PushToManager();
        OnCreatorsChanged?.Invoke(numberOfCreators);
    }

    public void ChangeCreationInterval(float newInterval)
    {
        creationInterval = newInterval;
        PushToManager();
    }

    public float GetCreationInterval() => creationInterval;
    public float GetMatterAmount() => matterAmount;
    public int GetNumberOfCreators() => numberOfCreators;
    public float GetMatterPerInterval() => matterAmount * numberOfCreators;

    public int CalculateCostForNextCreator(int baseCost, float costMultiplier)
    {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, numberOfCreators));
    }
    public int GetCostForNextCreator()
    {
        return CalculateCostForNextCreator(baseCost, costMultiplier2);
    }
    public int CalculateCostForNextMatterAmount(int baseCost, float costMultiplier)
    {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, (int)matterAmount));
    }
    public int GetCostForNextMatterAmount()
    {
        return CalculateCostForNextMatterAmount(baseCost, costMultiplier1);
    }
}

