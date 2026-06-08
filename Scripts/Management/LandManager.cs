using System;
using UnityEngine;

public class LandManager : MonoBehaviour
{
    public event Action<int, int> OnLandChanged; // (total, used)

    [SerializeField] private ResourceManager resourceManager;
    [Tooltip("Land the player starts with.")]
    [SerializeField] private int startingLand = 5;
    [Tooltip("Matter cost for the first land purchase.")]
    [SerializeField] private int landBaseCost = 500;
    [Tooltip("Additional matter cost added per subsequent purchase.")]
    [SerializeField] private int landCostIncrement = 100;

    private int _totalLand = 0;
    private int _usedLand = 0;
    private int _purchaseCount = 0;

    public int TotalLand => _totalLand;
    public int UsedLand => _usedLand;
    public int AvailableLand => _totalLand - _usedLand;

    void Start()
    {
        if (resourceManager == null) resourceManager = FindAnyObjectByType<ResourceManager>();
        _totalLand = startingLand;
        OnLandChanged?.Invoke(_totalLand, _usedLand);
    }

    public bool TryBuyLand()
    {
        int cost = GetNextLandCost();
        if (resourceManager.GetMatter() < cost) return false;
        resourceManager.RemoveMatter(cost);
        _totalLand++;
        _purchaseCount++;
        OnLandChanged?.Invoke(_totalLand, _usedLand);
        return true;
    }
    public void BuyLand() // so UI button can call 
    {
        TryBuyLand();
    }
    public bool TryUseLand(int amount)
    {
        if (_usedLand + amount > _totalLand) return false;
        _usedLand += amount;
        OnLandChanged?.Invoke(_totalLand, _usedLand);
        return true;
    }

    public void FreeLand(int amount)
    {
        _usedLand = Mathf.Max(0, _usedLand - amount);
        OnLandChanged?.Invoke(_totalLand, _usedLand);
    }

    public int GetNextLandCost()
    {
        return landBaseCost + landCostIncrement * _purchaseCount;
    }
}
