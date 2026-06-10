using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class BuildingListObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private TextMeshProUGUI buildingCountText;

    [SerializeField] private BuildingDefinition buildingDefinition;
    [SerializeField] private BuildingManager buildingManager;
    [SerializeField] private BuildButton bb1;
    [SerializeField] private BuildButton bb10;
    [SerializeField] private BuildButton bb100;
    [SerializeField] private BuildButton bbMax;

    private Canvas _parentCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _parentCanvas = GetComponentInParent<Canvas>();
    }
    public void Initialize(BuildingDefinition definition, BuildingManager manager)
    {
        buildingDefinition = definition;
        buildingManager = manager;
        buildingNameText.text = definition.buildingName;
        buildingManager.OnBuildingsChanged += UpdateCount;
        bb1.Initialize(definition, manager);
        bb10.Initialize(definition, manager);
        bb100.Initialize(definition, manager);
        bbMax.Initialize(definition, manager);
        UpdateCount();
    }

    void OnDestroy()
    {
        if (buildingManager != null)
            buildingManager.OnBuildingsChanged -= UpdateCount;
    }

    private void UpdateCount()
    {
        if (buildingCountText != null)
            buildingCountText.text = buildingManager.GetCount(buildingDefinition).ToString();
    }

    public void OnBuildButtonPressed()
    {
        if (buildingManager.TryBuild(buildingDefinition))
        {
            Debug.Log($"Built {buildingDefinition.buildingName}!");
        }
        else
        {
            Debug.Log($"Failed to build {buildingDefinition.buildingName}.");
        }
    }
    public void OnBuildTenButtonPressed()
    {
        bool builtAtLeastOne = false;
        for (int i = 0; i < 10; i++)
        {
            if (buildingManager.TryBuild(buildingDefinition))
            {
                builtAtLeastOne = true;
            }
            else
            {
                break;
            }
        }
        if (builtAtLeastOne)
            Debug.Log($"Built up to 10 {buildingDefinition.buildingName}!");
        else
            Debug.Log($"Failed to build any {buildingDefinition.buildingName}.");
    }
    public void OnBuildHundredButtonPressed()
    {
        bool builtAtLeastOne = false;
        for (int i = 0; i < 100; i++)
        {
            if (buildingManager.TryBuild(buildingDefinition))
            {
                builtAtLeastOne = true;
            }
            else
            {
                break;
            }
        }
        if (builtAtLeastOne)
            Debug.Log($"Built up to 100 {buildingDefinition.buildingName}!");
        else
            Debug.Log($"Failed to build any {buildingDefinition.buildingName}.");
    }
    public void OnMaxBuildButtonPressed()
    {
        bool builtAtLeastOne = false;
        while (buildingManager.TryBuild(buildingDefinition))
        {
            builtAtLeastOne = true;
        }
        if (builtAtLeastOne)
            Debug.Log($"Built as many {buildingDefinition.buildingName} as possible!");
        else
            Debug.Log($"Failed to build any {buildingDefinition.buildingName}.");
    }
    public void OnDemolishButtonPressed()
    {
        if (buildingManager.TryDemolish(buildingDefinition))
        {
            Debug.Log($"Demolished {buildingDefinition.buildingName}!");
        }
        else
        {
            Debug.Log($"Failed to demolish {buildingDefinition.buildingName}.");
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        buildingManager.costPanel.UpdateCostText(buildingDefinition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buildingManager.costPanel.UpdateCostText(null);
    }


}
