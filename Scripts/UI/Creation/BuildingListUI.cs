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
        foreach (var building in buildingUnlockManager.GetUnlockedBuildings())
        {
            var obj = Instantiate(buildingListItemPrefab, buildingListContainer);
            var listObj = obj.GetComponent<BuildingListObject>();
            if (listObj != null)
                listObj.Initialize(building, buildingManager);
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
        PopulateBuildingList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
