using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static event Action<float> OnMatterChanged;

    private readonly Dictionary<int, Action> _thresholdCallbacks = new();
    private readonly HashSet<int> _firedThresholds = new();

    [SerializeField] private float matter = 0f;

    /// <summary>
    /// Registers a callback to fire exactly once when matter reaches or exceeds <paramref name="threshold"/>.
    /// </summary>
    public void RegisterMatterThreshold(int threshold, Action callback)
    {
        if (_firedThresholds.Contains(threshold))
        {
            // Already passed — fire immediately if current matter qualifies
            if (matter >= threshold)
            {
                callback?.Invoke();
                return;
            }
        }

        if (_thresholdCallbacks.ContainsKey(threshold))
            _thresholdCallbacks[threshold] += callback;
        else
            _thresholdCallbacks[threshold] = callback;
    }

    public void AddMatter(float amount)
    {
        matter = Mathf.Round((matter + amount) * 100f) / 100f;
        CheckThresholds();
        OnMatterChanged?.Invoke(matter);
    }
    public void RemoveMatter(float amount)
    {
        matter = Mathf.Round((matter - amount) * 100f) / 100f;
        OnMatterChanged?.Invoke(matter);
    }
    public bool HasEnoughMatter(float amount)
    {
        return matter >= amount;
    }
    public float GetMatter()
    {
        return matter;
    }

    private void CheckThresholds()
    {
        foreach (var threshold in new List<int>(_thresholdCallbacks.Keys))
        {
            if (matter >= threshold)
            {
                _thresholdCallbacks[threshold]?.Invoke();
                _thresholdCallbacks.Remove(threshold);
                _firedThresholds.Add(threshold);
            }
        }
    }

    void Start()
    {
        OnMatterChanged?.Invoke(matter);
    }

    void Update()
    {
        
    }
}
