using System;
using UnityEngine;

public class MatterCreator : MonoBehaviour
{
    public event Action<float> OnMatterCreatedChanged;
    public event Action<int> OnCreatorsChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private float matterAmount = 1f;
    [SerializeField] private int numberOfCreators = 1;
    [SerializeField] private float creationInterval = 5f; // default is 5 seconds
    [SerializeField] private int baseCost = 10;
    [SerializeField] private float costMultiplier1 = 1.25f;
    [SerializeField] private float costMultiplier2 = 1.5f;
    private float timer = 0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= creationInterval)
        {
            float matterCreated = matterAmount * numberOfCreators;
            resourceManager.AddMatter(matterCreated);
            timer = 0f;
        }
    }
    public void IncreaseMatterAmount(float amount)
    {
        // of has enough matter, increase matter amount
        int cost = GetCostForNextMatterAmount();
        if (resourceManager.GetMatter() < cost) return;
        matterAmount += amount;
        resourceManager.RemoveMatter(cost);
        OnMatterCreatedChanged?.Invoke(matterAmount);
    }
    public void IncreaseNumberOfCreators(int amount)
    {
        // if has enough matter, buy creator
        int cost = GetCostForNextCreator();
        if (resourceManager.GetMatter() < cost) return;
        numberOfCreators += amount;
        resourceManager.RemoveMatter(cost);
        OnCreatorsChanged?.Invoke(numberOfCreators);
    }
    public void ChangeCreationInterval(float newInterval)
    {
        creationInterval = newInterval;
    }
    public float GetCreationInterval()
    {
        return creationInterval;
    }
    public float GetMatterAmount()
    {
        return matterAmount;
    }
    public int GetNumberOfCreators()
    {
        return numberOfCreators;
    }
    public float GetMatterPerInterval()
    {
        return matterAmount * numberOfCreators;
    }
    void Start()
    {
        if (resourceManager == null) resourceManager = FindAnyObjectByType<ResourceManager>();
        OnMatterCreatedChanged?.Invoke(matterAmount);
        OnCreatorsChanged?.Invoke(numberOfCreators);
    }
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
