using System.Collections.Generic;
using UnityEngine;

public class ResourceListUI : MonoBehaviour
{
    [SerializeField] private ProductionManager productionManager;
    [SerializeField] private Transform panel;
    [SerializeField] private GameObject resourceButtonPrefab;
    [Tooltip("Every ResourceDefinition SO in the game. ResourceListUI will decide which ones to show.")]
    [SerializeField] private ResourceDefinition[] allResources;

    private readonly HashSet<ResourceDefinition> _visibleResources = new();

    void Start()
    {
        // Spawn rows for resources flagged as visible from the start.
        if (allResources != null)
        {
            foreach (var def in allResources)
            {
                if (def != null && def.showFromStart)
                    SpawnRow(def);
            }
        }

        productionManager.OnResourceChanged += HandleResourceChanged;
    }

    void OnDestroy()
    {
        if (productionManager != null)
            productionManager.OnResourceChanged -= HandleResourceChanged;
    }

    private void HandleResourceChanged(ResourceDefinition def, float amount)
    {
        if (amount > 0f && !_visibleResources.Contains(def))
            SpawnRow(def);
    }

    private void SpawnRow(ResourceDefinition def)
    {
        if (resourceButtonPrefab == null || panel == null) return;
        _visibleResources.Add(def);
        var obj = Instantiate(resourceButtonPrefab, panel);
        var listObj = obj.GetComponent<ResourceListObject>();
        if (listObj != null)
            listObj.Initialize(def, productionManager);
    }
}
