using System;
using UnityEngine;

public class LandManager : MonoBehaviour
{
    public event Action<int, int> OnLandChanged; // (total, used)

    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private int landPerPurchase = 10;
    [SerializeField] private int landBaseCost = 50;
    [SerializeField] private float landCostMultiplier = 1.5f;

    private int _totalLand = 0;
    private int _usedLand = 0;
    private int _purchaseCount = 0;

    public int TotalLand => _totalLand;
    public int UsedLand => _usedLand;
    public int AvailableLand => _totalLand - _usedLand;

    void Start()
    {
        if (resourceManager == null) resourceManager = FindAnyObjectByType<ResourceManager>();
        OnLandChanged?.Invoke(_totalLand, _usedLand);
    }

    public bool TryBuyLand()
    {
        int cost = GetNextLandCost();
        if (resourceManager.GetMatter() < cost) return false;
        resourceManager.RemoveMatter(cost);
        _totalLand += landPerPurchase;
        _purchaseCount++;
        OnLandChanged?.Invoke(_totalLand, _usedLand);
        return true;
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
        return Mathf.RoundToInt(landBaseCost * Mathf.Pow(landCostMultiplier, _purchaseCount));
    }
}
