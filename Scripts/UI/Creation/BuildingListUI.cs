using System.Collections.Generic;
using UnityEngine;

public class BuildingListUI : MonoBehaviour
{
    [SerializeField] private Transform buildingListContainer;
    [SerializeField] private GameObject buildingListItemPrefab;
    [SerializeField] private BuildingUnlockManager buildingUnlockManager;
    [SerializeField] private BuildingManager buildingManager;
    public void PopulateBuildingList()
    {
        ClearBuildingList();
        HashSet<BuildingDefinition> unlockedBuildings = new HashSet<BuildingDefinition>();
        foreach (var building in buildingUnlockManager.GetUnlockedBuildings())
        {
            unlockedBuildings.Add(building);
        }
    }
    private void ClearBuildingList()
    {
        foreach (Transform child in buildingListContainer)
        {
            Destroy(child.gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
